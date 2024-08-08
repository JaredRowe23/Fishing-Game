using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing.IO
{
    [System.Serializable]
    public struct HasSeenTutorialData
    {
        public bool castTutorial;
        public bool reelingTutorial;
        public bool reelingMinigameTutorial;
        public bool bucketTutorial;
        public bool bucketMenuTutorial;
        public bool baitTutorial;
        public bool inventoryTutorial;
        public bool fishTutorial;
        public bool NPCTutorial;

        public HasSeenTutorialData(bool _setSeenToAll)
        {
            castTutorial = reelingTutorial = reelingMinigameTutorial = bucketTutorial = bucketMenuTutorial = baitTutorial = inventoryTutorial = fishTutorial = NPCTutorial = _setSeenToAll;
        }
    }
}
