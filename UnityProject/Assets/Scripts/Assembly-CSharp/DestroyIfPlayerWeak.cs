using UnityEngine;

public class DestroyIfPlayerWeak : MonoBehaviour
{
	private void Update()
	{
		var configuration = GetComponent<ConfigAttackLayer>();
		if (configuration && configuration.playerWeak) Destroy(gameObject);
	}
}
