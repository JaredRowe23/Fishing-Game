using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.Util;

namespace Fishing.Fishables.Fish
{
    [RequireComponent(typeof(Fishable))]
    public class SectionedFish : MonoBehaviour
    {
        [SerializeField] private GameObject sectionPrefab;
        [SerializeField] private int numberOfSections = 20;
        [SerializeField] private float sectionScale = 0.75f;
        [SerializeField] private float sectionSpacing = 0.1f;
        [SerializeField] private float sectionRotationDampening = 0.1f;

        private List<GameObject> sections;
        private List<Vector3> previousFrameSectionPositions;
        private List<Quaternion> previousFrameSectionRotations;

        private Fishable fishable;

        private void Awake()
        {
            fishable = GetComponent<Fishable>();
        }

        void Start()
        {
            GenerateSections();
            UpdatePreviousFrameTransforms();
        }

        void Update()
        {
            UpdateSectionTransforms();
            UpdatePreviousFrameTransforms();
        }

        private void GenerateSections()
        {
            sections = new List<GameObject>();
            sections.Add(gameObject);
            for (int i = 0; i < numberOfSections; i++)
            {
                GameObject _newSection = Instantiate(sectionPrefab, transform.position + (-transform.up * (i + 1) * sectionSpacing), transform.rotation, transform);
                _newSection.transform.localScale = Utilities.SetGlobalScale(_newSection.transform, transform.localScale.x * sectionScale);
                sections.Add(_newSection);
            }
        }

        private void UpdateSectionTransforms()
        {
            sections[0].GetComponentInChildren<SpriteRenderer>().flipY = false;
            for (int i = 1; i < sections.Count; i++)
            {
                sections[i].transform.position = sections[i - 1].transform.position + (previousFrameSectionPositions[i] - sections[i - 1].transform.position).normalized * sectionSpacing;
                sections[i].transform.rotation = Quaternion.Euler(0f, 0f, Vector2.SignedAngle(Vector2.up, (Vector2)sections[i - 1].transform.position - (Vector2)sections[i].transform.position));
                if (!fishable.IsHooked)
                {
                    sections[i].transform.localScale = Utilities.SetGlobalScale(sections[i].transform, transform.localScale.x * sectionScale);
                }
            }
        }

        private void UpdatePreviousFrameTransforms()
        {
            previousFrameSectionPositions = new List<Vector3>();
            previousFrameSectionRotations = new List<Quaternion>();
            foreach (GameObject _section in sections)
            {
                previousFrameSectionPositions.Add(_section.transform.position);
                previousFrameSectionRotations.Add(_section.transform.rotation);
            }
        }

        public void DespawnSections()
        {
            foreach(GameObject _section in sections)
            {
                if (_section == gameObject) continue;
                GameObject.Destroy(_section.gameObject);
            }
        }

        private void OnDestroy()
        {
            DespawnSections();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = new Color(0, 1, 0);
            foreach (GameObject _section in sections)
            {
                Gizmos.DrawSphere(_section.transform.position, 0.05f);
                Gizmos.color = new Color(Gizmos.color.r + 0.1f, Gizmos.color.g - 0.1f, 0);
            }

            Gizmos.color = new Color(0, 1, 0);
            for (int i = 1; i < sections.Count; i++)
            {
                Gizmos.DrawRay(sections[i].transform.position, (previousFrameSectionPositions[i - 1] - sections[i].transform.position) * sectionSpacing);
                Gizmos.color = new Color(Gizmos.color.r + 0.1f, Gizmos.color.g - 0.1f, 0);
            }
        }
    }
}
