using System.Collections;
using System.Collections.Generic;
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

    }

    public void OnClickNext()
    {

    }
    public void OnClickHome()
    {

    }
}
