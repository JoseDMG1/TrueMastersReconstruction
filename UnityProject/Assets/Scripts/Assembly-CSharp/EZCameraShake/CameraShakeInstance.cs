using UnityEngine;

namespace EZCameraShake
{
	public class CameraShakeInstance
	{
		public float Magnitude;

		public float Roughness;

		public Vector3 PositionInfluence;

		public Vector3 RotationInfluence;

		public bool DeleteOnInactive;

		private float roughMod;

		private float magnMod;

		private float fadeOutDuration;

		private float fadeInDuration;

		private bool sustain;

		private float currentFadeTime;

		private float tick;

		private Vector3 amt;

		public float ScaleRoughness
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		public float ScaleMagnitude
		{
			get
			{
				return 0f;
			}
			set
			{
			}
		}

		public float NormalizedFadeTime => 0f;

		private bool IsShaking => false;

		private bool IsFadingOut => false;

		private bool IsFadingIn => false;

		public CameraShakeState CurrentState => CameraShakeState.FadingIn;

		public CameraShakeInstance(float magnitude, float roughness, float fadeInTime, float fadeOutTime)
		{
		}

		public CameraShakeInstance(float magnitude, float roughness)
		{
		}

		public Vector3 UpdateShake()
		{
			return default;
		}

		public void StartFadeOut(float fadeOutTime)
		{
		}

		public void StartFadeIn(float fadeInTime)
		{
		}
	}
}
