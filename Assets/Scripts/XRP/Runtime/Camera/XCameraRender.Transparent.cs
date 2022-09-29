using UnityEngine.Rendering;

namespace XRP.Runtime
{
    public partial class XCameraRender
    {
        private void _InitTransparentGeometryRenderSetting()
        {
            _FilteringSettingsTransparent = new FilteringSettings(RenderQueueRange.transparent);
        }
        
        
        private void _DrawTransparentGeometry()
        {
            var sortingSettings = new SortingSettings(_Camera);
            var drawingSettings = new DrawingSettings(ShaderTagConst.Forword, sortingSettings);
            _Context.DrawRenderers(_CullingResults, ref drawingSettings, ref _FilteringSettingsTransparent);
        }
        
        private FilteringSettings _FilteringSettingsTransparent;
    }
}