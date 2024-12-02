using System.Collections;
using System.Collections.Generic;
using Fishing.PlayerCamera;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Tests.CameraBehaviourTests {
    public class CameraZoomOut {
        private int _testZoomSteps = 50;
        private float _timeForZoomToFinish = 3f;

        [UnityTest]
        public IEnumerator When_above_minimum_zoom_Should_increase_orthographic_size() {
            GameObject cameraGO = new GameObject();
            Camera camera = cameraGO.AddComponent<Camera>();
            CameraBehaviour cameraBehaviour = cameraGO.AddComponent<CameraBehaviour>();

            yield return null;

            float startingOrthoSize = camera.orthographicSize;
            cameraBehaviour.CameraZoomOut();

            yield return null;

            Assert.Greater(camera.orthographicSize, startingOrthoSize);
        }

        [UnityTest]
        public IEnumerator When_at_or_below_minimum_zoom_Should_clamp_orthographic_size() {
            GameObject cameraGO = new GameObject();
            Camera camera = cameraGO.AddComponent<Camera>();
            CameraBehaviour cameraBehaviour = cameraGO.AddComponent<CameraBehaviour>();

            yield return null;

            for (int i = 0; i < _testZoomSteps; i++) {
                cameraBehaviour.CameraZoomOut();
            }

            yield return new WaitForSeconds(_timeForZoomToFinish);

            float startingOrthoSize = camera.orthographicSize;
            cameraBehaviour.CameraZoomOut();

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

            cameraBehaviour.CameraZoomOut();

            yield return null;

            Assert.AreEqual(camera.orthographicSize, startingOrthoSize);
        }
    }
}