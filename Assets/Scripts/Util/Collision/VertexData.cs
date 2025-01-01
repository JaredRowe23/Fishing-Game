using System.Collections.Generic;
using UnityEngine;

namespace Fishing.Util.Collision {
    public class VertexData {
        private Vector2 _position;
        public Vector2 Position { get => _position; private set => _position = value; }
        private float _distanceToPoint;
        public float DistanceToPoint { get => _distanceToPoint; private set => _distanceToPoint = value; }
        private List<PhysicsShape2D> _containingShapes;
        public List<PhysicsShape2D> ContainingShapes { get => _containingShapes; private set => _containingShapes = value; }
        private List<Vector2> _neighborPositions;
        public List<Vector2> NeighborPositions { get => _neighborPositions; private set => _neighborPositions = value; }
        private List<int[]> _validNormalsIndices;
        public List<int[]> ValidNormalsIndices { get => _validNormalsIndices; private set { _validNormalsIndices = value; } }

        public VertexData(Vector2 point, Vector2 _position, int _vertexIndex, PhysicsShape2D _initialShape, int _initialShapeIndex, PhysicsShapeGroup2D _shapeGroup) {
            Position = _position;
            DistanceToPoint = Vector2.Distance(point, Position);
            ContainingShapes = new List<PhysicsShape2D>() { _initialShape };
            NeighborPositions = new List<Vector2>();
            ValidNormalsIndices = new List<int[]>();

            AddNeighbors(_vertexIndex, _initialShapeIndex, _initialShape, _shapeGroup);
            CalculateValidNormals();
        }

        public void AddDataFromContainingShape(int vertexIndex, int shapeIndex, PhysicsShape2D shape, PhysicsShapeGroup2D shapeGroup) {
            ContainingShapes.Add(shape);

            AddNeighbors(vertexIndex, shapeIndex, shape, shapeGroup);
            CalculateValidNormals();
        }

        private void AddNeighbors(int vertexIndex, int shapeIndex, PhysicsShape2D shape, PhysicsShapeGroup2D shapeGroup) {
            if (vertexIndex == shape.vertexCount - 1) {
                Vector2 neighborPosition = shapeGroup.GetShapeVertex(shapeIndex, 0);
                if (!NeighborPositions.Contains(neighborPosition)) {
                    NeighborPositions.Add(neighborPosition);
                }
            }
            else {
                Vector2 neighborPosition = shapeGroup.GetShapeVertex(shapeIndex, vertexIndex + 1);
                if (!NeighborPositions.Contains(neighborPosition)) {
                    NeighborPositions.Add(neighborPosition);
                }
            }

            if (vertexIndex == 0) {
                Vector2 neighborPosition = shapeGroup.GetShapeVertex(shapeIndex, shape.vertexCount - 1);
                if (!NeighborPositions.Contains(neighborPosition)) {
                    NeighborPositions.Add(neighborPosition);
                }
            }
            else {
                Vector2 neighborPosition = shapeGroup.GetShapeVertex(shapeIndex, vertexIndex - 1);
                if (!NeighborPositions.Contains(neighborPosition)) {
                    NeighborPositions.Add(neighborPosition);
                }
            }
        }

        private void CalculateValidNormals() {
            ValidNormalsIndices = new List<int[]>();
            for (int i = 0; i < NeighborPositions.Count - 1; i++) {
                for (int j = i + 1; j < NeighborPositions.Count; j++) {
                    Vector2 neighborADirection = (NeighborPositions[i] - Position).normalized;
                    Vector2 neighborBDirection = (NeighborPositions[j] - Position).normalized;
                    Vector2 normal = (neighborADirection + neighborBDirection).normalized;
                    if (normal == Vector2.zero) {
                        normal = Vector2.up;
                    }

                    if (IsNormalValid(normal)) {
                        int[] newValidNormalIndices = new int[2] { i, j };
                        bool isAlreadyAdded = false;
                        for (int k = 0; k < ValidNormalsIndices.Count; k++) {
                            if (ValidNormalsIndices[k][0] == newValidNormalIndices[0] && ValidNormalsIndices[k][1] == newValidNormalIndices[1]) {
                                isAlreadyAdded = true;
                            }
                        }
                        if (!isAlreadyAdded) {
                            ValidNormalsIndices.Add(newValidNormalIndices);
                        }
                        Debug.DrawRay(Position, normal, Color.green);
                    }
                    else {
                        Debug.DrawRay(Position, normal, Color.red);
                    }

                    if (IsNormalValid(normal * -1)) {
                        int[] newValidNormalIndices = new int[2] { i, j };
                        bool isAlreadyAdded = false;
                        for (int k = 0; k < ValidNormalsIndices.Count; k++) {
                            if (ValidNormalsIndices[k][0] == newValidNormalIndices[0] && ValidNormalsIndices[k][1] == newValidNormalIndices[1]) {
                                isAlreadyAdded = true;
                            }
                        }
                        if (!isAlreadyAdded) {
                            ValidNormalsIndices.Add(newValidNormalIndices);
                        }
                        Debug.DrawRay(Position, normal * -1, Color.green);
                    }
                    else {
                        Debug.DrawRay(Position, normal * -1, Color.red);
                    }
                }
            }
        }

        private bool IsNormalValid(Vector2 normal) {
            int layerMask = ~LayerMask.NameToLayer("Terrain");
            if (!Physics2D.OverlapPoint(Position + normal * 0.01f, layerMask)) {
                return true;
            }
            return false;
        }
    }
}