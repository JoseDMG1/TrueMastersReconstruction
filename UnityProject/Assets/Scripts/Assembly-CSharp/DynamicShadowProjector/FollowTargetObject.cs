using UnityEngine;

namespace DynamicShadowProjector
{
	public class FollowTargetObject : MonoBehaviour
	{
		public enum TextureAlignment
		{
			None = 0,
			TargetAxisX = 1,
			TargetAxisY = 2,
			TargetAxisZ = 3
		}

		public enum UpdateFunction
		{
			OnPreCull = 0,
			LateUpdate = 1,
			UpdateTransform = 2
		}

		[SerializeField]
		private Transform m_target;

		[SerializeField]
		private Transform m_targetDirection;

		[SerializeField]
		private TextureAlignment m_textureAlignment;

		[SerializeField]
		private UpdateFunction m_updateFunction;

		private Vector3 m_localTargetPosition;

		public Transform target
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public Transform targetDirection
		{
			get
			{
				return null;
			}
			set
			{
			}
		}

		public TextureAlignment textureAlignment
		{
			get
			{
				return TextureAlignment.None;
			}
			set
			{
			}
		}

		public UpdateFunction updateFunction
		{
			get
			{
				return UpdateFunction.OnPreCull;
			}
			set
			{
			}
		}

		public void UpdateTransform()
		{
		}

		private void Awake()
		{
		}

		private void LateUpdate()
		{
		}

		private void OnPreCull()
		{
		}
	}
}
