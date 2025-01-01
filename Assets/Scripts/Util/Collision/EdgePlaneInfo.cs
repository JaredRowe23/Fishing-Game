using UnityEngine;

namespace Fishing.Util.Collision {
    public struct EdgePlaneInfo {
        private Plane _plane;
        public Plane Plane { get => _plane; private set => _plane = value; }
        private Vector2 _vertexA;
        public Vector2 VertexA { get => _vertexA; private set => _vertexA = value; }
        private Vector2 _vertexB;
        public Vector2 VertexB { get => _vertexB; private set => _vertexB = value; }
        private float _edgeLength;
        public float EdgeLength { get => _edgeLength; private set => _edgeLength = value; }


        public EdgePlaneInfo(Collider2D collider, Vector2 originPoint, Vector2 neighborVertex) {
            _vertexA = originPoint;

            PhysicsShapeGroup2D shapeGroup = new PhysicsShapeGroup2D();
            collider.GetShapes(shapeGroup);

            _vertexB = neighborVertex;

            _edgeLength = Vector2.Distance(_vertexA, _vertexB);

            _plane = new Plane((Vector3)originPoint, (Vector3)_vertexB, (Vector3)_vertexB + Vector3.forward); // 3rd vector is positioned this way because plane normal is based on viewing points clockwise
        }
    }
}
