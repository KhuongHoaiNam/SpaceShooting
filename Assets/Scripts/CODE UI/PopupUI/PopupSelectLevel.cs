using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupSelectLevel : View
{
    public TotalLevelData totalLevel;

    public CellLevel cellLevel;
    public RectTransform RtfCell;
    public override void OnShow()
    {
        base.OnShow();
        for (int i = 0; i < totalLevel.maps[0].levels.Count; i++) {
            CellLevel cell = Instantiate(cellLevel, RtfCell.transform.position, Quaternion.identity, RtfCell);
            cell.SetUp(i);
        }

    }
}
