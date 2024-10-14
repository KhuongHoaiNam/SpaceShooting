using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CellLevel : MonoBehaviour
{
    private int idLelevel;
    [SerializeField] private Button btnClick;
    [SerializeField] private TextMeshProUGUI txtLevel;
    [SerializeField] private Image[] imgStar;
    [SerializeField] private Sprite sprStar;
    [SerializeField] private GameObject objLock;
    public void OnEnable()
    {
        btnClick.onClick.RemoveListener(OnLoadGame);
        btnClick.onClick.AddListener(OnLoadGame);
    }

    public void SetUp(int id)
    {
        idLelevel = id;
        this.gameObject.name = $" level{idLelevel}";
        txtLevel.text = id.ToString();
        if (idLelevel <= Datamanager.Instance.user.currentLevel)
        {

            btnClick.interactable = true;
            objLock.SetActive(false);
            Debug.Log(Datamanager.Instance.user.currentLevel);

        }
        else 
        {
            btnClick.interactable = false;
            objLock.SetActive(true);
        }
     

    }


    public void OnLoadGame()
    {

        if (idLelevel <= Datamanager.Instance.user.currentLevel)
        {

            Datamanager.Instance.user.levelPlaying = idLelevel;
            Loader.Instance.Loading(SenceId.GameSence);

        }

    }
}
