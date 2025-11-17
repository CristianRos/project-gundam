using UnityEngine;
using UnityEngine.Assertions;
using static IPiece;

public class PieceController : MonoBehaviour, IPiece
{
	[SerializeField] private float _rotationSpeed = 25f;
	[SerializeField] private float _followSpeed = 25f;

	private PieceStates _state = PieceStates.Free;

	public PieceStates State => _state;
	public bool IsFree => _state == PieceStates.Free;
	public bool IsGrabbed => _state == PieceStates.Grabbed;
	public bool IsFixed => _state == PieceStates.Fixed;
	public bool IsLocked => _state == PieceStates.Locked;

	public Quaternion CurrentRotation => transform.rotation;

	private Transform _followTarget = null;
	private Transform _followRotator = null;

	private Rigidbody _rigidbody;
	private Rigidbody _rigidbodyClone;

	private ISnapZone _snapZone;

	private IPiece _parentPiece = null;
	public IPiece ParentPiece => _parentPiece;

	public GameObject GameObject => gameObject;


	void Awake()
	{
		TryGetComponent(out Rigidbody rigidbody);
		Assert.IsNotNull(rigidbody);
		_rigidbody = rigidbody;
	}

	void FixedUpdate()
	{
		if (_state != PieceStates.Grabbed) return;

		// Follow position
		Vector3 toTarget = _followTarget.position - _rigidbody.position;
		_rigidbody.linearVelocity = toTarget * _followSpeed;

		// Follow rotation
		Quaternion deltaRot = _followRotator.rotation * Quaternion.Inverse(_rigidbody.rotation);
		deltaRot.ToAngleAxis(out float angle, out Vector3 axis);
		if (angle > 180f) angle -= 360f;

		Vector3 angularVelocity = _rotationSpeed * angle * Mathf.Deg2Rad * axis;
		_rigidbody.angularVelocity = angularVelocity;
	}

	public void Lock()
	{
		_state = PieceStates.Locked;

		_rigidbody.isKinematic = true;
		_rigidbody.useGravity = false;
	}

	public void TryStartFollow(Transform target, Transform rotator)
	{
		if (_state != PieceStates.Free || !_rigidbody) return;

		_state = PieceStates.Grabbed;

		// _rigidbody.isKinematic = true;
		_rigidbody.useGravity = false;

		_followTarget = target;
		_followRotator = rotator;
	}

	public void StopFollow()
	{
		if (_state == PieceStates.Fixed) return;

		_state = PieceStates.Free;

		_rigidbody.isKinematic = false;
		_rigidbody.useGravity = true;

		_followTarget = null;
	}

	public void TrySnap()
	{
		if (_snapZone == null || _snapZone.GetJoint() == null || _state != PieceStates.Grabbed) return;

		_state = PieceStates.Fixed;

		_rigidbodyClone = _rigidbody;
		Destroy(_rigidbody);
		_rigidbody = null;

		transform.parent = _snapZone.GetJoint();
		transform.SetLocalPositionAndRotation(_snapZone.GetJointPosition(), Quaternion.Euler(_snapZone.GetJointRotation()));
		_parentPiece = transform.parent.GetComponent<IPiece>();
	}

	public void Detach()
	{
		if (_state != PieceStates.Grabbed) return;

		_state = PieceStates.Free;

		if (_rigidbody == null)
			GenerateRigidbody();

		transform.parent = null;
		transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
		_parentPiece = null;

		_rigidbody.isKinematic = false;
		_rigidbody.useGravity = true;
	}

	void GenerateRigidbody()
	{
		Assert.IsNotNull(_rigidbodyClone);

		_rigidbody = gameObject.AddComponent<Rigidbody>();
		JsonUtility.FromJsonOverwrite(JsonUtility.ToJson(_rigidbodyClone), _rigidbody);
		_rigidbodyClone = null;
	}

	public void SetSnapZone(SnapZone zone)
	{
		Assert.IsNotNull(zone);

		_snapZone = zone;
	}

	public void RemoveSnapZone() =>
		_snapZone = null;
}
