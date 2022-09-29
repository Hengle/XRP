using UnityEngine.Rendering;

namespace XRP.Runtime
{
    public partial class XCameraRender
    {
        private void _InitOpaqueGeometryRenderSetting()
        {
            _FilteringSettingsOpaque = new FilteringSettings(RenderQueueRange.opaque);
        }
        
        
        private void _DrawOpaqueGeometry()
        {
            var sortingSettings = new SortingSettings(_Camera);
            var drawingSettings = new DrawingSettings(ShaderTagConst.Forword, sortingSettings);
            _Context.DrawRenderers(_CullingResults, ref drawingSettings, ref _FilteringSettingsOpaque);
        }
        
        private FilteringSettings _FilteringSettingsOpaque;
    }
}