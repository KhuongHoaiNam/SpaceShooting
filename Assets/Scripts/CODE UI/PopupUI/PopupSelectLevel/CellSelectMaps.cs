using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellSelectMaps : MonoBehaviour
{
    public Button btnCellMap;
    public int idMap;

    public void OnEnable()
    {
        btnCellMap.onClick.RemoveListener(OnClickMap);
        btnCellMap.onClick.AddListener(OnClickMap);
    }
    public void OnClickMap()
    {
        Datamanager.Instance.MapPlaying = idMap;
        ViewManager.SwitchView(ViewIndex.SelectLevelView);
    }
}
