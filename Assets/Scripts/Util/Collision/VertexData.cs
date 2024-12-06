using System.Collections.Generic;
using UnityEngine;

namespace Fishing.Util.Collision {
    public class VertexData {
        public Vector2 position;
        public float distanceToPoint;
        public List<PhysicsShape2D> containingShapes;
        public List<Vector2> neighborPositions;
        private List<int[]> validNormalsIndices;
        public List<int[]> ValidNormalsIndices {
            get => validNormalsIndices;
            set {
                validNormalsIndices = value;
            }
        }

        public VertexData(Vector2 point, Vector2 _position, int _vertexIndex, PhysicsShape2D _initialShape, int _initialShapeIndex, PhysicsShapeGroup2D _shapeGroup) {
            position = _position;
            distanceToPoint = Vector2.Distance(point, position);
            containingShapes = new List<PhysicsShape2D>() { _initialShape };
            neighborPositions = new List<Vector2>();
            validNormalsIndices = new List<int[]>();

            AddNeighbors(_vertexIndex, _initialShapeIndex, _initialShape, _shapeGroup);
            CalculateValidNormals();
        }

        public void AddDataFromContainingShape(int vertexIndex, int shapeIndex, PhysicsShape2D shape, PhysicsShapeGroup2D shapeGroup) {
            containingShapes.Add(shape);

            AddNeighbors(vertexIndex, shapeIndex, shape, shapeGroup);
            CalculateValidNormals();
        }

        private void AddNeighbors(int vertexIndex, int shapeIndex, PhysicsShape2D shape, PhysicsShapeGroup2D shapeGroup) {
            if (vertexIndex == shape.vertexCount - 1) {
                Vector2 neighborPosition = shapeGroup.GetShapeVertex(shapeIndex, 0);
                if (!neighborPositions.Contains(neighborPosition)) {
                    neighborPositions.Add(neighborPosition);
                }
            }
            else {
                Vector2 neighborPosition = shapeGroup.GetShapeVertex(shapeIndex, vertexIndex + 1);
                if (!neighborPositions.Contains(neighborPosition)) {
                    neighborPositions.Add(neighborPosition);
                }
            }

            if (vertexIndex == 0) {
                Vector2 neighborPosition = shapeGroup.GetShapeVertex(shapeIndex, shape.vertexCount - 1);
                if (!neighborPositions.Contains(neighborPosition)) {
                    neighborPositions.Add(neighborPosition);
                }
            }
            else {
                Vector2 neighborPosition = shapeGroup.GetShapeVertex(shapeIndex, vertexIndex - 1);
                if (!neighborPositions.Contains(neighborPosition)) {
                    neighborPositions.Add(neighborPosition);
                }
            }
        }

        private void CalculateValidNormals() {
            ValidNormalsIndices = new List<int[]>();
            for (int i = 0; i < neighborPositions.Count - 1; i++) {
                for (int j = i + 1; j < neighborPositions.Count; j++) {
                    Vector2 neighborADirection = (neighborPositions[i] - position).normalized;
                    Vector2 neighborBDirection = (neighborPositions[j] - position).normalized;
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
                        Debug.DrawRay(position, normal, Color.green);
                    }
                    else {
                        Debug.DrawRay(position, normal, Color.red);
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
                        Debug.DrawRay(position, normal * -1, Color.green);
                    }
                    else {
                        Debug.DrawRay(position, normal * -1, Color.red);
                    }
                }
            }
        }

        private bool IsNormalValid(Vector2 normal) {
            int layerMask = ~LayerMask.NameToLayer("Terrain");
            if (!Physics2D.OverlapPoint(position + normal * 0.01f, layerMask)) {
                return true;
            }
            return false;
        }
    }
}