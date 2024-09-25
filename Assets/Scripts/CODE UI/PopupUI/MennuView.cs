using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MennuView : View
{
    [SerializeField] private Button btnPlay;

    public override void OnShow()
    {
        base.OnShow();
        btnPlay.onClick.RemoveListener(OnClickPlay);
        btnPlay.onClick.AddListener(OnClickPlay);
    }

    public override void OnHide()
    {
        base.OnHide();
        Loader.Instance.loadGame = true;
        Loader.Instance.Loading(SenceId.LoadingSence);
    }

    public void OnClickPlay()
    {
        ViewManager.SwitchView(ViewIndex.SelectLevelView);
    }
}
