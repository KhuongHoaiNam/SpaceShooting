using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupSelectMap : View
{
    public TotalLevelData mapsDatas;

    public List<CellSelectMaps> cellSelectMaps;
    public override void OnShow()
    {
        base.OnShow(); InitData();
    }

    public void InitData()
    {
        for (int i = 0; i < mapsDatas.maps.Count; i++) {
            cellSelectMaps[i].idMap = i;
        }
    }
}
