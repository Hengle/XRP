
using UnityEngine;
using UnityEngine.Rendering;

namespace XRP.Runtime
{
    public class XCameraRender
    {
        public XCameraRender(XRenderPipelineAsset config)
        {
        }

        /// <summary>
        /// 渲染一个摄像机
        /// </summary>
        public void RenderCamera(ScriptableRenderContext context, Camera camera)
        {
            _Setup(context, camera);
            _ClearRenderTarget();
            _Culling();
            _DrawGeometry();
            _DrawSkyBox();
            context.Submit();
        }

        private void _Setup(ScriptableRenderContext context, Camera camera)
        {
            _Context = context;
            _Camera = camera;
            
            _Context.SetupCameraProperties(_Camera);
        }

        private void _ClearRenderTarget()
        {
            _CommandBuffer.ClearRenderTarget(true, true, Color.gray);
            _Context.ExecuteCommandBuffer(_CommandBuffer);
            _CommandBuffer.Clear();
        }

        private void _Culling()
        {
            _Camera.TryGetCullingParameters(out var cullingParameters);
            _CullingResults = _Context.Cull(ref cullingParameters);
        }

        private void _DrawGeometryForward()
        {
            var shaderTagId = new ShaderTagId("XRPForward");
            var sortingSettings = new SortingSettings(_Camera);
            var drawingSettings = new DrawingSettings(shaderTagId, sortingSettings);
            var filteringSettings = FilteringSettings.defaultValue;
            _Context.DrawRenderers(_CullingResults, ref drawingSettings, ref filteringSettings);
        }
        
        private void _DrawGeometry()
        {
            _DrawGeometryForward();
        }
        
        private void _DrawSkyBox()
        {
            if (_Camera.clearFlags == CameraClearFlags.Skybox && RenderSettings.skybox != null)
            {
                _Context.DrawSkybox(_Camera);
            }
        }

        private Camera _Camera;
        private ScriptableRenderContext _Context;
        private CullingResults _CullingResults;
        
        private readonly CommandBuffer _CommandBuffer = new CommandBuffer();
    }
}