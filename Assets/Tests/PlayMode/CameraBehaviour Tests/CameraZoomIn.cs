using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Fishing.PlayerCamera;

namespace Tests.CameraBehaviourTests {
    public class CameraZoomIn {
        private int _testZoomSteps = 50;
        private float _timeForZoomToFinish = 3f;

        [UnityTest]
        public IEnumerator When_below_maximum_zoom_Should_decrease_orthographic_size() {
            GameObject cameraGO = new GameObject();
            Camera camera = cameraGO.AddComponent<Camera>();
            CameraBehaviour cameraBehaviour = cameraGO.AddComponent<CameraBehaviour>();

            yield return null;

            float startingOrthoSize = camera.orthographicSize;
            cameraBehaviour.CameraZoomIn();

            yield return null;

            Assert.Less(camera.orthographicSize, startingOrthoSize);
        }

        [UnityTest]
        public IEnumerator When_at_or_above_maximum_zoom_Should_clamp_orthographic_size() {
            GameObject cameraGO = new GameObject();
            Camera camera = cameraGO.AddComponent<Camera>();
            CameraBehaviour cameraBehaviour = cameraGO.AddComponent<CameraBehaviour>();

            yield return null;

            for (int i = 0; i < _testZoomSteps; i++) {
                cameraBehaviour.CameraZoomIn();
            }

            yield return new WaitForSeconds(_timeForZoomToFinish);

            float startingOrthoSize = camera.orthographicSize;
            cameraBehaviour.CameraZoomIn();

            yield return null;

            Assert.AreEqual(camera.orthographicSize, startingOrthoSize);
        }


        [UnityTest]
        public IEnumerator When_player_controls_locked_Should_return_with_no_action() {
            GameObject cameraGO = new GameObject();
            Camera camera = cameraGO.AddComponent<Camera>();
            CameraBehaviour cameraBehaviour = cameraGO.AddComponent<CameraBehaviour>();
            cameraBehaviour.LockPlayerControls = true;
            float startingOrthoSize = camera.orthographicSize;

            cameraBehaviour.CameraZoomIn();

            yield return null;

            Assert.AreEqual(camera.orthographicSize, startingOrthoSize);
        }
    }
}
