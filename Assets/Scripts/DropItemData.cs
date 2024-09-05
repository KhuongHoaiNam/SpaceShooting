using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu (fileName = "DataDropItem", menuName = "DropItem")]
public class DropItemData : ScriptableObject
{
    public List<DataItemDrop> listItemDrop;
    
}
[Serializable]
public struct DataItemDrop
{
    public Item idItem;
}