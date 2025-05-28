using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.Rendering.RenderGraphModule;
using UnityEngine.Rendering.RenderGraphModule.Util;

public class CustomRenderPassFeature : ScriptableRendererFeature
{
    class PixelatePass : ScriptableRenderPass
    {
        const string m_OutlinePassName = "OutlineEffectPass";
        Material m_OutlineMaterial;

        public void SetUp(Material outlineMat)
        {
            m_OutlineMaterial = outlineMat;
            requiresIntermediateTexture = true; //TODO: Needs to read current color texture of the scene?
        }

        // RecordRenderGraph is where the RenderGraph handle can be accessed, through which render passes can be added to the graph.
        // FrameData is a context container through which URP resources can be accessed and managed.
        public override void RecordRenderGraph(UnityEngine.Rendering.RenderGraphModule.RenderGraph renderGraph, ContextContainer frameData)
        {
            var resourceData = frameData.Get<UniversalResourceData>();
            if (resourceData.isActiveTargetBackBuffer)
            {
                Debug.LogError($"Skipping render pass. Needs an intermediate ColorTexture, cannot use BackBuffer as " +
                    $"a texture input."); //TODO: is this needed?
                return;
            }
            //Texture to use in blit operation
            var source = resourceData.activeColorTexture;

            //Render texture properties in render graph description struct
            var destinationDesc = renderGraph.GetTextureDesc(source);
            //Modify, not start from new!
            destinationDesc.clearBuffer = false;

            //Name of destination texture should match pass
            destinationDesc.name = $"CameraColor-{m_OutlinePassName}";
            //Destination texture handle with set up parameters
            UnityEngine.Rendering.RenderGraphModule.TextureHandle destination = renderGraph.CreateTexture(destinationDesc);

            //Have source and destination, material and default Shader pass 0 for blit operation
            //Helper struct, for outline blit operation
            RenderGraphUtils.BlitMaterialParameters outlineBlit = new(source, destination,
                m_OutlineMaterial, 0);
            renderGraph.AddBlitPass(outlineBlit, passName: m_OutlinePassName);

            resourceData.cameraColor = destination;
        }

        //OnCameraSetUp, Excute, OnCameraCleanup not used in Render graph, will be deprecated
    }

    public RenderPassEvent outlineInjectionPoint = RenderPassEvent.BeforeRenderingPostProcessing;
    public Material pixelMaterial;

    PixelatePass m_ScriptablePixelatePass;

    /// <inheritdoc/>
    public override void Create()
    {
        m_ScriptablePixelatePass = new PixelatePass();

        // Configures where the render pass should be injected.
        m_ScriptablePixelatePass.renderPassEvent = outlineInjectionPoint;

    }

    // Here you can inject one or multiple render passes in the renderer.
    // This method is called when setting up the renderer once per-camera.
    public override void AddRenderPasses(ScriptableRenderer renderer, ref RenderingData renderingData)
    {
        if (pixelMaterial != null)
        {
            m_ScriptablePixelatePass.SetUp(pixelMaterial);
            renderer.EnqueuePass(m_ScriptablePixelatePass);
        }
    }
}
