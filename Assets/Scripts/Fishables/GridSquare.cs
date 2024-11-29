using Fishing.Fishables.Fish;
using System;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

namespace Fishing.Fishables {
    [Serializable]
    public class GridSquare {
        [Min(0)]
		private int _gridX;
		public int GridX { get => _gridX; private set { _gridX = Mathf.Max(0, value); } }

        [Min(0)]
        private int _gridY;
        public int GridY { get => _gridY; private set { _gridY = Mathf.Max(0, value); } }

        private Vector2 _gridCenter;
        public Vector2 GridCenter { get { return _gridCenter; } private set { _gridCenter = value; } }


        private bool _isCollidingWithTerrain;
        public bool IsCollidingWithTerrain { get => _isCollidingWithTerrain; set { _isCollidingWithTerrain = value; } }


        private List<Fishable> _gridFishables;
        public List<Fishable> GridFishables { get => _gridFishables; private set { _gridFishables = value; } }

        private List<Edible> _gridEdibles;
        public List<Edible> GridEdibles { get => _gridEdibles; set { _gridEdibles = value; } }

		public GridSquare(int _xPos, int _yPos) {
            GridX = _xPos;
            GridY = _yPos;

            FishableGrid fishGrid = FishableGrid.instance;
            GridCenter = (Vector2)fishGrid.transform.position + new Vector2(GridX * fishGrid.GridSquareSize + fishGrid.GridSquareHalfSize, -fishGrid.GridHeight + (GridY * fishGrid.GridSquareSize) + fishGrid.GridSquareHalfSize);

            GridFishables = new List<Fishable>();
            GridEdibles = new List<Edible>();
		}
	}
}
