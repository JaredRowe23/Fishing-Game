using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;
using System.Linq;
using Fishing.FishingMechanics;

namespace Fishing.Fishables.Fish
{
    public class FoodSearch : MonoBehaviour
    {
        [SerializeField] private float sightAngle;
        [SerializeField] private float sightDistance;
        [SerializeField] private float sightDensity;
        [SerializeField] private float smellRadius;
        [SerializeField] private float chaseDistance;
        [SerializeField] private Edible.FoodTypes[] desiredFoodTypes;
        public GameObject desiredFood;

        private void Start()
        {
            FoodSearchManager.instance.AddFish(this);
            BaitManager.instance.AddFish(this);
        }

        private void OnDrawGizmosSelected()
        {
            if (!desiredFood)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(transform.position, smellRadius);

                for (int i = 0; i < sightDensity; i++)
                {
                    Vector2 _dir = -transform.right;
                    _dir = Quaternion.Euler(0f, 0f, -sightAngle + (i * sightAngle * 2 / sightDensity)) * _dir;
                    Debug.DrawRay(transform.position, _dir * sightDistance, Color.green);
                }
            }
            else
            {
                Debug.DrawRay(transform.position, (desiredFood.transform.position - transform.position), Color.red);
            }
        }

        public List<int> DesiredTypesToInts()
        {
            List<int> _typesList = new List<int>();
            for (int i = 0; i < desiredFoodTypes.Length; i++)
            {
                int _typeInt = (int)desiredFoodTypes[i];
                _typesList.Add(_typeInt);
            }
            return _typesList;
        }

        public float GetSightRange() => sightDistance;
        public float GetSightAngle() => sightAngle;
        public float GetSmellRange() => smellRadius;

        public long GetFoodTypes()
        {
            if (desiredFoodTypes.Length > 9)
            {
                Debug.LogError("Too many food types assigned to object for c# long to handle!", this);
                return 0;
            }
            string _typesString = "1";
            for (int i = 0; i < desiredFoodTypes.Length; i++)
            {
                int _typeInt = (int)desiredFoodTypes[i];
                if (_typeInt < 10)
                {
                    _typesString += "0";
                }
                _typesString += _typeInt.ToString();
            }
            long types = long.Parse(_typesString);
            return types;
        }
    }

}