using UnityEngine;

public class StopParticles : MonoBehaviour
{
	public float time = 1f;

	private void Start()
	{
		Invoke(nameof(Stop), time);
	}

	private void Update()
	{
	}

	private void Stop()
	{
		var particles = GetComponent<ParticleSystem>(); if (particles) particles.Stop();
	}
}
