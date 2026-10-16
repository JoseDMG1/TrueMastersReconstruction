using UnityEngine;

public class SpringManager : MonoBehaviour
{
	public float dynamicRatio;

	public float stiffnessForce;

	public AnimationCurve stiffnessCurve;

	public float dragForce;

	public AnimationCurve dragCurve;

	public SpringBone[] springBones;

	private void Start()
	{
	}

	private void Update()
	{
	}

	private void LateUpdate()
	{
	}

	private void UpdateParameters()
	{
	}

	private void UpdateParameter(string fieldName, float baseValue, AnimationCurve curve)
	{
	}
}
