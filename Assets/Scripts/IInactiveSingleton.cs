using UnityEngine;

namespace Fishing {
    public abstract class IInactiveSingleton : MonoBehaviour {
        public abstract void SetInstanceReference();
        public abstract void SetDepenencyReferences();
    }

}