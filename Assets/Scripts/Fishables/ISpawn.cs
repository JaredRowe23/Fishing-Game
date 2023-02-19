using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing
{
    public interface ISpawn
    {

        void Spawn();

        void RemoveFromList(GameObject _go);
    }
}
