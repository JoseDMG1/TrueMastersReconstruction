using System.Collections.Generic;
using UnityEngine;

public class Utilities : MonoBehaviour
{
	public static Utilities i;

	public bool FindObjsWithComp;

	public string ComponentName;

	public string directory;

	private void Start()
	{
		if (!i) i = this;
		else if (i != this) Destroy(this);
	}

	public object GetFieldValue(object src, string propName)
	{
		return null;
	}

	public void SetFieldValue(object src, string propName, int value)
	{
	}

	public string removeSimbols(string region)
	{
		return null;
	}

	public string GetTimeAsString(int time)
	{
		return (time / 60).ToString() + ":" + (time % 60).ToString("D2");
	}

	public Vector3 QuaternionToVector3(Quaternion rot)
	{
		// Native 0x12DB80C: Internal_ToEulerRad, Rad2Deg, Internal_MakePositive.
		return rot.eulerAngles;
	}

	public Quaternion Vector3ToQuaternion(Vector3 V3)
	{
		return default;
	}

	public float DistanceBtwTwoObjs(GameObject firstObj, GameObject secondObj)
	{
		return Vector3.Distance(firstObj.transform.position, secondObj.transform.position);
	}

	public bool AreObjetsNear(GameObject firstObj, GameObject secondObj, float limitDistance)
	{
		return DistanceBtwTwoObjs(firstObj, secondObj) < limitDistance;
	}

	public static List<GameObject> LoadPrefabsContaining<T>(string path) where T : Component
	{
		return null;
	}

	public float GetAngleDiference(GameObject obj1, GameObject obj2)
	{
		// VA 0x12DB994: collider direction first, defender direction second.
		return Vector3.SignedAngle(obj2.transform.forward, obj1.transform.forward, Vector3.up);
	}

	public static string GetRelativePosition(Transform referenceObject, Transform targetObject)
	{
		return null;
	}

	public static string GetRelativePositionByAngle(Transform referenceObject, Transform targetObject, float frontBackRatio = 0.5f, float frontRatio = 0.5f)
	{
		// VA 0x12DBD14. Classify only the horizontal displacement.
		frontBackRatio = Mathf.Clamp01(frontBackRatio);
		frontRatio = Mathf.Clamp01(frontRatio);
		var direction = targetObject.position - referenceObject.position;
		direction.y = 0f;
		float angle = Vector3.SignedAngle(referenceObject.forward, direction, Vector3.up);
		float span = 180f * frontBackRatio;
		if (Mathf.Abs(angle) <= span * frontRatio * .5f) return "In Front";
		if (Mathf.Abs(angle) >= 180f - span * (1f - frontRatio) * .5f) return "Behind";
		return angle > 0f ? "Left" : "Right";
	}

	public float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
	{
		return 0f;
	}

	public long GetDateTime()
	{
		return 0L;
	}

	public List<GameObject> GetAllChilds(GameObject Go)
	{
		return null;
	}
}
