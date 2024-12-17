using UnityEngine;

namespace Fishing.Fishables.Fish {
    public class Edible : MonoBehaviour {
        [SerializeField, Min(0), Tooltip("The base amount of food for a \"normal\" sized fish of this type.")] private float _baseFoodAmount = 10f;
        public float BaseFoodAmount { get => _baseFoodAmount; private set { } }
    }
}
