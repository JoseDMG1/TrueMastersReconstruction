using System;
using UnityEngine;

namespace DynamicShadowProjector
{
	[ExecuteAlways]
	public class PropertyBlockForTransparentReceiver : MonoBehaviour
	{
		[Flags]
		public enum Options
		{
			CopyMainTexFromMaterial = 1,
			CopyAlphaCutoffFromMaterial = 2,
			CopyBaseColorFromMaterial = 4,
			EnableAlphaCutoffByKeyword = 8,
			EnableAlphaCutoff = 0x10,
			CopyCullModeFromMaterial = 0x20,
			CullFront = 0x40,
			CullBack = 0x80
		}

		public enum CullMode
		{
			None = 0,
			Front = 0x40,
			Back = 0x80
		}

		[SerializeField]
		private Options m_options;

		public Texture mainTexture;

		public Vector4 mainTextureST;

		public float baseAlpha;

		public float alphaCutoff;

		public string mainTextureName;

		public string baseColorName;

		public string alphaCutoffName;

		public string alphaCutoffKeyword;

		public string cullModeName;

		private string m_mainTextureSTName;

		private Renderer m_renderer;

		private MaterialPropertyBlock m_propertyBlock;

		private static int s_dspBaseAlphaId;

		private static int s_dspCullModeId;

		private static int s_dspMainTexId;

		private static int s_dspMainTexStId;

		public bool copyMainTexFromMaterial
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public bool copyBaseColorFromMaterial
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public bool copyAlphaCutoffFromMaterial
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public bool copyCullModeFromMaterial
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public CullMode cullMode
		{
			get
			{
				return CullMode.None;
			}
			set
			{
			}
		}

		public bool enableAlphaCutoffByKeyword
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		public bool enableAlphaCutoff
		{
			get
			{
				return false;
			}
			set
			{
			}
		}

		private bool GetOption(Options option)
		{
			return false;
		}

		private void SetOption(Options option, bool value)
		{
		}

		static PropertyBlockForTransparentReceiver()
		{
		}

		private static void StaticInitialize()
		{
		}

		private Texture GetMaterialMainTexture()
		{
			return null;
		}

		private Vector4 GetMaterialMainTextureST()
		{
			return default;
		}

		private Color GetMaterialBaseColor()
		{
			return default;
		}

		private float GetMaterialAlphaCutoff()
		{
			return 0f;
		}

		private float GetMaterialCullMode()
		{
			return 0f;
		}

		public void UpdatePropertyBlock()
		{
		}

		private void Initialize()
		{
		}

		private void Awake()
		{
		}

		private void Start()
		{
		}
	}
}
