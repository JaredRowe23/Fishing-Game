using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

        [SerializeField] private List<Transform> sections;
        [SerializeField] private List<Vector3> previousFrameSectionPositions;
        [SerializeField] private List<Quaternion> previousFrameSectionRotations;

        private Fishable fishable;

        private void Awake()
        {
            fishable = GetComponent<Fishable>();
        }

        void Start()
        {
            sections.Add(transform);
            for (int i = 0; i < numberOfSections; i++)
            {
                GameObject newSection = Instantiate(sectionPrefab, transform.position, transform.rotation, null);
                newSection.transform.localScale = transform.localScale * sectionScale;
                newSection.transform.SetParent(transform.parent);
                sections.Add(newSection.transform);
            }

            previousFrameSectionPositions = new List<Vector3>();
            previousFrameSectionRotations = new List<Quaternion>();
            foreach(Transform _section in sections)
            {
                previousFrameSectionPositions.Add(_section.position);
                previousFrameSectionRotations.Add(_section.rotation);
            }
        }

        void Update()
        {
            for (int i = 1; i < sections.Count; i++)
            {
                sections[i].transform.position += (previousFrameSectionPositions[i - 1] - sections[i].transform.position) * sectionSpacing;
                sections[i].transform.Rotate(0f, 0f, Vector3.SignedAngle(sections[i].transform.right, sections[i - 1].right, Vector3.forward) * sectionRotationDampening);
                if (!fishable.isHooked)
                {
                    sections[i].transform.SetParent(null);
                    sections[i].transform.localScale = transform.localScale * sectionScale;
                    sections[i].transform.SetParent(transform.parent);
                }
            }


            previousFrameSectionPositions = new List<Vector3>();
            previousFrameSectionRotations = new List<Quaternion>();
            foreach (Transform _section in sections)
            {
                previousFrameSectionPositions.Add(_section.position);
                previousFrameSectionRotations.Add(_section.rotation);
            }
        }

        public void DespawnSections()
        {
            foreach(Transform _section in sections)
            {
                if (_section == transform) continue;
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
            foreach (Transform _section in sections)
            {
                Gizmos.DrawSphere(_section.position, 0.05f);
                Gizmos.color = new Color(Gizmos.color.r + 0.1f, Gizmos.color.g - 0.1f, 0);
            }

            Gizmos.color = new Color(0, 1, 0);
            for (int i = 1; i < sections.Count; i++)
            {
                Gizmos.DrawRay(sections[i].position, (previousFrameSectionPositions[i - 1] - sections[i].transform.position) * sectionSpacing);
                Gizmos.color = new Color(Gizmos.color.r + 0.1f, Gizmos.color.g - 0.1f, 0);
            }
        }
    }
}
