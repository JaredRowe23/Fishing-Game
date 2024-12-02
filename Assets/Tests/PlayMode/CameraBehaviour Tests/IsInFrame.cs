using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Fishing.PlayerCamera;

namespace Tests.CameraBehaviourTests {
    public class IsInFrame {
        private float _testOrthographicSize = 10f;
        private float _outOfFrameDistance = 100f;

        [UnityTest]
        public IEnumerator When_in_frame_Should_return_true() {
            GameObject cameraGO = new GameObject();
            Camera camera = cameraGO.AddComponent<Camera>();
            CameraBehaviour cameraBehaviour = cameraGO.AddComponent<CameraBehaviour>();
            camera.orthographic = true;
            camera.orthographicSize = _testOrthographicSize;

            yield return null;

            Assert.IsTrue(cameraBehaviour.IsInFrame(Vector2.zero));
        }

        [UnityTest]
        public IEnumerator When_out_of_frame_Should_return_false() {
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