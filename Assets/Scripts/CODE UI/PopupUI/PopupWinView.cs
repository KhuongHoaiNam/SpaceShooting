using System.Collections;
using System.Collections.Generic;
using Unity.Loading;
using UnityEngine;
using UnityEngine.UI;

public class PopupWinView : View
{
    [SerializeField] private Button btnNext;
    [SerializeField] private Button btnHome;

    public override void OnShow()
    {
        base.OnShow();
        btnNext.onClick.RemoveListener(OnClickNext);
        btnNext.onClick.AddListener(OnClickNext);

        btnHome.onClick.RemoveListener(OnClickHome);
        btnHome.onClick.AddListener(OnClickHome);
        Datamanager.Instance. ComplateLeves();
    }

    public void OnClickNext()
    {
        this.gameObject.SetActive(false);
        Loader.Instance.ResetSence();
        Loader.Instance.Loading(SenceId.GameSence);
    }
    public void OnClickHome()
    {
        this.gameObject.SetActive(false);
       Loader.Instance.LoadGameSelectView(SenceId.MennuView, ViewIndex.MennuView);
    }
}
