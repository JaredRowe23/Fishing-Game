using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing.Util.Collision {
    public struct EdgePlaneInfo {
        public Plane plane;
        public Vector2 vertexA;
        public Vector2 vertexB;
        public float edgeLength;

        public EdgePlaneInfo(Collider2D collider, Vector2 originPoint, Vector2 neighborVertex) {
            vertexA = originPoint;

            PhysicsShapeGroup2D shapeGroup = new PhysicsShapeGroup2D();
            collider.GetShapes(shapeGroup);

            vertexB = neighborVertex;

            edgeLength = Vector2.Distance(vertexA, vertexB);

            plane = new Plane((Vector3)originPoint, (Vector3)vertexB, (Vector3)vertexB + Vector3.forward); // 3rd vector is positioned this way because plane normal is based on viewing points clockwise
        }
    }
}
