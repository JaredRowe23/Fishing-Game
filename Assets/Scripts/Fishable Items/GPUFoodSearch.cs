using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPUFoodSearch : MonoBehaviour
{
    public struct Fish
    {
        Vector3 position;
        Vector3 forward;
        float sightRange;
        float sightAngle;
        float smellRange;
        public int target;

        public Fish(Vector3 _pos, Vector3 _forward, float _sightRange, float _sightAngle, float _smellRange, int _targetLength)
        {
            position = _pos;
            forward = _forward;
            sightRange = Mathf.FloatToHalf(_sightRange);
            sightAngle = Mathf.FloatToHalf(_sightAngle);
            smellRange = Mathf.FloatToHalf(_smellRange);
            target = -1;
        }
    }

    public ComputeShader foodComputeShader;
    public List<FoodSearch> fishes;
    private Fish[] data;

    private void Update()
    {
        Search();
    }

    private void Search()
    {
        data = new Fish[fishes.Count];
        GenerateFishData();


        int intSize = sizeof(int);
        int floatSize = sizeof(float);
        int vector3Size = sizeof(float) * 3;
        int totalSize = (intSize) + (floatSize * 3) + (vector3Size * 2);
        Debug.Log(intSize.ToString() + ", " + floatSize.ToString() + ", " + vector3Size.ToString() + ": " + totalSize.ToString());
        ComputeBuffer foodBuffer = new ComputeBuffer(fishes.Count, totalSize);
        foodBuffer.SetData(data);

        foodComputeShader.SetBuffer(0, "fishes", foodBuffer);
        foodComputeShader.Dispatch(0, fishes.Count / 32, 1, 1);

        foodBuffer.GetData(data);

        for (int i = 0; i < data.Length; i++)
        {
            if (data[i].target == -1) Debug.Log("-1");
            else if (data[i].target == -2) Debug.Log("-2");
            else if (data[i].target == -3) Debug.Log("-3");
            else if (data[i].target == -4) Debug.Log("-4");
            else if (data[i].target == -5) Debug.Log("-5");
            if (data[i].target > -1)
            {
                Debug.Log(data[i].target.ToString());
                //fishes[i].IsFood(fishes[data[i].target].gameObject);
            }
        }
        foodBuffer.Dispose();
    }

    private void GenerateFishData()
    {
        int i = 0;
        foreach(FoodSearch fish in fishes)
        {
            data[i] = new Fish(fish.transform.position, fish.transform.forward, fish.GetSightRange(), fish.GetSightAngle(), fish.GetSmellRange(), fishes.Count);
            i++;
        }
    }
}
