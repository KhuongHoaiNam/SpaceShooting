using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PopupSelectLevel : View
{
    public TotalLevelData totalLevel;

    public List<CellLevel> cellLevel;
    public CellLevel cellLevela;
    public RectTransform RtfCell;
    private bool isGenerated = false;
   
    public override void OnShow()
    {
        base.OnShow();


        // Kiểm tra nếu các đối tượng đã được sinh ra thì không thực hiện lại
        /*  if (!isGenerated)
          {
              for (int i = 0; i < totalLevel.maps[Datamanager.Instance.MapPlaying].levels.Count; i++)
              {
                  CellLevel cell = Instantiate(cellLevela, RtfCell.transform.position, Quaternion.identity, RtfCell);
                  cell.SetUp(i);
              }

              // Đánh dấu là đã sinh ra các đối tượng
              isGenerated = true;
          }*/
        /* for (int i = 0; i < totalLevel.maps[0].levels.Count; i++) {
             CellLevel cell = Instantiate(cellLevel, RtfCell.transform.position, Quaternion.identity, RtfCell);
             cell.SetUp(i);
         }*/

        for (int i = 0; i < cellLevel.Count; i++)
        {

            cellLevel[i].SetUp(i);
        }
    }
}
