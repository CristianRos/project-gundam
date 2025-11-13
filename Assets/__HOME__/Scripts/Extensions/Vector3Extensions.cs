using UnityEngine;

public static class Vector3Extensions
{
	public static Vector3 With(this Vector3 v, float? x, float? y, float? z) =>
		new Vector3(x ?? v.x, y ?? v.y, z ?? v.z);
}