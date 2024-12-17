using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fishing.IO;

namespace Fishing
{
    public class PlayerNameUI : MonoBehaviour {
        [SerializeField] private Text playerNameText;

        void Update() {
            playerNameText.text = SaveManager.Instance.LoadedPlayerData.SaveFileData.PlayerName;
        }
    }
}
