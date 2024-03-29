﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing.Testing
{
    public class Testing : MonoBehaviour
    {
        public ComputeShader testShader;

        private RenderTexture _target;

        private Camera _camera;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
        }

        private void OnRenderImage(RenderTexture source, RenderTexture destination)
        {
            Render(destination);
        }

        private void Render(RenderTexture destination)
        {
            // Make sure we have a current render target
            InitRenderTexture();

            // Set the target and dispatch the compute shader
            testShader.SetTexture(0, "Result", _target);
            int threadGroupsX = Mathf.CeilToInt(Screen.width / 8.0f);
            int threadGroupsY = Mathf.CeilToInt(Screen.height / 8.0f);
            testShader.Dispatch(0, threadGroupsX, threadGroupsY, 1);

            // Blit the result texture onto the screen
            Graphics.Blit(_target, destination);
        }

        private void InitRenderTexture()
        {
            if (_target == null || _target.width != Screen.width || _target.height != Screen.height)
            {
                // Release render texture if we already have one
                if (_target != null)
                {
                    _target.Release();
                }

                // Get a render target for Ray Tracing
                _target = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
                _target.enableRandomWrite = true;
                _target.Create();
            }
        }
    }

}