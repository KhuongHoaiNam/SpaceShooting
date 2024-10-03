using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class Loader : SingletonMono<Loader>
{
    public float timeEndLoadGame = 4f;
    private float timeStartLoadGame = 0f;
  public bool loadGame;

    public SenceId seneceId;
    [SerializeField] private GameObject Loadbar;
    [SerializeField] private Image imageLoading;

    private void Start()
    {
        loadGame = true;
    }

    private void Update()
    {
        if(loadGame == true)
        {
            Loadbar.SetActive(true);
            this.gameObject.SetActive(true);
            timeStartLoadGame += Time.deltaTime;
            float fillAmout = Mathf.Clamp01(timeStartLoadGame / timeEndLoadGame     );
            imageLoading.fillAmount = fillAmout;

            if(timeStartLoadGame >= timeEndLoadGame)
            {
                imageLoading.fillAmount = 1f;
                Loadbar.SetActive(false);
                loadGame = false;
                LoadingView(SenceId.MennuView, ViewIndex.MennuView);
            }
        }
    }

    public void Loading(SenceId senceId )
    {
        ViewManager.Instance.CloseAllViews();
        SceneManager.LoadScene(senceId.ToString());
        
    }
   
    public void LoadingView(SenceId senceId, ViewIndex viewIndex)
    {
        ViewManager.Instance.viewindex = viewIndex;
        SceneManager.LoadScene(viewIndex.ToString());
    }
    public void ResetSence()
    {

    }
}

