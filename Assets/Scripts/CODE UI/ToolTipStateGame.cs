using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ToolTipStateGame : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI txtToolTipTate;


    public void SetUpToolTip(string txt, int Value)
    {
        txtToolTipTate.text = $"{txt}/{Value.ToString()}";
    }
}
