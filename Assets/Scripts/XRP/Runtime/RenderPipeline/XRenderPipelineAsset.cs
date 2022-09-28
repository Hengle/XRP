using UnityEngine;
using UnityEngine.Rendering;

namespace XRP.Runtime
{
    [CreateAssetMenu(menuName = "XRP/XRenderPipelineAsset")]
    public class XRenderPipelineAsset : RenderPipelineAsset
    {
        protected override RenderPipeline CreatePipeline()
        {
            return new XRenderPipeline(this);
        }
    }
}