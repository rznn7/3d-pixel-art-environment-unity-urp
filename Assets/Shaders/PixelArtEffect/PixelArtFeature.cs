using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class PixelArtFeature : ScriptableRendererFeature
{
    class PixelArtPass : ScriptableRenderPass
    {
        readonly Material _pixelArtMaterial;
        RenderTargetHandle _temporaryRenderTarget;

        public PixelArtPass(Material material)
        {
            _pixelArtMaterial = material;
            _temporaryRenderTarget.Init("_TemporaryRenderTarget");
        }

        public override void Execute(ScriptableRenderContext context, ref RenderingData renderingData)
        {
            var cmd = CommandBufferPool.Get("PixelArtEffect");

            var cameraColorTarget = renderingData.cameraData.renderer.cameraColorTarget;

            var cameraTextureDescriptor = renderingData.cameraData.cameraTargetDescriptor;
            cameraTextureDescriptor.depthBufferBits = 0;

            cmd.GetTemporaryRT(_temporaryRenderTarget.id, cameraTextureDescriptor, FilterMode.Point);

            Blit(cmd, cameraColorTarget, _temporaryRenderTarget.Identifier(), _pixelArtMaterial);
            Blit(cmd, _temporaryRenderTarget.Identifier(), cameraColorTarget);

            context.ExecuteCommandBuffer(cmd);
            CommandBufferPool.Release(cmd);

            cmd.ReleaseTemporaryRT(_temporaryRenderTarget.id);
        }
    }

    [SerializeField]
    Material pixelArtMaterial;
    PixelArtPass _pixelArtPass;

    public override void Create()
    {
        _pixelArtPass = new PixelArtPass(pixelArtMaterial)
        {
            renderPassEvent = RenderPassEvent.AfterRenderingPostProcessing
        };
    }

    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (pixelArtMaterial == null)
        {
            Debug.LogWarning("Missing Pixel Art Material");
            return;
        }

        renderer.EnqueuePass(_pixelArtPass);
    }
}
