using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using Fishing.PlayerCamera;

namespace Tests.CameraBehaviourTests {
    public class HandleCameraPosition {
        [UnityTest]
        public IEnumerator When_away_from_desired_position_Should_move_towards_desired_position() {
            GameObject cameraGO = new GameObject();
            Camera camera = cameraGO.AddComponent<Camera>();
            CameraBehaviour cameraBehaviour = cameraGO.AddComponent<CameraBehaviour>();
            cameraBehaviour.DesiredPosition = Vector2.zero;
            cameraGO.transform.position = Vector2.one * 100f;
            float beforeDistance = Vector2.Distance(cameraGO.transform.position, cameraBehaviour.DesiredPosition);

            yield return null;

            float afterDistance = Vector2.Distance(cameraGO.transform.position, cameraBehaviour.DesiredPosition);
            Assert.Less(afterDistance, beforeDistance);
        }
    }
}