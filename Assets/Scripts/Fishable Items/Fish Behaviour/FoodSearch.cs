using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;
using System.Linq;

namespace Fishing.Fishables.Fish
{
    public class FoodSearch : MonoBehaviour
    {
        public struct Data
        {
            public readonly Vector3 position;
            readonly Vector3 forward;

            readonly float sightRange;
            readonly float sightAngle;

            readonly float smellRange;

            public int foodSearchIndex;
            public int thisIndex;

            public Vector3 toCheckPos;
            public int toCheckType;
            public bool isOccupiedHook;
            public long types;

            public int nearestFoodIndex;
            public float nearestFoodDistance;


            public Data(Vector3 _pos, Vector3 _forward, float _sightRange, float _sightAngle, float _smellRange, int _index, long _types)
            {
                position = _pos;
                forward = _forward;
                sightRange = _sightRange;
                sightAngle = _sightAngle;
                smellRange = _smellRange;
                toCheckPos = Vector3.zero;
                toCheckType = 0;
                nearestFoodIndex = -1;
                nearestFoodDistance = -1f;
                foodSearchIndex = 0;
                thisIndex = _index;
                types = _types;
                isOccupiedHook = false;
            }

            public void Update()
            {
                nearestFoodIndex = Search();
            }

            private int Search()
            {
                if (isOccupiedHook)
                {
                    if (nearestFoodIndex == foodSearchIndex)
                    {
                        return -1;
                    } 
                    else return nearestFoodIndex;
                }

                if (toCheckPos.y >= 0)
                {
                    return nearestFoodIndex;
                }

                float _distance = Vector3.Distance(toCheckPos, position);
                if (_distance == 0)
                {
                    return nearestFoodIndex;
                }

                if (_distance > sightRange)
                {
                    return nearestFoodIndex;
                }

                if (nearestFoodDistance != -1)
                {
                    if (_distance > nearestFoodDistance)
                    {
                        return nearestFoodIndex;
                    }
                }

                if (!IsDesiredType())
                {
                    return nearestFoodIndex;
                }

                if (_distance <= smellRange)
                {
                    nearestFoodDistance = _distance;
                    return foodSearchIndex;
                }

                if (IsInSightAngle())
                {
                    nearestFoodDistance = _distance;
                    return foodSearchIndex;
                }


                return nearestFoodIndex;
            }

            private bool IsDesiredType()
            {
                int[] _typeArray = GetTypesArray(types);

                for (int i = 0; i < _typeArray.Length; i++)
                {
                    if (_typeArray[i] == toCheckType) return true;
                }

                return false;
            }

            private bool IsInSightAngle()
            {
                float _dot = Vector2.Dot(forward, Vector3.Normalize(GlobalToLocal()));
                float _angle = Mathf.Acos(_dot) * 180 * 0.3183098861928886f;
                if (_angle < sightAngle) return true;
                return false;
            }

            private int[] GetTypesArray(long _types)
            {
                int[] _digitArray = new int[(int)(Mathf.Floor(Mathf.Log10((long)_types) + 1) - 1)];
                int[] _typeArray = new int[(int)(_digitArray.Length * 0.5f)];

                long num = _types;
                for (int i = 0; i < _digitArray.Length; i++)
                {
                    if (num == 1)
                    {
                        break;
                    }
                    _digitArray[_digitArray.Length - 1 - i] = (int)(num % 10);
                    num = (long)(num / 10);
                }

                for (int i = 0; i < _typeArray.Length; i++)
                {
                    _typeArray[i] = _digitArray[i * 2] * 10 + _digitArray[(i * 2) + 1];
                }
                return _typeArray;
            }

            private Vector3 GlobalToLocal() => (toCheckPos - position);
        }

        [SerializeField] private float sightAngle;
        [SerializeField] private float sightDistance;
        [SerializeField] private float sightDensity;
        [SerializeField] private float smellRadius;
        [SerializeField] private float chaseDistance;
        [SerializeField] private Edible.FoodTypes[] desiredFoodTypes;
        public GameObject desiredFood;

        private void Start() => GameController.instance.AddFish(this);

        private void OnDrawGizmosSelected()
        {
            if (!desiredFood)
            {
                Gizmos.color = Color.green;
                Gizmos.DrawWireSphere(transform.position, smellRadius);

                for (int i = 0; i < sightDensity; i++)
                {
                    Vector3 _dir = -transform.right;
                    _dir = Quaternion.Euler(0f, 0f, -sightAngle + (i * sightAngle * 2 / sightDensity)) * _dir;
                    Debug.DrawRay(transform.position, _dir * sightDistance, Color.green);
                }
            }
            else
            {
                Debug.DrawRay(transform.position, (desiredFood.transform.position - transform.position), Color.red);
            }
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