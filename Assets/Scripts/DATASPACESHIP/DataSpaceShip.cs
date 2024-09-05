using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

//đây là dữ liệu củ một phi thuyền gồm có các level của phi thuyền đó ở trong game 
[CreateAssetMenu(fileName = "DataSpaceShip", menuName = "GolobaDataScpaceShip")]
public class DataSpaceShip : ScriptableObject
{
    public List<DataConfigLevelSpaceShip> dataConfigLevelSpaceShips;
    public void SpawnGunbaseForLevel(int level ,Transform pos)
    {
        DataConfigLevelSpaceShip ship = dataConfigLevelSpaceShips.Find(x => x.level == level);

        if (ship.gun != null)
        {
            // Sinh ra Gunbase tại vị trí pos
            GameObject gunObject = Instantiate(ship.gun, pos.position, pos.rotation);

            // Cập nhật thuộc tính của đối tượng Gunbase
            Gunbase gunbase = gunObject.GetComponent<Gunbase>();
            if (gunbase != null)
            {
                gunbase.dame = ship.Dm;
                gunbase.transform.SetParent(pos, true);
            }
            else
            {
                Debug.LogError("Không tìm thấy component Gunbase trên đối tượng sinh ra.");
            }
        }
        else
        {
            Debug.LogError("Không tìm thấy prefab Gunbase cho level " + level);
        }
    }


}
[Serializable]
public struct DataConfigLevelSpaceShip
{
    public int level;
    public float Hp;
    public float Dm;
    public GameObject gun;
}