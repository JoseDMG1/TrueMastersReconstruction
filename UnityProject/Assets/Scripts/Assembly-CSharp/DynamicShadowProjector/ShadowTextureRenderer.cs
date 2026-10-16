using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

namespace DynamicShadowProjector
{
	[ExecuteAlways]
	[DisallowMultipleComponent]
	[RequireComponent(typeof(Projector))]
	public class ShadowTextureRenderer : MonoBehaviour, ISerializationCallbackReceiver
	{
        // Only the inventoried zero-blur/zero-mip Built-In profile is restored.
        private Material originalProjectorMaterial;
        public bool supportsAuthoredProfile => GraphicsSettings.currentRenderPipeline==null && m_blurLevel==0 && m_mipLevel==0 && (int)m_superSampling<=1 && !m_testViewClip;
        public bool shadowTextureValid => m_shadowTextureValid;
        private static void DestroyOwned(Object value) { if(Application.isPlaying)Destroy(value);else DestroyImmediate(value); }

		public enum TextureMultiSample
		{
			x1 = 1,
			x2 = 2,
			x4 = 4,
			x8 = 8
		}

		public enum TextureSuperSample
		{
			x1 = 1,
			x4 = 2,
			x16 = 4
		}

		public enum MipmapFalloff
		{
			None = 0,
			Linear = 1,
			Custom = 2
		}

		public enum BlurFilter
		{
			Uniform = 0,
			Gaussian = 1
		}

		private struct BlurParam
		{
			public int tap;

			public Vector4 offset;

			public Vector4 weight;
		}

		[SerializeField]
		private TextureMultiSample m_multiSampling;

		[SerializeField]
		private TextureSuperSample m_superSampling;

		[SerializeField]
		private MipmapFalloff m_mipmapFalloff;

		[SerializeField]
		private BlurFilter m_blurFilter;

		[SerializeField]
		private bool m_testViewClip;

		[SerializeField]
		private int m_textureWidth;

		[SerializeField]
		private int m_textureHeight;

		[SerializeField]
		private int m_mipLevel;

		[SerializeField]
		private int m_blurLevel;

		[SerializeField]
		private float m_blurSize;

		[SerializeField]
		private float m_mipmapBlurSize;

		[SerializeField]
		private bool m_singlePassMipmapBlur;

		[SerializeField]
		private Color m_shadowColor;

		[SerializeField]
		private Material m_blurShader;

		[SerializeField]
		private Material m_downsampleShader;

		[SerializeField]
		private Material m_copyMipmapShader;

		[SerializeField]
		private Material m_eraseShadowShader;

		[SerializeField]
		private float[] m_customMipmapFalloff;

		[SerializeField]
		private RenderTextureFormat[] m_preferredTextureFormats;

		[SerializeField]
		private Camera[] m_camerasForViewClipTest;

		private static int s_falloffParamID;

		private static int s_blurOffsetHParamID;

		private static int s_blurOffsetVParamID;

		private static int s_blurWeightHParamID;

		private static int s_blurWeightVParamID;

		private static int s_shadowColorParamID;

		private static int s_downSampleBlurOffset0ParamID;

		private static int s_downSampleBlurOffset1ParamID;

		private static int s_downSampleBlurOffset2ParamID;

		private static int s_downSampleBlurOffset3ParamID;

		private static int s_downSampleBlurWeightParamID;

		private static int s_blitSrcTexParamID;

		private static int s_blitSrcTexelSizeParamID;

		private static int s_shadowTexParamID;

		private static int s_mipLevelParamID;

		private Projector m_projector;

		private Material m_projectorMaterial;

		private CommandBuffer m_commandBuffer;

		[SerializeField]
		[HideInInspector]
		private Camera m_camera;

		private CommandBuffer m_postProcessCommands;

		private bool m_isTexturePropertyChanged;

		private bool m_isVisible;

		private bool m_shadowTextureValid;

		private static HashSet<Material> s_sharedMaterials;

		private const HideFlags CLONED_MATERIAL_HIDE_FLAGS = HideFlags.HideAndDontSave;

		private static int[] s_temporaryRenderTargetNameIds;

		private static RenderTargetIdentifier[] s_temporaryRenderTargetIdentifiers;

		private RenderTexture m_temporaryRenderTarget;

		private List<MaterialPropertyBlock> m_propertyBlockPool;

		private int m_propetyBlockCount;

		private static Dictionary<int, Mesh> s_borderMeshCache;

		private static readonly int[] s_borderMeshIndices;

		private static Mesh s_fullScreenQuad;

		private const int MAX_BLUR_TAP_SIZE = 7;

		private static float[] s_blurWeights;

		public TextureMultiSample multiSampling
		{ get { return m_multiSampling; } set { m_multiSampling=value; SetTexturePropertyDirty(); } }

		public TextureSuperSample superSampling
		{ get { return m_superSampling; } set { m_superSampling=value; SetTexturePropertyDirty(); } }

		public int textureWidth
		{ get { return m_textureWidth; } set { m_textureWidth=value; SetTexturePropertyDirty(); } }

		public int textureHeight
		{ get { return m_textureHeight; } set { m_textureHeight=value; SetTexturePropertyDirty(); } }

		public RenderTextureFormat[] preferredTextureFormats
		{ get { return m_preferredTextureFormats; } set { m_preferredTextureFormats=value; SetTexturePropertyDirty(); } }

		public int mipLevel
		{ get { return m_mipLevel; } set { m_mipLevel=value; SetTexturePropertyDirty(); } }

		public int blurLevel
		{ get { return m_blurLevel; } set { m_blurLevel=value; SetTexturePropertyDirty(); } }

		public float blurSize
		{ get { return m_blurSize; } set { m_blurSize=value; SetTexturePropertyDirty(); } }

		public BlurFilter blurFilter
		{ get { return m_blurFilter; } set { m_blurFilter=value; SetTexturePropertyDirty(); } }

		public float mipmapBlurSize
		{ get { return m_mipmapBlurSize; } set { m_mipmapBlurSize=value; SetTexturePropertyDirty(); } }

		public bool singlePassMipmapBlur
		{ get { return m_singlePassMipmapBlur; } set { m_singlePassMipmapBlur=value; SetTexturePropertyDirty(); } }

		public MipmapFalloff mipmapFalloff
		{ get { return m_mipmapFalloff; } set { m_mipmapFalloff=value; SetTexturePropertyDirty(); } }

		public float[] customMipmapFalloff
		{ get { return m_customMipmapFalloff; } set { m_customMipmapFalloff=value; SetTexturePropertyDirty(); } }

		public Color shadowColor
		{ get { return m_shadowColor; } set { m_shadowColor=value; SetTexturePropertyDirty(); } }

		public Material blurShader
		{ get { return m_blurShader; } set { m_blurShader=value; SetTexturePropertyDirty(); } }

		public Material downsampleShader
		{ get { return m_downsampleShader; } set { m_downsampleShader=value; SetTexturePropertyDirty(); } }

		public Material copyMipmapShader
		{ get { return m_copyMipmapShader; } set { m_copyMipmapShader=value; SetTexturePropertyDirty(); } }

		public Material eraseShadowShader
		{ get { return m_eraseShadowShader; } set { m_eraseShadowShader=value; SetTexturePropertyDirty(); } }

		public RenderTexture shadowTexture { get; private set; }

		public bool testViewClip
		{ get { return m_testViewClip; } set { m_testViewClip=value; SetTexturePropertyDirty(); } }

		public Camera[] camerasForViewClipTest
		{ get { return m_camerasForViewClipTest; } set { m_camerasForViewClipTest=value; SetTexturePropertyDirty(); } }

		public Camera projectorCamera { get { Initialize(); return m_camera; } }

		public float cameraNearClipPlane { get { return projectorCamera.nearClipPlane; } set { projectorCamera.nearClipPlane=value; } }

		public LayerMask cameraCullingMask { get { return projectorCamera.cullingMask; } set { projectorCamera.cullingMask=value; } }

		public bool isProjectorVisible => m_isVisible;

		private static bool isBuiltinRP => false;

		private bool useIntermediateTexture => false;

		public CommandBuffer postProcessCommandBuffer => null;

		public void SetReplacementShader(Shader shader, string replacementTag)
		{
            if (Initialize()) m_camera.SetReplacementShader(shader,replacementTag);
		}

		public void SetTexturePropertyDirty()
		{
            m_isTexturePropertyChanged=true;
		}

		public void CreateRenderTexture()
		{
            if (!m_camera || !m_projector || !supportsAuthoredProfile) return;
            if (shadowTexture) { m_camera.targetTexture=null; shadowTexture.Release(); DestroyOwned(shadowTexture); }
            var format=RenderTextureFormat.ARGB32;
            if(m_preferredTextureFormats!=null) foreach(var candidate in m_preferredTextureFormats) if(SystemInfo.SupportsRenderTextureFormat(candidate)) {format=candidate;break;}
            var descriptor=new RenderTextureDescriptor(Mathf.Max(1,m_textureWidth),Mathf.Max(1,m_textureHeight),format,16);
            int requested=Mathf.Clamp((int)m_multiSampling,1,8);
            descriptor.msaaSamples=requested==2||requested==4||requested==8 ? requested : 1;
            descriptor.msaaSamples=SystemInfo.GetRenderTextureSupportedMSAASampleCount(descriptor);
            shadowTexture=new RenderTexture(descriptor) {name="Reconstructed dynamic shadow",hideFlags=HideFlags.HideAndDontSave,wrapMode=TextureWrapMode.Clamp,filterMode=FilterMode.Bilinear};
            shadowTexture.Create(); m_camera.targetTexture=shadowTexture;
            m_projectorMaterial.SetTexture("_ShadowTex",shadowTexture);
            m_isTexturePropertyChanged=false; m_shadowTextureValid=false;
		}

		internal void AddCommandBuffer(CommandBuffer commandBuffer)
		{
            if(!Initialize())return;
            m_camera.RemoveCommandBuffer(CameraEvent.BeforeImageEffectsOpaque,commandBuffer);
            m_camera.AddCommandBuffer(CameraEvent.BeforeImageEffectsOpaque,commandBuffer);
		}

		internal void RemoveCommandBuffer(CommandBuffer commandBuffer)
		{
            if(m_camera) m_camera.RemoveCommandBuffer(CameraEvent.BeforeImageEffectsOpaque,commandBuffer);
		}

		private bool Initialize()
		{
            if(!gameObject.scene.IsValid()) return false;
            if(!m_projector)m_projector=GetComponent<Projector>();
            if(!m_projector || !m_projector.material) return false;
            if(!m_camera)m_camera=GetComponent<Camera>();
            if(!m_camera)m_camera=gameObject.AddComponent<Camera>();
            if(!m_projectorMaterial) { originalProjectorMaterial=m_projector.material; m_projectorMaterial=new Material(originalProjectorMaterial) {name=originalProjectorMaterial.name+" (owned shadow)",hideFlags=HideFlags.HideAndDontSave};m_projector.material=m_projectorMaterial; }
            m_camera.cullingMask=0; m_camera.clearFlags=CameraClearFlags.SolidColor; m_camera.backgroundColor=Color.white;
            m_camera.depth=-100; m_camera.allowHDR=false; m_camera.allowMSAA=true;
            m_camera.stereoTargetEye=StereoTargetEyeMask.None;
            return true;
		}

		private bool IsInitialized()
		{
            return m_camera && m_projectorMaterial && shadowTexture && shadowTexture.IsCreated();
		}

		private void Awake()
		{
		}

		private void OnEnable()
		{
            if(Initialize()) {CreateRenderTexture(); Update();}
		}

		private void OnDisable()
		{
            if(m_camera) {m_camera.enabled=false;m_camera.targetTexture=null;}
            if(m_projectorMaterial)m_projectorMaterial.SetTexture("_ShadowTex",Texture2D.whiteTexture);
            if(shadowTexture) {shadowTexture.Release();DestroyOwned(shadowTexture);shadowTexture=null;}
            m_isVisible=false;m_shadowTextureValid=false;
		}

		private void Start()
		{
		}

		private static bool IsPrefabAsset(Object obj)
		{
			return false;
		}

		private bool IsPrefabAsset()
		{
			return false;
		}

		private void OnValidate()
		{
            m_isTexturePropertyChanged=true;
		}

		private void OnApplicationPause(bool pause)
		{
		}

		private void OnApplicationFocus(bool focus)
		{
		}

		private void CloneProjectorMaterialIfShared()
		{
		}

		private void OnDestroy()
		{
            OnDisable();
            if(m_projector && m_projector.material==m_projectorMaterial)m_projector.material=originalProjectorMaterial;
            if(m_projectorMaterial)DestroyOwned(m_projectorMaterial);
		}

		private bool IsReadyToExecute()
		{
			return false;
		}

		private void SetVisible(bool isVisible)
		{
		}

		private static void StaticInitialize()
		{
		}

		static ShadowTextureRenderer()
		{
		}

		private void CreateTemporaryRenderTarget()
		{
		}

		private void ReleaseTemporaryRenderTarget()
		{
		}

		private void ClearPropertyBlockPool()
		{
		}

		private MaterialPropertyBlock AllocatePropertyBlock()
		{
			return null;
		}

		private void AddPostRenderPassCommands(CommandBuffer cmd, RenderTargetIdentifier srcId)
		{
		}

		private void AddEraseShadowOnBorderCommand(CommandBuffer cmd, int w, int h)
		{
		}

		private void AddBlitToCurrentTargetCommand(CommandBuffer cmd, RenderTargetIdentifier srcId, Material mat, int pass, MaterialPropertyBlock properties)
		{
		}

		private void Update()
		{
            if(!Initialize())return;
            if(supportsAuthoredProfile && (m_isTexturePropertyChanged || !shadowTexture || !shadowTexture.IsCreated()))CreateRenderTexture();
            m_camera.orthographic=m_projector.orthographic; m_camera.orthographicSize=m_projector.orthographicSize;
            m_camera.fieldOfView=m_projector.fieldOfView; m_camera.aspect=m_projector.aspectRatio;
            m_camera.nearClipPlane=m_projector.nearClipPlane; m_camera.farClipPlane=m_projector.farClipPlane;
            m_isVisible=m_projector.enabled && supportsAuthoredProfile && shadowTexture;
            m_camera.enabled=m_isVisible;
		}

		private void OnPreCull()
		{
            Update();
		}

		private bool HasShadowColor()
		{
			return false;
		}

		private void PrepareRendering()
		{
		}

		private void OnPreRender()
		{
		}

		private void OnPostRender()
		{
            m_shadowTextureValid=shadowTexture && shadowTexture.IsCreated();
		}

		private static BlurParam GetBlurParam(float blurSize, BlurFilter filter)
		{
			return default;
		}

		private static BlurParam GetDownsampleBlurParam(float blurSize, BlurFilter filter)
		{
			return default;
		}

		private void SetBlurShaderProperties(MaterialPropertyBlock dst, out BlurParam blurH, out BlurParam blurV)
		{
			blurH = default;
			blurV = default;
		}

		private void GetMipmapBlurParams(ref BlurParam blurH, ref BlurParam blurV)
		{
		}

		private void SetMipmapBlurShaderProperties(MaterialPropertyBlock dst, BlurParam blurH, BlurParam blurV)
		{
		}

		private void SetMipmapDownsampleWithBlurProperties(MaterialPropertyBlock dst, BlurParam blurH, BlurParam blurV, int w, int h)
		{
		}

		public void OnBeforeSerialize()
		{
		}

		public void OnAfterDeserialize()
		{
            m_isTexturePropertyChanged=true;
		}
	}
}
