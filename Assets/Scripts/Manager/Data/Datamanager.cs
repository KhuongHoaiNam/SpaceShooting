using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class Datamanager : SingletonMono<Datamanager>
{
    public UserData user;
    private string UserDataPath;

    public void Start() { }

    public void OnEnable()
    {
        string dataFolderPath = Path.Combine(Application.persistentDataPath, "Game Data");

        // Tạo thư mục nếu nó chưa tồn tại
        if (!Directory.Exists(dataFolderPath))
        {
            Directory.CreateDirectory(dataFolderPath);
        }

        // Gán giá trị cho UserDataPath
        UserDataPath = Path.Combine(dataFolderPath, "UserData.json");

        // Debug để kiểm tra xem đường dẫn có được tạo đúng không
        Debug.Log("UserDataPath: " + UserDataPath);

        // Load user data tại thời điểm bắt đầu
        Load();
    }

    public void Save()
    {
        // Chuyển dictionary thành list cho việc serialize
        user.serializableItems = new List<ItemAmount>();
        foreach (var kvp in user.items)
        {
            user.serializableItems.Add(new ItemAmount { item = kvp.Key, amount = kvp.Value });
        }

        string jsonData = JsonUtility.ToJson(user);
        File.WriteAllText(UserDataPath, jsonData);
        Debug.Log("Save Data +++++++++++++++++++++++++++++++++++++");
    }

    public void Load()
    {
        if (File.Exists(UserDataPath))
        {
            string jsonData = File.ReadAllText(UserDataPath);
            user = JsonUtility.FromJson<UserData>(jsonData);

            // Chuyển list lại thành dictionary
            user.items = new Dictionary<Item, int>();
            foreach (var itemAmount in user.serializableItems)
            {
                user.items[itemAmount.item] = itemAmount.amount;
            }
        }
        else
        {
            user = new UserData
            {
                currentLevel = 0,
                currentMap = 0,
                items = new Dictionary<Item, int>
                {
                    { Item.coin, 0 }
                }, 
                starCollection = new Dictionary<int, Dictionary<int, int>>()
            };
            Save();
            Debug.Log("Load Data ============================");
        }
    }
    public void DeleteUserData()
    {
        // Xóa file UserData.json nếu tồn tại
        if (File.Exists(UserDataPath))
        {
            File.Delete(UserDataPath);
            Debug.Log("Đã xóa dữ liệu người dùng.");
        }
        else
        {
            Debug.LogWarning("Không tìm thấy dữ liệu người dùng để xóa.");
        }

        // Khởi tạo lại dữ liệu người dùng với giá trị mặc định
        user = new UserData
        {
            currentLevel = 0,
            currentMap = 0,
            items = new Dictionary<Item, int>
        {
            { Item.coin, 0 }
        }
        };

        Save(); // Lưu lại dữ liệu mặc định
        Debug.Log("Dữ liệu người dùng đã được khởi tạo lại.");
    }

    public void UpdateItem(Item item, int amount)
    {
        if (user.items.ContainsKey(item))
        {
            user.items[item] += amount;
        }
        else
        {
            user.items[item] = amount;
        }
        Save();
    }

    public int GetItemAmount(Item idItem)
    {
        if (user == null || user.items == null)
        {
            Debug.LogWarning("User data or items dictionary is null.");
            return 0;
        }

        if (user.items.ContainsKey(idItem))
        {
            return user.items[idItem];
        }
        return 0;
    }


    public void UpdateStars(int mapId, int levels, int starIndex)
    {
        if (!user.starCollection.ContainsKey(mapId))
        {
            user.starCollection[mapId] = new Dictionary<int, int>();

        }
        if (user.starCollection[mapId].ContainsKey(levels))
        {
            user.starCollection[mapId][levels] = Math.Max(user.starCollection[mapId][levels], starIndex);
        }
        else
        {
            user.starCollection[mapId][levels] = starIndex;
        }
        Save();
        Debug.Log($"Cap nhap so sao cho level");
    }


    public int GetStars(int map, int levels)
    {
        if (user.starCollection.ContainsKey(map) && user.starCollection.ContainsKey(levels))
        {
            return user.starCollection[map][levels];
        }
        return 0;
    }


    public void ComplateLeves()
    {
        user.currentLevel++;
        Save();
    }


    
    [Serializable]
    public class UserData
    {
        public int currentLevel;
        public int currentMap;
        public int levelPlaying;
        public Dictionary<Item, int> items;
        public Dictionary<int, Dictionary<int, int>> starCollection;

        // List cho việc serialize
        public List<ItemAmount> serializableItems;
    }

    [Serializable]
    public class ItemAmount
    {
        public Item item;
        public int amount;
    }
}