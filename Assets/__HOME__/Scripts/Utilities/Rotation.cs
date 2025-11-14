static class RotationUtils
{
	public static float NormalizeAngle(float a)
	{
		if (a > 180f) a -= 360f;
		return a;
	}
}