using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing.PlayerCamera
{
    public class BackgroundParallax : MonoBehaviour
    {
        [SerializeField] private List<Transform> parallaxTransforms;
        [SerializeField] private List<int> layers;
        [SerializeField] private float parallaxStrength;
        [SerializeField] private Vector3 parallaxOrigin;

        private List<Vector3> originalPositions;

        private Camera cam;

        void Awake()
        {
            cam = CameraBehaviour.instance.cam;
            parallaxOrigin = cam.transform.position;
        }

        void Start()
        {
            originalPositions = new List<Vector3>();
            for (int i = 0; i < parallaxTransforms.Count; i++)
            {
                originalPositions.Add(parallaxTransforms[i].position);
            }
        }

        void Update()
        {
            for (int i = 0; i < parallaxTransforms.Count; i++)
            {
                parallaxTransforms[i].position = originalPositions[i] + (cam.transform.position - parallaxOrigin) * parallaxStrength * layers[i];
            }
        }
    }
}
