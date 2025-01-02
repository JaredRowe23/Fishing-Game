using UnityEngine;

namespace Fishing.IO {
    [System.Serializable]
    public class HasSeenTutorialData {
        [SerializeField] private bool _castTutorial;
        public bool CastTutorial { get => _castTutorial; set => _castTutorial = value; }

        [SerializeField] private bool _reelingTutorial;
        public bool ReelingTutorial { get => _reelingTutorial; set => _reelingTutorial = value; }

        [SerializeField] private bool _reelingMinigameTutorial;
        public bool ReelingMinigameTutorial { get => _reelingMinigameTutorial; set => _reelingMinigameTutorial = value; }

        [SerializeField] private bool _bucketTutorial;
        public bool BucketTutorial { get => _bucketTutorial; set => _bucketTutorial = value; }

        [SerializeField] private bool _bucketMenuTutorial;
        public bool BucketMenuTutorial { get => _bucketMenuTutorial; set => _bucketMenuTutorial = value; }

        [SerializeField] private bool _baitTutorial;
        public bool BaitTutorial { get => _baitTutorial; set => _baitTutorial = value; }

        [SerializeField] private bool _inventoryTutorial;
        public bool InventoryTutorial { get => _inventoryTutorial; set => _inventoryTutorial = value; }

        [SerializeField] private bool _fishTutorial;
        public bool FishTutorial { get => _fishTutorial; set => _fishTutorial = value; }

        [SerializeField] private bool _npcTutorial;
        public bool NPCTutorial { get => _npcTutorial; set => _npcTutorial = value; }

        public HasSeenTutorialData(bool setSeenToAll) {
            CastTutorial = ReelingTutorial = ReelingMinigameTutorial = BucketTutorial = BucketMenuTutorial = BaitTutorial = InventoryTutorial = FishTutorial = NPCTutorial = setSeenToAll;
        }
    }
}
