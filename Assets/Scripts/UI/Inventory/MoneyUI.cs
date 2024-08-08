using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fishing.IO;

namespace Fishing
{
    public class MoneyUI : MonoBehaviour
    {

        [SerializeField] private Text moneyText;

        void Update()
        {
            moneyText.text = "$" + PlayerData.instance.saveFileData.money.ToString("F2");
            if (moneyText.text.Substring(moneyText.text.Length - 2, 2) == "00") moneyText.text = "$" + PlayerData.instance.saveFileData.money.ToString();
        }
    }
}
