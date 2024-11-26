using Fishing.Fishables.Fish;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Fishing.Fishables
{
    public class FishableGrid : MonoBehaviour {
        [Header("Grid Shape")]
        [SerializeField, Min(1)] private int _columns = 16;
        [SerializeField, Min(1)] private int _rows = 16;
        [SerializeField, Min(0.1f)] private float _gridSquareSize = 1f;
        public float GridSquareSize { get => _gridSquareSize; private set { } }
        private float GridWidth => _columns * _gridSquareSize;
        private float GridHeight => _rows * _gridSquareSize;
        private float GridSquareHalfSize => _gridSquareSize * 0.5f;

        private GridSquare[][] _gridSquares;
        public GridSquare[][] GridSquares { get => _gridSquares; private set { } }

        [Header("Gizmos")]
        [SerializeField] private Color _gridColor;
        [SerializeField] private Color _fishableColor;
        [SerializeField] private bool _drawFishableLines;

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
                    _gridSquares[x][y] = _newGridSquare;
                }
            }
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

        public List<Fishable> GetNearbyFishables(int originSquareX, int originSquareY, int range) {
            Debug.Assert(originSquareX >= 0 && originSquareY >= 0, $"Attempting to get fishables form outside of grid ({originSquareX},{originSquareY})");

            List<Fishable> fishables = new List<Fishable>();
            for (int x = originSquareX - range; x <= originSquareX + range; x++) {
                if (x >= _gridSquares.Length || x < 0) {
                    continue;
                }

                for (int y = originSquareY - range; y <= originSquareY + range; y++) {
                    if (y >= _gridSquares[x].Length || y < 0) {
                        continue;
                    }

                    for (int i = 0; i < _gridSquares[x][y].GridFishables.Count; i++) {
                        fishables.Add(_gridSquares[x][y].GridFishables[i]);
                    }
                }
            }
            return fishables;
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

        /*public IList<T> GetComponentsWithinNearbyFishables<T>(int originSquareX, int originSquareY, int range) {
            Debug.Assert(originSquareX >= 0 && originSquareY >= 0, $"Attempting to get fishables form outside of grid ({originSquareX},{originSquareY})");

            List<T> _foundComponents = new List<T>();
            for (int x = originSquareX - range; x <= originSquareX + range; x++) {
                if (x >= _gridSquares.Length || x < 0) {
                    continue;
                }

                for (int y = originSquareY - range; y <= originSquareY + range; y++) {
                    if (y >= _gridSquares[x].Length || y < 0) {
                        continue;
                    }

                    for (int i = 0; i < _gridSquares[x][y].GridFishables.Count; i++) {
                        if (_gridSquares[x][y].GridFishables[i].TryGetComponent<T>(out T component)) {
                            _foundComponents.Add(_gridSquares[x][y].GridFishables[i].GetComponent<T>());
                        }
                    }
                }
            }

            return _foundComponents;
        }*/

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
            Gizmos.color = _gridColor;
            DrawGridVerticalLines();
            DrawGridHorizontalLines();

            if (_drawFishableLines) {
                DrawFishableLines();
            }
        }

        private void DrawGridVerticalLines() {
            for (int x = 0; x < _columns + 1; x++) {
                float lineX = _gridSquareSize * x;
                Vector2 fromPos = new Vector2(lineX, 0f) + (Vector2)transform.position;
                Vector2 toPos = new Vector2(lineX, -GridHeight) + (Vector2)transform.position;
                Gizmos.DrawLine(fromPos, toPos);
            }
        }
        private void DrawGridHorizontalLines() {
            for (int y = 0; y < _rows + 1; y++) {
                float _lineY = _gridSquareSize * -y;
                Vector2 fromPos = new Vector2(0f, _lineY) + (Vector2)transform.position;
                Vector2 toPos = new Vector2(GridWidth, _lineY) + (Vector2)transform.position;
                Gizmos.DrawLine(fromPos, toPos);
            }
        }

        private void DrawFishableLines() {
            Gizmos.color = _fishableColor;
            for (int x = 0; x < _columns; x++) {
                for (int y = 0; y < _rows; y++) {
                    GridSquare gridSquare = _gridSquares[x][y];
                    for (int i = 0; i < gridSquare.GridFishables.Count; i++) {
                        Vector2 fishablePosition = gridSquare.GridFishables[i].transform.position;
                        Vector2 gridSquarePosition = new Vector2(_gridSquareSize * x + GridSquareHalfSize, -GridHeight + (_gridSquareSize * y) + GridSquareHalfSize);
                        gridSquarePosition += (Vector2)transform.position;

                        Gizmos.DrawLine(fishablePosition, gridSquarePosition);
                        Gizmos.DrawSphere(gridSquarePosition, _gridSquareSize * 0.025f);
                    }
                }
            }
        }
    }
}
