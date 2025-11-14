using UnityEngine;

public interface ISnapZone
{
	Transform GetJoint();
	Vector3 GetJointPosition();
	Vector3 GetJointRotation();
}