using UnityEngine;

public class ChangeRendererMat : MonoBehaviour
{
	public Material newMat;

	private void Start()
	{
	}

	public void ChangeColor(GameObject obj)
	{
		if (newMat == null) return;
		var changer = obj.GetComponent<ChangeRendererMat>();
		if (changer != null) changer.newMat = newMat;
		foreach (var particles in obj.GetComponentsInChildren<ParticleSystem>())
		{
			if (particles.CompareTag("UnColor")) continue;
			var renderer = particles.GetComponent<ParticleSystemRenderer>();
			renderer.material = newMat;
			renderer.trailMaterial = newMat;
		}
	}

	public void ChangeColorOnline()
	{
	}
}
