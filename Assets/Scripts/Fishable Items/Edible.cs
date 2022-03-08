using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing.Fishables
{
    public class Edible : MonoBehaviour
    {
        private void Start() => GameController.instance.AddFood(this);

        public enum FoodTypes { Hook, Fish1, SinkingTrash, Fish3, Fish4, Fish5, Fish6, Fish7, Fish8, Fish9, Fish10, Fish11 };
        [SerializeField] private FoodTypes foodType;

        public int GetFoodType() => (int)foodType;
    }
}
