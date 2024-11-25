using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.Fishables.Fish;
using System.Linq;

namespace Fishing.Fishables
{
	[Serializable]
    public class GridSquare {
        [Min(0)]
		private int _gridX;
		public int GridX { get => _gridX; private set { _gridX = Mathf.Max(0, value); } }

        [Min(0)]
        private int _gridY;
        public int GridY { get => _gridY; private set { _gridY = Mathf.Max(0, value); } }

        private List<Fishable> _gridFishables;
        public List<Fishable> GridFishables { 
            get {
                _gridFishables.RemoveAll(item => item == null); // TODO: Remove after source of null references is resolved
                return _gridFishables; 
            } 
            private set { } }

		public GridSquare(int _xPos, int _yPos) {
            _gridX = _xPos;
            _gridY = _yPos;
            _gridFishables = new List<Fishable>();
		}
	}
}
