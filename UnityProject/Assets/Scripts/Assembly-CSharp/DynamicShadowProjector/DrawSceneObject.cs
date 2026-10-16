using UnityEngine;

namespace DynamicShadowProjector
{
	[ExecuteAlways]
	[DisallowMultipleComponent]
	[RequireComponent(typeof(ShadowTextureRenderer))]
	public class DrawSceneObject : MonoBehaviour
	{
		[SerializeField]
		private Shader m_replacementShader;

		[SerializeField]
		private LayerMask m_cullingMask;

		private ShadowTextureRenderer m_shadowTextureRenderer;

		public Shader replacementShader
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public LayerMask cullingMask
		{
			get
			{
				return default;
			}
			set
			{
			}
		}

		public ShadowTextureRenderer shadowTextureRenderer => null;

		private void OnValidate()
		{
		}

		private void OnEnable()
		{
		}

		private void OnDisable()
		{
		}

		private void OnVisibilityChanged(bool isVisible)
		{
		}
	}
}
