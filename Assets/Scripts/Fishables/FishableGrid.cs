using Fishing.Fishables.Fish;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing.Fishables {
    public class FishableGrid : MonoBehaviour {
        [Header("Grid Shape")]
        [SerializeField, Min(1)] private int _columns = 16;
        [SerializeField, Min(1)] private int _rows = 16;
        [SerializeField, Min(0.1f)] private float _gridSquareSize = 1f;
        public float GridSquareSize { get => _gridSquareSize; private set { } }
        private float GridWidth => _columns * _gridSquareSize;
        public float GridHeight => _rows * _gridSquareSize;
        public float GridSquareHalfSize => _gridSquareSize * 0.5f;

        private GridSquare[][] _gridSquares;
        public GridSquare[][] GridSquares { get => _gridSquares; private set { } }

        [Header("Gizmos")]
        [SerializeField] private bool _drawGridLines;
        [SerializeField] private Color _gridColor;

        [SerializeField] private bool _drawGridCenters;
        [SerializeField] private Color _gridCenterColor;
        [SerializeField] private Color _gridHasTerrainColor;

        [SerializeField] private bool _drawFishableLines;
        [SerializeField] private Color _fishableColor;

        public static FishableGrid instance;

        private void Awake() {
            if (instance != null) {
                Destroy(gameObject);
                return;
            }
            instance = this;
        }

        private void Start() {
            GenerateGridSquares();
        }

        private void FixedUpdate() {
            for (int x = 0; x < _columns; x++) {
                for (int y = 0; y < _rows; y++) {
                    AssessGridSquareOutOfBoundsFish(_gridSquares[x][y]);
                }
            }
        }

        private void GenerateGridSquares() {
            _gridSquares = new GridSquare[_columns][];
            for (int x = 0; x < _columns; x++) {
                _gridSquares[x] = new GridSquare[_rows];
                for (int y = 0; y < _rows; y++) {
                    GridSquare _newGridSquare = new GridSquare(x, y);
                    DetermineIfCollidingWithTerrain(_newGridSquare);
                    _gridSquares[x][y] = _newGridSquare;
                }
            }
        }

        private void DetermineIfCollidingWithTerrain(GridSquare gridSquare) {
            PolygonCollider2D[] terrainColliders = GameObject.Find("Grid").GetComponentsInChildren<PolygonCollider2D>();
            for (int i = 0; i < terrainColliders.Length; i++) {
                if (IsGridSquareOutsideOfChunk(terrainColliders[i], gridSquare)) {
                    continue;
                }
                if (!AreGridEdgesCollidingWithCollider(terrainColliders[i], gridSquare)) {
                    continue;
                }

                // TODO: Add function for detecting if a section of terrain is completely encapsulated by a grid square.
                //       This should take each point of the terrain collider and raycast towards the grid center by it's extents length.
                //       If the raycast exits the grid square an odd amount of times, then the point it came from is within the grid square.

                gridSquare.IsCollidingWithTerrain = true;
                return;
            }
            gridSquare.IsCollidingWithTerrain = false;
        }

        private bool IsGridSquareOutsideOfChunk(PolygonCollider2D collider, GridSquare gridSquare) {
            Bounds terrainBounds = collider.bounds;
            float distanceToCenter = Vector2.Distance(gridSquare.GridCenter, terrainBounds.center);
            float inBoundsRange = terrainBounds.extents.magnitude + GridSquareSize;
            bool isChunkTooFarAway = distanceToCenter >= inBoundsRange;
            return isChunkTooFarAway;
        }

        private bool AreGridEdgesCollidingWithCollider(PolygonCollider2D collider, GridSquare gridSquare) {

            int terrainLayer = ~LayerMask.NameToLayer("Terrain");

            // Rays move and check clockwise around square
            Vector2 topLeftPosition = gridSquare.GridCenter + new Vector2(-GridSquareHalfSize, GridSquareHalfSize);
            RaycastHit2D topLeftHit = Physics2D.Raycast(topLeftPosition, Vector2.right, GridSquareSize, terrainLayer);

            Vector2 topRightPosition = gridSquare.GridCenter + new Vector2(GridSquareHalfSize, GridSquareHalfSize);
            RaycastHit2D topRightHit = Physics2D.Raycast(topRightPosition, Vector2.down, GridSquareSize, terrainLayer);

            Vector2 bottomRightPosition = gridSquare.GridCenter + new Vector2(GridSquareHalfSize, -GridSquareHalfSize);
            RaycastHit2D bottomRightHit = Physics2D.Raycast(bottomRightPosition, Vector2.left, GridSquareSize, terrainLayer);

            Vector2 bottomLeftPosition = gridSquare.GridCenter + new Vector2(-GridSquareHalfSize, -GridSquareHalfSize);
            RaycastHit2D bottomLeftHit = Physics2D.Raycast(bottomLeftPosition, Vector2.up, GridSquareSize, terrainLayer);

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

        public void SortFishableIntoGridSquare(Fishable fishable) {
            int[] gridCoord = Vector2ToGrid(fishable.transform.position);
            GridSquare gridSquare = _gridSquares[gridCoord[0]][gridCoord[1]];
            gridSquare.GridFishables.Add(fishable);
            if (fishable.TryGetComponent(out Edible edible)) {
                gridSquare.GridEdibles.Add(edible);
            }
            fishable.GridSquare = gridCoord;
        }
        // TODO: Return after removing first instance, as there should only ever be one instance in the whole grid
        public void RemoveFromGridSquares(Fishable fishable) {
            for (int x = 0; x < _columns; x++) {
                for (int y = 0; y < _rows; y++) {
                    if (!_gridSquares[x][y].GridFishables.Contains(fishable)) {
                        continue;
                    }
                    _gridSquares[x][y].GridFishables.Remove(fishable);

                    if (fishable.TryGetComponent(out Edible edible)) {
                        if (!_gridSquares[x][y].GridEdibles.Contains(edible)) {
                            continue;
                        }
                        _gridSquares[x][y].GridEdibles.Remove(edible);
                    }
                }
            }
        }

        public int[] Vector2ToGrid(Vector2 position) {
            Debug.Assert(position != null, "Cannot convert null position to grid");

            Vector2 cleanPosition = position - (Vector2)transform.position;
            cleanPosition.y += GridHeight;
            cleanPosition.x = Mathf.Clamp(cleanPosition.x, 0, GridWidth);
            cleanPosition.y = Mathf.Clamp(cleanPosition.y, 0, GridHeight);

            int gridX = Mathf.Min(Mathf.FloorToInt(cleanPosition.x / _gridSquareSize), _columns - 1);
            int gridY = Mathf.Min(Mathf.FloorToInt(cleanPosition.y / _gridSquareSize), _rows - 1);

            int[] grid = new int[2] { gridX, gridY };

            return grid;
        }

        public List<Edible> GetNearbyEdibles(int originSquareX, int originSquareY, int range) {
            Debug.Assert(originSquareX >= 0 && originSquareY >= 0, $"Attempting to get fishables form outside of grid ({originSquareX},{originSquareY})");

            List<Edible> edible = new List<Edible>();
            for (int x = originSquareX - range; x <= originSquareX + range; x++) {
                if (x >= _gridSquares.Length || x < 0) {
                    continue;
                }

                for (int y = originSquareY - range; y <= originSquareY + range; y++) {
                    if (y >= _gridSquares[x].Length || y < 0) {
                        continue;
                    }

                    for (int i = 0; i < _gridSquares[x][y].GridFishables.Count; i++) {
                        edible.Add(_gridSquares[x][y].GridEdibles[i]);
                    }
                }
            }
            return edible;
        }

        public bool IsNearbyTerrainGrid(int originSquareX, int originSquareY, int range) {
            Debug.Assert(originSquareX >= 0 && originSquareY >= 0, $"Attempting to get fishables form outside of grid ({originSquareX},{originSquareY})");

            for (int x = originSquareX - range; x <= originSquareX + range; x++) {
                if (x >= _gridSquares.Length || x < 0) {
                    continue;
                }

                for (int y = originSquareY - range; y <= originSquareY + range; y++) {
                    if (y >= _gridSquares[x].Length || y < 0) {
                        continue;
                    }

                    if (_gridSquares[x][y].IsCollidingWithTerrain) {
                        return true;
                    }
                }
            }
            return false;
        }

        private void AssessGridSquareOutOfBoundsFish(GridSquare gridSquare) {
            if (gridSquare.GridFishables.Count == 0) {
                return;
            }

            float xMin = transform.position.x + (GridSquareSize * gridSquare.GridX);
            float xMax = transform.position.x + (GridSquareSize * gridSquare.GridX) + GridSquareSize;
            float yMin = (transform.position.y - GridHeight) + (GridSquareSize * gridSquare.GridY);
            float yMax = (transform.position.y - GridHeight) + (GridSquareSize * gridSquare.GridY) + GridSquareSize;

            List<Fishable> fishablesList = new List<Fishable>(gridSquare.GridFishables);
            foreach (Fishable fishable in fishablesList) {
                Vector2 fishablePos = fishable.transform.position;
                if (fishablePos.x >= xMin && fishablePos.x <= xMax && fishablePos.y >= yMin && fishablePos.y <= yMax) {
                    continue;
                }

                gridSquare.GridFishables.Remove(fishable);
                if (fishable.TryGetComponent(out Edible edible)) {
                    gridSquare.GridEdibles.Remove(edible);
                }
                SortFishableIntoGridSquare(fishable);
            }
        }

        private void OnDrawGizmosSelected() {
            if (_drawGridLines) {
                DrawGridVerticalLines();
                DrawGridHorizontalLines();
            }
            if (_drawGridCenters) {
                DrawGridCenters();
            }

            if (_drawFishableLines) {
                DrawFishableLines();
            }
        }

        private void DrawGridVerticalLines() {
            Gizmos.color = _gridColor;
            for (int x = 0; x < _columns + 1; x++) {
                float lineX = _gridSquareSize * x;
                Vector2 fromPos = new Vector2(lineX, 0f) + (Vector2)transform.position;
                Vector2 toPos = new Vector2(lineX, -GridHeight) + (Vector2)transform.position;
                Gizmos.DrawLine(fromPos, toPos);
            }
        }
        private void DrawGridHorizontalLines() {
            Gizmos.color = _gridColor;
            for (int y = 0; y < _rows + 1; y++) {
                float _lineY = _gridSquareSize * -y;
                Vector2 fromPos = new Vector2(0f, _lineY) + (Vector2)transform.position;
                Vector2 toPos = new Vector2(GridWidth, _lineY) + (Vector2)transform.position;
                Gizmos.DrawLine(fromPos, toPos);
            }
        }

        private void DrawGridCenters() {
            for (int x = 0; x < _columns; x++) {
                for (int y = 0; y < _rows; y++) {
                    if (_gridSquares[x][y].IsCollidingWithTerrain) {
                        Gizmos.color = _gridHasTerrainColor;
                    }
                    else {
                        Gizmos.color = _gridCenterColor;
                    }
                    Gizmos.DrawSphere(_gridSquares[x][y].GridCenter, _gridSquareSize * 0.1f);
                }
            }
        }

        private void DrawFishableLines() {
            Gizmos.color = _fishableColor;
            for (int x = 0; x < _columns; x++) {
                for (int y = 0; y < _rows; y++) {
                    GridSquare gridSquare = _gridSquares[x][y];
                    for (int i = 0; i < gridSquare.GridFishables.Count; i++) {
                        Vector2 fishablePosition = gridSquare.GridFishables[i].transform.position;
                        Gizmos.DrawLine(fishablePosition, _gridSquares[x][y].GridCenter);
                    }
                }
            }
        }
    }
}
