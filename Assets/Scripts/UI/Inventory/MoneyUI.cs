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
            moneyText.text = "$" + PlayerData.instance.money.ToString();
        }
    }
}
