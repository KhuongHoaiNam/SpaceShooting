using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Teesst : MonoBehaviour
{
    public TextMeshProUGUI txtCoin;

    public void Start()
    {
        if (txtCoin != null)
        {
            txtCoin.text = Datamanager.Instance.GetItemAmount(Item.coin).ToString();
        }
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Datamanager.Instance.UpdateItem(Item.coin, 100);
            if (txtCoin != null)
            {
                txtCoin.text = Datamanager.Instance.GetItemAmount(Item.coin).ToString();
            }
        }

        // Xóa dữ liệu khi nhấn phím D
        if (Input.GetKeyDown(KeyCode.D))
        {
            if (txtCoin != null)
            {
                txtCoin.text = Datamanager.Instance.GetItemAmount(Item.coin).ToString();
            }
        }
    }
    /*
        public void Start()
        {
            if (txtCoin != null)
            {
                txtCoin.text = Datamanager.Instance.user.level.ToString();
            }
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                Datamanager.Instance.user.level++;
                Datamanager.Instance.Save();
                if (txtCoin != null)
                {
                    txtCoin.text = Datamanager.Instance.user.level.ToString();
                }
            }
        }*/
}
