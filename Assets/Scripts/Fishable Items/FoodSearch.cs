using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Jobs;
using Unity.Collections;
using System.Linq;

public class FoodSearch : MonoBehaviour
{
    public struct Data
    {
        readonly Vector3 position;
        readonly Vector3 forward;

        readonly float sightRange;
        readonly float sightAngle;

        readonly float smellRange;

        public int foodSearchIndex;
        public int thisIndex;

        public Vector3 toCheckPos;
        public int toCheckType;
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
        }

        public void Update()
        {
            nearestFoodIndex = Search();
        }

        private int Search()
        {
            if (toCheckPos.y >= 0)
            {
                //Debug.Log("food is above water");
                return 0;
            }

            float _distance = Vector3.Distance(toCheckPos, position);
            if (_distance == 0)
            {
                //Debug.Log("iterating over self");
                return 0;
            }

            if (_distance > sightRange)
            {
                //Debug.Log("out of sight range: range = " + sightRange.ToString() + ", distance = " + _distance.ToString());
                return 0;
            }

            int[] typeArray = GetTypesArray(types);

            bool desiredType = false;
            for (int i = 0; i < typeArray.Length; i++)
            {
                if (typeArray[i] != toCheckType) continue;
                desiredType = true;
            }
            if (desiredType == false)
            {
                //Debug.Log("not desired fish type");
                return 0;
            }




            if (_distance <= smellRange)
            {
                //Debug.Log("in smell range: range = " + smellRange.ToString() + ", distance = " + _distance.ToString());
                return 1;
            }

            float dot = Vector2.Dot(forward, Vector3.Normalize(GlobalToLocal()));
            float angle = Mathf.Acos(dot) * 180 * 0.3183098861928886f;
            if (angle < sightAngle)
            {
                //Debug.Log("within sight angle: sightAngle = " + sightAngle + ", angle = " + angle);
                return 1;
            }

            //if (_distance > nearestFoodDistance && nearestFoodDistance != -1)
            //{
            //    Debug.Log("further than current food: currentFood = " + nearestFoodDistance + ", distance = " + _distance);
            //    return 0;
            //}

            return 0;
        }

        private int[] GetTypesArray(long _types)
        {
            int[] digitArray = new int[(int)(Mathf.Floor(Mathf.Log10((long)_types) + 1) - 1)];
            int[] typeArray = new int[(int)(digitArray.Length * 0.5f)];

            long num = _types;
            for (int i = 0; i < digitArray.Length; i++)
            {
                if (num == 1)
                {
                    break;
                }
                digitArray[digitArray.Length - 1 - i] = (int)(num % 10);
                num = (long)(num / 10);
            }

            for (int i = 0; i < typeArray.Length; i++)
            {
                typeArray[i] = digitArray[i * 2] * 10 + digitArray[(i * 2) + 1];
            }
            return typeArray;
        }

        private Vector3 GlobalToLocal() => (toCheckPos - position);
    }

    [SerializeField] private float sightAngle;
    [SerializeField] private float sightDistance;
    [SerializeField] private float sightDensity;
    [SerializeField] private float smellRadius;
    [SerializeField] private float chaseDistance;
    [SerializeField] private FishableItem.FoodTypes[] desiredFoodTypes;
    public GameObject desiredFood;
    private FishableItem desiredFishableItem;
    private HookObject desiredHookObject;

    //    // If we're chasing the hook, check if we've already hooked something
    //    if (desiredHookObject)
    //    {
    //        if (desiredHookObject.hookedObject != null)
    //        {
    //            UnsassignFood();
    //            return;
    //        }
    //    }

    //    if (desiredFishableItem)
    //    {
    //        if (desiredFishableItem.isHooked)
    //        {
    //            UnsassignFood();
    //            return;
    //        }
    //    }
    //}

    //public GameObject IsFood(GameObject food)
    //{
    //    // Rule out hook object if it's already hooked something
    //    if (food.GetComponent<HookObject>())
    //    {
    //        if (food.GetComponent<HookObject>().hookedObject != null) return null;
    //    }
    //    return null;
    //}

    //private void AssignFood(GameObject food)
    //{
    //    desiredFood = food;
    //    if (desiredFood.GetComponent<HookObject>()) desiredHookObject = desiredFood.GetComponent<HookObject>();
    //    else if (desiredFood.GetComponent<FishableItem>()) desiredFishableItem = desiredFood.GetComponent<FishableItem>();
    //}

    //private void UnsassignFood()
    //{
    //    desiredFood = null;
    //    desiredFishableItem = null;
    //    desiredHookObject = null;
    //}

    private void OnDrawGizmosSelected()
    {
        if (!desiredFood)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, smellRadius);

            for (int i = 0; i < sightDensity; i++)
            {
                Vector3 dir = -transform.right;
                dir = Quaternion.Euler(0f, 0f, -sightAngle + (i * sightAngle * 2 / sightDensity)) * dir;
                Debug.DrawRay(transform.position, dir * sightDistance, Color.green);
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
        string typesString = "1";
        for (int i = 0; i < desiredFoodTypes.Length; i++)
        {
            int typeInt = (int)desiredFoodTypes[i];
            if (typeInt < 10)
            {
                typesString += "0";
            }
            typesString += typeInt.ToString();
        }
        long types = long.Parse(typesString);
        return types;
    }
}
