using Fishing.Util;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing.Fishables.Fish {
    [RequireComponent(typeof(Fishable))]
    public class SectionedFish : MonoBehaviour {
        [SerializeField] private GameObject _sectionPrefab;
        [SerializeField] private int _numberOfSections = 20;
        [SerializeField] private float _sectionScale = 0.75f;
        [SerializeField] private float _sectionSpacing = 3f;
        [SerializeField] private float _sectionRotationDampening = 0.1f;

        private List<GameObject> _sections;
        private List<Vector3> _previousFrameSectionPositions;
        private List<Quaternion> _previousFrameSectionRotations;

        [Header("Gizmos")]
        #region
        [SerializeField] private bool _drawSectionGizmos = false;
        [SerializeField] private Color _sectionGizmoColor = Color.yellow;
        #endregion

        private Fishable _fishable;

        private void Awake() {
            _fishable = GetComponent<Fishable>();
        }

        void Start() {
            GenerateSections();
            UpdatePreviousFrameTransforms();
        }

        void FixedUpdate() {
            UpdateSectionTransforms();
            UpdatePreviousFrameTransforms();
        }

        private void GenerateSections() {
            _sections = new List<GameObject> { gameObject };
            for (int i = 0; i < _numberOfSections; i++) {
                GameObject _newSection = Instantiate(_sectionPrefab, transform.position + (-transform.up * (i + 1) * _sectionSpacing), transform.rotation, transform);
                _newSection.transform.localScale = Utilities.SetGlobalScale(_newSection.transform, transform.localScale.x * _sectionScale);
                _sections.Add(_newSection);
            }
        }

        private void UpdateSectionTransforms() {
            _sections[0].GetComponentInChildren<SpriteRenderer>().flipY = false;
            for (int i = 1; i < _sections.Count; i++) {
                _sections[i].transform.position = _sections[i - 1].transform.position + (_previousFrameSectionPositions[i] - _sections[i - 1].transform.position).normalized * _sectionSpacing;
                _sections[i].transform.rotation = Quaternion.Euler(0f, 0f, Vector2.SignedAngle(Vector2.up, (Vector2)_sections[i - 1].transform.position - (Vector2)_sections[i].transform.position));
                if (!_fishable.IsHooked) { // TODO: Find out if this check is necessary
                    _sections[i].transform.localScale = Utilities.SetGlobalScale(_sections[i].transform, transform.localScale.x * _sectionScale);
                }
            }
        }

        private void UpdatePreviousFrameTransforms() {
            _previousFrameSectionPositions = new List<Vector3>();
            _previousFrameSectionRotations = new List<Quaternion>();
            foreach (GameObject _section in _sections) {
                _previousFrameSectionPositions.Add(_section.transform.position);
                _previousFrameSectionRotations.Add(_section.transform.rotation);
            }
        }

        public void DespawnSections() {
            foreach(GameObject _section in _sections) {
                if (_section == gameObject) { 
                    continue;
                }
                Destroy(_section.gameObject);
            }
        }

        private void OnDestroy() {
            DespawnSections();
        }

        private void OnDrawGizmosSelected() {
            if (_drawSectionGizmos) {
                DrawSections();
            }
        }

        private void DrawSections() {
            Gizmos.color = _sectionGizmoColor;
            Gizmos.DrawSphere(_sections[0].transform.position, 0.05f);
            for (int i = 1; i < _sections.Count; i++) {
                Gizmos.color = Utilities.SetTransparency(_sectionGizmoColor, 1 - (1f / _sections.Count) * i);
                Gizmos.DrawSphere(_sections[i].transform.position, 0.05f);
                Gizmos.DrawRay(_sections[i].transform.position, (_previousFrameSectionPositions[i - 1] - _sections[i].transform.position));
            }
        }
    }
}
