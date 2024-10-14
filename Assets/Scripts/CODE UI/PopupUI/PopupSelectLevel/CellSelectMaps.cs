using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellSelectMaps : MonoBehaviour
{
    public Button btnCellMap;
    public int idMap;
    public Image imgLock;

    public void OnEnable()
    {

        btnCellMap.onClick.RemoveListener(OnClickMap);
        btnCellMap.onClick.AddListener(OnClickMap);
 /*       var checkLock = Datamanager.Instance.user.levelUnlocked[idMap, 10];
        imgLock.gameObject.SetActive(checkLock);
        btnCellMap.interactable = checkLock;*/
    }
    public void OnClickMap()
    {
        ViewManager.SwitchView(ViewIndex.SelectLevelView);
    }
}
