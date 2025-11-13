using UnityEngine;
using UnityEngine.InputSystem;

public class ReverseJoint : MonoBehaviour
{
	bool _isJointInside = false;
	Transform _colJointTransform;
	SphereCollider _colJointSphereCollider;

	public Vector3 _jointPosition = new();
	public Vector3 _jointRotation = new();

	void Update()
	{
		if (_isJointInside && Keyboard.current.spaceKey.wasPressedThisFrame)
		{
			_colJointTransform.GetComponent<IPiece>().SnapTo(transform.parent, _jointPosition, _jointRotation);
			_colJointSphereCollider.enabled = false;
		}
	}

	void OnTriggerEnter(Collider other)
	{
		Debug.Log("Trigger Enter");
		if (other.CompareTag("Joint"))
		{
			_isJointInside = true;
			_colJointTransform = other.GetComponentInParent<PieceController>().transform;
			Debug.Log(_colJointTransform);
			_colJointSphereCollider = other.GetComponent<SphereCollider>();
		}
	}

	void OnTriggerExit(Collider other)
	{
		Debug.Log("Trigger Exit");
		if (other.CompareTag("Joint"))
		{
			_isJointInside = false;
			_colJointTransform = null;
			_colJointSphereCollider = null;
		}
	}
}