using System.Collections.Generic;
using UnityEngine;

namespace Fishing.Util.Collision {
    public static class CollisionDetection {

        public static Vector2 ClosestPointFromInsideCollider2D(Vector2 point, Collider2D collider) {
            List<VertexData> vertexData = GetVertexDatasFromCollider(collider, point);

            vertexData = GetValidVertices(vertexData);

            if (vertexData.Count == 0) {
                return point;
            }

            VertexData closestVertex = GetClosestVertex(vertexData);

            List<EdgePlaneInfo> planeInfos = GetPlaneInfosFromVertexValidNormals(closestVertex, collider);

            List<Vector2> closestPoints = GetClosestValidPointsFromPlaneInfos(planeInfos, point);

            closestPoints.Add(closestVertex.position);

            Vector2 closestPoint = GetClosestPointFromList(closestPoints, point);

            return closestPoint;
        }

        private static List<VertexData> GetVertexDatasFromCollider(Collider2D collider, Vector2 point) {
            List<VertexData> vertexData = new List<VertexData>();

            PhysicsShapeGroup2D shapeGroup = new PhysicsShapeGroup2D();
            collider.GetShapes(shapeGroup);

            for (int shapeIndex = 0; shapeIndex < shapeGroup.shapeCount; shapeIndex++) {
                PhysicsShape2D shape = shapeGroup.GetShape(shapeIndex);
                for (int vertexIndex = 0; vertexIndex < shape.vertexCount; vertexIndex++) {
                    Vector2 vertexPosition = shapeGroup.GetShapeVertex(shapeIndex, vertexIndex);
                    int vertexDataListIndex = TryFindVertexIndexInVertexDataList(vertexData, vertexPosition);

                    if (vertexDataListIndex == -1) {
                        VertexData vertex = new VertexData(point, vertexPosition, vertexIndex, shape, shapeIndex, shapeGroup);
                        vertexData.Add(vertex);
                        continue;
                    }

                    vertexData[vertexDataListIndex].AddDataFromContainingShape(vertexIndex, shapeIndex, shape, shapeGroup);
                }
            }

            return vertexData;
        }

        private static int TryFindVertexIndexInVertexDataList(List<VertexData> vertexData, Vector2 vectorPosition) {
            for (int i = 0; i < vertexData.Count; i++) {
                if (vertexData[i].position != vectorPosition) {
                    continue;
                }
                return i;
            }
            return -1;
        }

        private static List<VertexData> GetValidVertices(List<VertexData> vertexData) {
            List<VertexData> validVertices = new List<VertexData>();
            List<int> assessedIndices = new List<int>();
            int layerMask = ~LayerMask.NameToLayer("Terrain");
            for (int vertexDataIndex = 0; vertexDataIndex < vertexData.Count; vertexDataIndex++) {
                if (vertexData[vertexDataIndex].ValidNormalsIndices.Count == 0) {
                    continue;
                }

                validVertices.Add(vertexData[vertexDataIndex]);
            }
            return validVertices;
        }

        private static VertexData GetClosestVertex(List<VertexData> vertexData) {
            VertexData closestVertex = null;
            for (int i = 0; i < vertexData.Count; i++) {
                if (closestVertex == null) {
                    closestVertex = vertexData[i];
                    continue;
                }

                if (closestVertex.distanceToPoint < vertexData[i].distanceToPoint) {
                    continue;
                }

                closestVertex = vertexData[i];
            }

            return closestVertex;
        }

        private static List<EdgePlaneInfo> GetPlaneInfosFromVertexValidNormals(VertexData vertex, Collider2D collider) {
            List<EdgePlaneInfo> planeInfos = new List<EdgePlaneInfo>();
            for (int i = 0; i < vertex.ValidNormalsIndices.Count; i++) {
                bool planeAAlreadyIn = false;
                bool planeBAlreadyIn = false;

                for (int j = 0; j < planeInfos.Count; j++) {
                    if (planeInfos[j].vertexB == vertex.neighborPositions[vertex.ValidNormalsIndices[i][0]]) {
                        planeAAlreadyIn = true;
                    }
                    if (planeInfos[j].vertexB == vertex.neighborPositions[vertex.ValidNormalsIndices[i][1]]) {
                        planeBAlreadyIn = true;
                    }
                }

                if (planeAAlreadyIn == false) {
                    planeInfos.Add(new EdgePlaneInfo(collider, vertex.position, vertex.neighborPositions[vertex.ValidNormalsIndices[i][0]]));
                }
                if (planeBAlreadyIn == false) {
                    planeInfos.Add(new EdgePlaneInfo(collider, vertex.position, vertex.neighborPositions[vertex.ValidNormalsIndices[i][1]]));
                }
            }
            return planeInfos;
        }

        private static List<Vector2> GetClosestValidPointsFromPlaneInfos(List<EdgePlaneInfo> planeInfos, Vector2 point) {
            List<Vector2> closestPlanePoints = new List<Vector2>();
            int layerMask = ~LayerMask.NameToLayer("Terrain");
            for (int i = 0; i < planeInfos.Count; i++) {
                Vector2 closestPlanePoint = planeInfos[i].plane.ClosestPointOnPlane(point);

                float closestPlanePointDistanceA = Vector2.Distance(closestPlanePoint, planeInfos[i].vertexA);
                float closestPlanePointDistanceB = Vector2.Distance(closestPlanePoint, planeInfos[i].vertexB);

                if (closestPlanePointDistanceA <= planeInfos[i].edgeLength && closestPlanePointDistanceB <= planeInfos[i].edgeLength) {
                    Vector2 edgeNormal = (closestPlanePoint - point).normalized;

                    if (!Physics2D.OverlapPoint(closestPlanePoint + edgeNormal * 0.01f, layerMask)) {
                        closestPlanePoints.Add(closestPlanePoint);
                        continue;
                    }

                    else if (!Physics2D.OverlapPoint(closestPlanePoint + edgeNormal * -0.01f, layerMask)) {
                        closestPlanePoints.Add(closestPlanePoint);
                        continue;
                    }
                }
            }

            return closestPlanePoints;
        }

        private static Vector2 GetClosestPointFromList(List<Vector2> comparisonPoints, Vector2 sourcePoint) {
            Vector2 closestPoint = Vector2.positiveInfinity;
            float closestPointDistance = float.PositiveInfinity;

            for (int i = 0; i < comparisonPoints.Count; i++) {
                float distance = Vector2.Distance(comparisonPoints[i], sourcePoint);

                if (distance > closestPointDistance) {
                    continue;
                }

                closestPoint = comparisonPoints[i];
                closestPointDistance = distance;
            }

            return closestPoint;
        }

        /// <summary>
        /// Returns struct with the data on the closest collider, point, and distance from a given position and array of colliders.
        /// </summary>
        /// <param name="position"></param>
        /// <param name="colliders"></param>
        /// <returns></returns>
        public static ClosestPointInfo ClosestPointFromOutside(Vector2 position, PolygonCollider2D[] colliders) {
            if (colliders.Length <= 0) {
                Debug.LogWarning("ClosestPointFromColliders called with no provided colliders! Returning empty ClosestPointInfo");
                return new ClosestPointInfo(Vector2.zero, 0, null);
            }

            Vector2 initialPoint = colliders[0].ClosestPoint(position); // TODO: Convert from using Collider.ClosestPoint to Physics2D.Distance/Collider2D.Distance. Provides info for results inside of colliders.
            float initialPointDistance = Vector2.Distance(position, initialPoint);
            ClosestPointInfo _closestPoint = new ClosestPointInfo(initialPoint, initialPointDistance, colliders[0]);

            for (int i = 1; i < colliders.Length; i++) {
                Vector2 pointToEvaluate = colliders[i].ClosestPoint(position);
                float distanceToEvaluate = Vector2.Distance(position, pointToEvaluate);

                if (distanceToEvaluate >= _closestPoint.distance) continue;

                _closestPoint = new ClosestPointInfo(pointToEvaluate, distanceToEvaluate, colliders[i]);
            }

            return _closestPoint;
        }
    }
}