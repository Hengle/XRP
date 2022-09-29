
using UnityEngine;
using UnityEngine.Rendering;

namespace XRP.Runtime
{
    public class XCameraRender
    {
        private const string BufferName = "Render Camera";
        
        public XCameraRender(XRenderPipelineAsset config)
        {
            _CommandBuffer = new CommandBuffer()
            {
                name = BufferName
            };
            
            _FilteringSettingsOpaque = new FilteringSettings(RenderQueueRange.opaque);
            _FilteringSettingsTransparent = new FilteringSettings(RenderQueueRange.transparent);
        }

        /// <summary>
        /// 渲染一个摄像机
        /// </summary>
        public void RenderCamera(ScriptableRenderContext context, Camera camera)
        {
            _Context = context;
            _Camera = camera;

            if (!_Culling())
            {
                return;
            }
            
            _Setup(context, camera);
            _ClearRenderTarget();
            _BeginSample();
            _Culling();
            _DrawOpaqueGeometry();
            _DrawSkyBox();
            _DrawTransparentGeometry();
            _EndSample();
            _Submit();
        }

        private void _BeginSample()
        {
            _CommandBuffer.BeginSample(BufferName);
            _Context.ExecuteCommandBuffer(_CommandBuffer);
            _CommandBuffer.Clear();
        }

        private void _EndSample()
        {
            _CommandBuffer.EndSample(BufferName);
            _Context.ExecuteCommandBuffer(_CommandBuffer);
            _CommandBuffer.Clear();
        }
        
        private void _Setup(ScriptableRenderContext context, Camera camera)
        {
            _Context.SetupCameraProperties(_Camera);
        }

        private void _ClearRenderTarget()
        {
            _CommandBuffer.ClearRenderTarget(true, true, Color.gray);
            _Context.ExecuteCommandBuffer(_CommandBuffer);
            _CommandBuffer.Clear();
        }

        private bool _Culling()
        {
            if (_Camera.TryGetCullingParameters(out var cullingParameters))
            {
                _CullingResults = _Context.Cull(ref cullingParameters);
                return true;
            }
            return false;
        }

        
        private void _DrawOpaqueGeometry()
        {
            var shaderTagId = new ShaderTagId("XRPForward");
            var sortingSettings = new SortingSettings(_Camera);
            var drawingSettings = new DrawingSettings(shaderTagId, sortingSettings);
            _Context.DrawRenderers(_CullingResults, ref drawingSettings, ref _FilteringSettingsOpaque);
        }
        
        private void _DrawSkyBox()
        {
            if (_Camera.clearFlags == CameraClearFlags.Skybox && RenderSettings.skybox != null)
            {
                _Context.DrawSkybox(_Camera);
            }
        }

        private void _DrawTransparentGeometry()
        {
            var shaderTagId = new ShaderTagId("XRPForward");
            var sortingSettings = new SortingSettings(_Camera);
            var drawingSettings = new DrawingSettings(shaderTagId, sortingSettings);
            _Context.DrawRenderers(_CullingResults, ref drawingSettings, ref _FilteringSettingsTransparent);
        }

        private void _Submit()
        {
            _Context.Submit();
        }

        private Camera _Camera;
        private ScriptableRenderContext _Context;
        private CullingResults _CullingResults;
        private FilteringSettings _FilteringSettingsOpaque;
        
        private FilteringSettings _FilteringSettingsTransparent;

        private readonly CommandBuffer _CommandBuffer;
    }
}