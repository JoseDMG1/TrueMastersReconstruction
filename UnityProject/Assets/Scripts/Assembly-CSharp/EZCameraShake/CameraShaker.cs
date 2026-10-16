using System.Collections.Generic;
using UnityEngine;

namespace EZCameraShake
{
	[AddComponentMenu("EZ Camera Shake/Camera Shaker")]
	public class CameraShaker : MonoBehaviour
	{
		public static CameraShaker Instance;

		private static Dictionary<string, CameraShaker> instanceList;

		public Vector3 DefaultPosInfluence;

		public Vector3 DefaultRotInfluence;

		public Vector3 RestPositionOffset;

		public Vector3 RestRotationOffset;

		private Vector3 posAddShake;

		private Vector3 rotAddShake;

		private List<CameraShakeInstance> cameraShakeInstances;

		public List<CameraShakeInstance> ShakeInstances => null;

		private void Awake()
		{
		}

		private void Update()
		{
		}

		public static CameraShaker GetInstance(string name)
		{
			return null;
		}

		public CameraShakeInstance Shake(CameraShakeInstance shake)
		{
			return null;
		}

		public CameraShakeInstance ShakeOnce(float magnitude, float roughness, float fadeInTime, float fadeOutTime)
		{
			return null;
		}

		public CameraShakeInstance ShakeOnce(float magnitude, float roughness, float fadeInTime, float fadeOutTime, Vector3 posInfluence, Vector3 rotInfluence)
		{
			return null;
		}

		public CameraShakeInstance StartShake(float magnitude, float roughness, float fadeInTime)
		{
			return null;
		}

		public CameraShakeInstance StartShake(float magnitude, float roughness, float fadeInTime, Vector3 posInfluence, Vector3 rotInfluence)
		{
			return null;
		}

		private void OnDestroy()
		{
		}
	}
}
