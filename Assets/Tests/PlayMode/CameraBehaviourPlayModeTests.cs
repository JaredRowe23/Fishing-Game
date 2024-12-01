using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Fishing.PlayerCamera;

namespace Tests {
    public class CameraBehaviourPlayModeTests {
        private float _timeForZoomToFinish = 3f;
        private float _outOfFrameDistance = 100f;
        private float _testOrthographicSize = 10f;
        private int _testZoomSteps = 50;

        [UnityTest]
        public IEnumerator CameraZoomIn_lowers_orthographic_size() {
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
        public IEnumerator CameraZoomOut_increases_orthographic_size() {
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
        public IEnumerator CameraZoomIn_clamps_orthographic_size_to_lower_bound_when_too_small() {
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
        public IEnumerator CameraZoomOut_clamps_orthographic_size_to_upper_bound_when_too_high() {
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
        public IEnumerator CameraZoomIn_returns_with_no_action_when_player_controls_locked() {
            GameObject cameraGO = new GameObject();
            Camera camera = cameraGO.AddComponent<Camera>();
            CameraBehaviour cameraBehaviour = cameraGO.AddComponent<CameraBehaviour>();

            yield return null;

            cameraBehaviour.LockPlayerControls = true;
            float startingOrthoSize = camera.orthographicSize;
            cameraBehaviour.CameraZoomIn();

            yield return null;

            Assert.AreEqual(camera.orthographicSize, startingOrthoSize);
        }

        [UnityTest]
        public IEnumerator CameraZoomOut_returns_with_no_action_when_player_controls_locked() {
            GameObject cameraGO = new GameObject();
            Camera camera = cameraGO.AddComponent<Camera>();
            CameraBehaviour cameraBehaviour = cameraGO.AddComponent<CameraBehaviour>();

            yield return null;

            cameraBehaviour.LockPlayerControls = true;
            float startingOrthoSize = camera.orthographicSize;
            cameraBehaviour.CameraZoomOut();

            yield return null;

            Assert.AreEqual(camera.orthographicSize, startingOrthoSize);
        }

        [UnityTest]
        public IEnumerator IsInFrame_returns_true_when_in_frame() {
            GameObject cameraGO = new GameObject();
            Camera camera = cameraGO.AddComponent<Camera>();
            CameraBehaviour cameraBehaviour = cameraGO.AddComponent<CameraBehaviour>();
            camera.orthographic = true;
            camera.orthographicSize = _testOrthographicSize;

            yield return null;

            Assert.IsTrue(cameraBehaviour.IsInFrame(Vector2.zero));
        }

        [UnityTest]
        public IEnumerator IsInFrame_returns_false_when_out_of_frame() {
            GameObject cameraGO = new GameObject();
            Camera camera = cameraGO.AddComponent<Camera>();
            CameraBehaviour cameraBehaviour = cameraGO.AddComponent<CameraBehaviour>();
            camera.orthographic = true;
            camera.orthographicSize = _testOrthographicSize;

            yield return null;

            Assert.IsFalse(cameraBehaviour.IsInFrame(Vector2.one * _outOfFrameDistance));
        }
    }
}