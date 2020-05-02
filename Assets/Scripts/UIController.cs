using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;

public class UIController : MonoBehaviour
{
    #region Editor
    [SerializeField]
    TextMeshProUGUI balance;
    [SerializeField]
    TextMeshProUGUI bet;
    #endregion

    public int Balance
    {
        set
        {
            balance.text = value.ToString();
        }
    }

    public int Bet
    {
        set
        {
            bet.text = value.ToString();
        }
    }
}

