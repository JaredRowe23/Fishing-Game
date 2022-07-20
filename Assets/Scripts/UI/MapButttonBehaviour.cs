using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Fishing
{
    public class MapButttonBehaviour : MonoBehaviour
    {
        public void OpenMap()
        {
            SceneManager.LoadScene("World Map");
        }
    }
}
