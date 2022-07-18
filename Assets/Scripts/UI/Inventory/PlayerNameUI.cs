using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fishing.IO;

namespace Fishing
{
    public class PlayerNameUI : MonoBehaviour
    {

        [SerializeField] private Text playerNameText;

        // Update is called once per frame
        void Update()
        {
            playerNameText.text = PlayerData.instance.playerName;
        }
    }
}
