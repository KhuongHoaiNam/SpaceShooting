using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class DropItemManager : SingletonMono<DropItemManager>
{
    [SerializeField] private DropItemData dataTableItemDrop;

    public void Start()
    {
        //dataTableItemDrop = AssetDatabase.LoadAssetAtPath<DropItemData>("Assets/DataDropItem.asset");

    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
           DropItem(Item.upgradeitem, 1, this.transform);
        }
    }
    public void DropItem(Item id, int value , Transform _pos)
    {
        // Tìm kiếm item trong danh sách
        DataItemDrop data = dataTableItemDrop.listItemDrop.Find(x => x.idItem == id);

        // Tải tài nguyên từ thư mục Resources/Items
        GameObject itemPrefab = Resources.Load<GameObject>("Items/" + data.idItem.ToString());

        if (itemPrefab != null)
        {
            // Tạo ra số lượng item tương ứng với 'value' tại vị trí mong muốn
            for (int i = 0; i < value; i++)
            {
                Instantiate(itemPrefab, _pos.position, Quaternion.identity);
            }
        }
        else
        {
            Debug.LogWarning("Item prefab not found in Resources/Items/" + data.idItem.ToString());
        }

    }
}
