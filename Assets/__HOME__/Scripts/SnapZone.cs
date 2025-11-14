using UnityEngine;

public class SnapZone : MonoBehaviour, ISnapZone
{

	public Vector3 _jointPosition = new();
	public Vector3 _jointRotation = new();

	public Transform GetJoint() => transform.parent;
	public Vector3 GetJointPosition() => _jointPosition;
	public Vector3 GetJointRotation() => _jointRotation;

	void OnTriggerEnter(Collider other)
	{
		if (other.CompareTag("Joint"))
		{
			var piece = other.GetComponentInParent<IPiece>();
			piece.SetSnapZone(this);
		}
	}

	void OnTriggerExit(Collider other)
	{
		if (other.CompareTag("Joint"))
		{
			var piece = other.GetComponentInParent<IPiece>();
			piece.RemoveSnapZone();
		}
	}
}