using Fishing.Fishables.Fish;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing.Fishables.FishGrid {
    [Serializable]
    public class GridSquare {
		private int _gridX;
		public int GridX { get => _gridX; private set { _gridX = Mathf.Max(0, value); } }

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

        private List<FoodSearch> _gridFoodSearches;
        public List<FoodSearch> GridFoodSearches { get => _gridFoodSearches; set { _gridFoodSearches = value; } }

		public GridSquare(int _xPos, int _yPos) {
            GridX = _xPos;
            GridY = _yPos;

            FishableGrid fishGrid = FishableGrid.instance;
            GridCenter = (Vector2)fishGrid.transform.position + new Vector2(GridX * fishGrid.GridSquareSize + fishGrid.GridSquareHalfSize, -fishGrid.GridHeight + (GridY * fishGrid.GridSquareSize) + fishGrid.GridSquareHalfSize);

            DetermineIfCollidingWithTerrain();

            GridFishables = new List<Fishable>();
            GridEdibles = new List<Edible>();
            GridFoodSearches = new List<FoodSearch>();
        }

        private void DetermineIfCollidingWithTerrain() {
            PolygonCollider2D[] terrainColliders = GameObject.FindGameObjectWithTag("Fishing Level Terrain").GetComponentsInChildren<PolygonCollider2D>(); // TODO: Replace with searching for a FishingLevelTerrain script
            for (int i = 0; i < terrainColliders.Length; i++) {
                if (IsGridSquareOutsideOfChunk(terrainColliders[i])) {
                    continue;
                }
                if (!AreGridEdgesCollidingWithCollider(terrainColliders[i])) {
                    continue;
                }

                // TODO: Add function for detecting if a section of terrain is completely encapsulated by a grid square.
                //       This should take each point of the terrain collider and raycast towards the grid center by it's extents length.
                //       If the raycast exits the grid square an odd amount of times, then the point it came from is within the grid square.

                IsCollidingWithTerrain = true;
                return;
            }
            IsCollidingWithTerrain = false;
        }

        private bool IsGridSquareOutsideOfChunk(PolygonCollider2D collider) {
            Bounds terrainBounds = collider.bounds;
            float distanceToCenter = Vector2.Distance(GridCenter, terrainBounds.center);
            float inBoundsRange = terrainBounds.extents.magnitude + FishableGrid.instance.GridSquareSize;
            bool isChunkTooFarAway = distanceToCenter >= inBoundsRange;
            return isChunkTooFarAway;
        }

        private bool AreGridEdgesCollidingWithCollider(PolygonCollider2D collider) {

            int terrainLayer = ~LayerMask.NameToLayer("Terrain");
            float gridSquareSize = FishableGrid.instance.GridSquareSize;
            float gridSquareHalfSize = FishableGrid.instance.GridSquareHalfSize;

            // Rays move and check clockwise around square
            Vector2 topLeftPosition = GridCenter + new Vector2(-gridSquareHalfSize, gridSquareHalfSize);
            RaycastHit2D topLeftHit = Physics2D.Raycast(topLeftPosition, Vector2.right, gridSquareSize, terrainLayer);

            Vector2 topRightPosition = GridCenter + new Vector2(gridSquareHalfSize, gridSquareHalfSize);
            RaycastHit2D topRightHit = Physics2D.Raycast(topRightPosition, Vector2.down, gridSquareSize, terrainLayer);

            Vector2 bottomRightPosition = GridCenter + new Vector2(gridSquareHalfSize, -gridSquareHalfSize);
            RaycastHit2D bottomRightHit = Physics2D.Raycast(bottomRightPosition, Vector2.left, gridSquareSize, terrainLayer);

            Vector2 bottomLeftPosition = GridCenter + new Vector2(-gridSquareHalfSize, -gridSquareHalfSize);
            RaycastHit2D bottomLeftHit = Physics2D.Raycast(bottomLeftPosition, Vector2.up, gridSquareSize, terrainLayer);

            if (topLeftHit.collider == collider) {
                return true;
            }
            if (topRightHit.collider == collider) {
                return true;
            }
            if (bottomRightHit.collider == collider) {
                return true;
            }
            if (bottomLeftHit.collider == collider) {
                return true;
            }

            return false;
        }
    }
}
