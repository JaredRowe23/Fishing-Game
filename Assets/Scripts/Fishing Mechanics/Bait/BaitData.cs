using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing.FishingMechanics
{
    public struct BaitData
    {
        public readonly Vector2 baitPos;
        public readonly float baitRange;
        public readonly long baitTypes;

        public readonly Vector2 fishPos;
        public readonly int fishType;

        public bool isBaited;


        public BaitData(Vector2 _baitPos, float _baitRange, long _baitTypes, Vector2 _fishPos, int _fishType)
        {
            baitPos = _baitPos;
            baitRange = _baitRange;
            fishPos = _fishPos;
            fishType = _fishType;
            baitTypes = _baitTypes;
            isBaited = false;
        }

        public void Update()
        {
            isBaited = CheckBait();
        }

        private bool CheckBait()
        {
            float _distance = Vector2.Distance(fishPos, baitPos);

            if (!IsDesiredType())
            {
                return false;
            }

            if (_distance <= baitRange)
            {
                return true;
            }

            return false;
        }

        private bool IsDesiredType()
        {
            int[] _typeArray = GetTypesArray(baitTypes);

            for (int i = 0; i < _typeArray.Length; i++)
            {
                if (_typeArray[i] == fishType) return true;
            }

            return false;
        }

        private int[] GetTypesArray(long _baitTypes)
        {
            int[] _digitArray = new int[(int)(Mathf.Floor(Mathf.Log10((long)_baitTypes) + 1) - 1)];
            int[] _typeArray = new int[(int)(_digitArray.Length * 0.5f)];

            long num = _baitTypes;
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
    }
}
