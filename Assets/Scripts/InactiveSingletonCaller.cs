using System.Collections.Generic;
using UnityEngine;

namespace Fishing {
    public class InactiveSingletonCaller : MonoBehaviour {
        private List<InactiveSingleton> _inactiveSingletons;

        private void OnEnable() { // Using OnOnEnable for this instead of Awake as this shouldn't be set to inactive at any point, and this allows the scene to instantiate everything in Awake first, before their depenencies are required in Start
            _inactiveSingletons = new List<InactiveSingleton>(FindObjectsOfType<InactiveSingleton>(true));
            for (int i = 0; i < _inactiveSingletons.Count; i++) {
                _inactiveSingletons[i].SetInstanceReference();
            }
        }
        private void Start() {
            for (int i = 0; i < _inactiveSingletons.Count; i++) {
                _inactiveSingletons[i].SetDepenencyReferences();
            }
        }
    }
}