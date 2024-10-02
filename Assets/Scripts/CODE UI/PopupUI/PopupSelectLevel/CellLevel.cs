using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellLevel : MonoBehaviour
{
    public int idLelevel;
    public Button btnClick;
    public void OnEnable()
    {
        btnClick.onClick.RemoveListener(OnLoadGame);
        btnClick.onClick.AddListener(OnLoadGame);
    }

    public void SetUp(int id)
    {
        idLelevel = id;

    }

    public void OnLoadGame() 
    {
       
        if (idLelevel <= Datamanager.Instance.user.currentLevel) {

            Datamanager.Instance.user.levelPlaying = idLelevel;
            this.gameObject.SetActive(false);
            Loader.Instance.Loading(SenceId.GameSence);
           
        }

    }
}
