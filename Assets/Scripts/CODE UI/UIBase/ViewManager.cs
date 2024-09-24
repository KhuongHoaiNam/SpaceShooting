using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewManager : SingletonMono<ViewManager>
{
    public UIPooling uiPooling;
    public ViewIndex viewindex;
    private Dictionary<ViewIndex, string> viewDictionary;
    private List<ViewIndex> Stackview = new List<ViewIndex>();

    private View currentView;
    private View nextView;

    public View Currentview { get { return currentView; } }
    public void Start()
    {
    }
    public void Awake()
    {
        StartCoroutine(Init());

    }
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            SwitchView(viewindex);
        }
    }

    private IEnumerator Init()
    {
        yield return new WaitForEndOfFrame();
        viewDictionary = new Dictionary<ViewIndex, string>();
        Stackview = new List<ViewIndex>();
        //khai bao cac ten cuar popup d goi hien thi
        viewDictionary[ViewIndex.MennuView] = "MennuView";
        viewDictionary[ViewIndex.PopupWinView] = "PopupWinview";
        viewDictionary[ViewIndex.PopupLoseView] = "PopupLoseView";
        viewDictionary[ViewIndex.SelectLevelView] = "SelectLevelView";
        viewDictionary[ViewIndex.SelectMapView] = "SelectMapView";

        SwitchView(viewindex);
    }
    public void _SwitchView(ViewIndex view)
    {
        viewindex = view;
        if (Stackview.Count > 0)
        {
            int index = Stackview.Count - 1;
            if (Stackview[index] == viewindex)
            {

                Stackview.RemoveAt(index);
            }
            else if(currentView != null)
            {
                Stackview.Add(currentView.viewIndex);
            }
        }
        else if(currentView != null)
        {
            Stackview.Add(currentView.viewIndex);
        }
        if (currentView == null)
        {
            LoadNextView();
        }
        else
        {
            StartCoroutine("ExecuteWalkOut");
        }
    }
    private void LoadNextView()
    {
        if(nextView != null)
        {
            uiPooling.DeSpawner(nextView.transform);
        }
        nextView = uiPooling.Spawner(viewDictionary[viewindex]).GetComponent<View>();
        if(nextView == null)
        {
            UnityEngine.Debug.LogError("[ViewManager] Not found " + viewindex.ToString());
            return;
        }
    }

    public static void SwitchView(ViewIndex view)
    {
       SingletonMono<ViewManager>.Instance._SwitchView(view);
    }
}
