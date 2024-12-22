using UnityEngine;

namespace Fishing {
    public abstract class InactiveSingleton : MonoBehaviour {
        public abstract void SetInstanceReference();
        public abstract void SetDepenencyReferences();
    }

}