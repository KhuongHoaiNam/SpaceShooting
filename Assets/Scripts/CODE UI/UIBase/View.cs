
using UnityEngine;
using UnityEngine.UI;

public abstract class View : MonoBehaviour
{
    public ViewIndex viewIndex;
    public Button btnClose;

    public void OnEnable()
    {
        OnShow();
        if(btnClose != null)
        {
            btnClose.onClick.RemoveListener(OnHide);
            btnClose.onClick.AddListener(OnHide);
        }
    }
    public virtual void OnShow()
    {

    }

    public virtual void OnHide() 
    { 
        this.gameObject.SetActive(false);
    
    }
}
