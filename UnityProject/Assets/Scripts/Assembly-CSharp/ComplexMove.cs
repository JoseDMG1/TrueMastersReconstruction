using UnityEngine;

public class ComplexMove : MonoBehaviour
{
	[Header("Duraci\ufffdn total del movimiento (segundos)")]
	public float totalDuration;

	[Header("Curvas de velocidad normalizadas (valor entre 0 y 1, tiempo entre 0 y 1)")]
	public AnimationCurve forwardCurve;

	public AnimationCurve rightCurve;

	public AnimationCurve upCurve;

	[Header("Velocidades m\ufffdximas por direcci\ufffdn")]
	public float maxForwardSpeed;

	public float maxRightSpeed;

	public float maxUpSpeed;

	private Vector3 initialForward;

	private Vector3 initialRight;

	private Vector3 initialUp;

	private float startTime;

	private void Start()
	{
	}

	private void Update()
	{
	}
}
