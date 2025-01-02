using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing.Audio {
    [Serializable]
    public class SoundCategory {
        [SerializeField, Tooltip("Name of this category of sounds.")] private string _categoryName = "New Sound Category";
        public string CategoryName { get => _categoryName; }

        [SerializeField, Tooltip("List of sounds under this category.")] private List<Sound> _sounds;
        public List<Sound> Sounds { get => _sounds; }
    }
}