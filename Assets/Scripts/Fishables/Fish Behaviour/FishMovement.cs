using Fishing.Util;
using UnityEngine;

namespace Fishing.Fishables.Fish {
    [RequireComponent(typeof(Fishable), typeof(FoodSearch))]
    public class FishMovement : MonoBehaviour {
        [Header("Movement Speeds")]
        #region
        [SerializeField] private float _swimSpeed;
        [SerializeField] private float _rotationSpeed;
        #endregion

        [Header("Move Direction")]
        #region
        [SerializeField] private float _obstacleAvoidanceDistance;
        [SerializeField] private float _obstacleAvoidanceWeight = 1f;
        public float ObstacleAvoidanceWeight { get => _obstacleAvoidanceWeight; set => _obstacleAvoidanceWeight = value; }
        #endregion

        [Header("Home Distance")]
        #region
        [SerializeField] private float _baseMaxHomeDistance;
        [SerializeField] private float _maxHomeDistanceVariation;
        private float _maxHomeDistance;
        public float MaxHomeDistance { get => _maxHomeDistance; private set { _maxHomeDistance = value; } }


        private SpriteRenderer[] _flippableSprites;

        private Vector2 _targetPos;
        public Vector2 TargetPos { get => _targetPos; set => _targetPos = value; }

        private float _targetPosDir = 0;
        public float TargetPosDir { get => _targetPosDir; set => _targetPosDir = Mathf.Clamp(value, -1, 1); }

        private float _rotationDir = 0;
        public float RotationDir { get => _rotationDir; set => _rotationDir = Mathf.Clamp(value, -1, 1); }

        private FoodSearch _activePredator;
        public FoodSearch ActivePredator { get => _activePredator; set => _activePredator = value; }
        private FoodSearch _foodSearch;
        private SpawnZone _spawn;
        private PolygonCollider2D[] _floorColliders;
        private Fishable _fishable;
        #endregion

        [Header("Gizmos")]
        #region
        [SerializeField] private bool _drawMaxHomeDistance = false;
        [SerializeField] private Color _maxHomeDistanceColor = Color.cyan;

        [SerializeField] private bool _drawBaseMaxHomeDistance = false;
        [SerializeField] private Color _baseMaxHomeDistanceColor = Color.cyan;

        [SerializeField] private bool _drawMaxHomeDistanceVariations = false;
        [SerializeField] private Color _minHomeDistanceVariationColor = Color.cyan;
        [SerializeField] private Color _maxHomeDistanceVariationColor = Color.cyan;

        [SerializeField] private bool _drawTargetPos = false;
        [SerializeField] private Color _targetPosColor = Color.magenta;

        [SerializeField] private bool _drawObstacleAvoidanceDistance = false;
        [SerializeField] private Color _obstacleAvoidanceDistanceColor = Color.red;
        #endregion

        private void Awake() {
            _foodSearch = GetComponent<FoodSearch>();
            _spawn = transform.parent.GetComponent<SpawnZone>();
            _floorColliders = GameObject.Find("Grid").GetComponentsInChildren<PolygonCollider2D>(); // TODO: Change how the PolygonCollider2D is found, as GameObject.Find isn't reliable or performant
            _flippableSprites = GetSpriteRenderers();
            _fishable = GetComponent<Fishable>();
        }

        private void Start() {
            MaxHomeDistance = _baseMaxHomeDistance + Random.Range(-_maxHomeDistanceVariation, _maxHomeDistanceVariation);
        }

        private void FixedUpdate() {
            if (_fishable.IsHooked) {
                return;
            }

            ValidatePredator();
            DecideMovementDirection();
            Move();
            FlipSprite();
        }

        private void ValidatePredator() {
            if (!ActivePredator) {
                return;
            }
            if (ActivePredator.DesiredFood == null) {
                ActivePredator = null;
            }
        }

        private void DecideMovementDirection() {
            Vector2 surfacingCheck = (Vector2)transform.position + (Vector2.up * _obstacleAvoidanceDistance);
            if (surfacingCheck.y >= 0) {
                AvoidSurface();
                return;
            }

            ClosestPointInfo closestPointInfo = Utilities.ClosestPointFromColliders(transform.position, _floorColliders);
            float distToFloor = Vector2.Distance(transform.position, closestPointInfo.position);

            if (distToFloor < _obstacleAvoidanceDistance) {
                AvoidFloor(closestPointInfo.position);
                return;
            }

            if (ActivePredator != null) {
                AvoidPredator();
                return;
            }

            if (Vector2.Distance(transform.position, _spawn.transform.position) >= MaxHomeDistance) {
                CalculateTurnDirection(_spawn.transform.position);
                return;
            }

            if (_foodSearch.DesiredFood != null) {
                CalculateTurnDirection(_foodSearch.DesiredFood.transform.position);
                return;
            }

            GetComponent<IMovement>().Movement();
            return;
        }

        private void AvoidSurface() {
            RotationDir = transform.rotation.eulerAngles.z > 0 ? ObstacleAvoidanceWeight : -ObstacleAvoidanceWeight;
        }

        private void AvoidFloor(Vector2 _closestFloorPosition) {
            RotationDir = Utilities.DirectionFromTransformToTarget(transform, (Vector2)transform.position + ((Vector2)transform.position) - _closestFloorPosition);
        }

        private void AvoidPredator() {
            RotationDir = Utilities.DirectionFromTransformToTarget(transform, (Vector2)transform.position + ((Vector2)transform.position) - (Vector2)ActivePredator.transform.position);
        }

        public void CalculateTurnDirection(Vector2 targetPos) {
            TargetPos = targetPos;
            TargetPosDir = Utilities.DirectionFromTransformToTarget(transform, TargetPos);
            RotationDir = Mathf.Clamp(TargetPosDir * ObstacleAvoidanceWeight, -1, 1);
        }

        private void Move() {
            transform.Rotate(0f, 0f, _rotationSpeed * RotationDir * Time.fixedDeltaTime);
            transform.position = Vector2.MoveTowards(transform.position, transform.up + transform.position, _swimSpeed * Time.fixedDeltaTime);
        }

        private void FlipSprite() {
            bool flipSprite = Utilities.UnsignedToSignedAngle(transform.rotation.eulerAngles.z) < 0;
            for (int i = 0; i < _flippableSprites.Length; i++) {
                _flippableSprites[i].flipY = flipSprite;
            }
        }

        private SpriteRenderer[] GetSpriteRenderers() {
            SpriteRenderer[] _sprites = transform.GetComponentsInChildren<SpriteRenderer>();
            int _minimapSpriteIndex = 0;  // TODO: idk wtf this minimapSpriteIndex is *actually* doing, but this can probably be simplified massively.
            for (int i = 0; i < _sprites.Length; i++) {
                if (_sprites[i].gameObject.layer == LayerMask.NameToLayer("Minimap")) {
                    _minimapSpriteIndex = i;
                }
            }

            for (int i = _minimapSpriteIndex + 1; i < _sprites.Length; i++) {
                _sprites[i - 1] = _sprites[i];
            }
            System.Array.Resize(ref _sprites, _sprites.Length - 1);
            return _sprites;
        }

        private void OnDrawGizmosSelected() {
            if (_fishable.IsHooked) {
                return;
            }

            if (_drawMaxHomeDistance) {
                DrawMaxHomeDistance();
            }
            if (_drawBaseMaxHomeDistance) {
                DrawBaseMaxHomeDistance();
            }
            if (_drawMaxHomeDistanceVariations) {
                DrawMaxHomeDistanceVariations();
            }
            if (_drawTargetPos) {
                DrawTargetPos();
            }
            if (_drawObstacleAvoidanceDistance) {
                DrawObstacleAvoidanceDistance();
            }
        }

        private void DrawMaxHomeDistance() {
            Gizmos.color = _maxHomeDistanceColor;
            Gizmos.DrawWireSphere(_spawn.transform.position, MaxHomeDistance);
        }

        private void DrawBaseMaxHomeDistance() {
            Gizmos.color = _baseMaxHomeDistanceColor;
            Gizmos.DrawWireSphere(_spawn.transform.position, _baseMaxHomeDistance);
        }

        private void DrawMaxHomeDistanceVariations() {
            Gizmos.color = _minHomeDistanceVariationColor;
            Gizmos.DrawWireSphere(_spawn.transform.position, _baseMaxHomeDistance - _maxHomeDistanceVariation);
            Gizmos.color = _maxHomeDistanceVariationColor;
            Gizmos.DrawWireSphere(_spawn.transform.position, _baseMaxHomeDistance + _maxHomeDistanceVariation);
        }

        private void DrawTargetPos() {
            Gizmos.color = _targetPosColor;
            Gizmos.DrawSphere(TargetPos, 0.5f);
        }

        private void DrawObstacleAvoidanceDistance() {
            Gizmos.color = _obstacleAvoidanceDistanceColor;
            Gizmos.DrawWireSphere(transform.position, _obstacleAvoidanceDistance);
        }
    }
}
