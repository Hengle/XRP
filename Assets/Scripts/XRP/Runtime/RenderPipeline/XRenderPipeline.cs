using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace XRP.Runtime
{
    public class XRenderPipeline : RenderPipeline
    {
        public XRenderPipeline(XRenderPipelineAsset config)
        {
            _Config = config;
            _CameraRender = new XCameraRender(config);
        }
        
        protected override void Render(ScriptableRenderContext context, Camera[] cameras)
        {
            foreach (var camera in cameras)
            {
                _CameraRender.RenderCamera(context, camera);
            }
        }

        private readonly XRenderPipelineAsset _Config;
        private readonly XCameraRender _CameraRender;
    }
}
