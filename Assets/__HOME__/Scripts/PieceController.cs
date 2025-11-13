using UnityEngine;
using static IPiece;

[RequireComponent(typeof(Rigidbody))]
public class PieceController : MonoBehaviour, IPiece
{
	[SerializeField] private float _rotationSpeed = 10f;

	private PieceStates _state = PieceStates.Free;

	public PieceStates State => _state;
	public bool IsFree => _state == PieceStates.Free;
	public bool IsGrabbed => _state == PieceStates.Grabbed;
	public bool IsFixed => _state == PieceStates.Fixed;
	public bool IsLocked => _state == PieceStates.Locked;

	private Transform _followTarget = null;
	public Transform FollowTarget => _followTarget;

	private Rigidbody rb;

	void Awake()
	{
		rb = GetComponent<Rigidbody>();
	}

	public void Lock()
	{
		_state = PieceStates.Locked;

		rb.isKinematic = true;
		rb.useGravity = false;
	}
	public void StartFollow(Transform target)
	{
		_state = PieceStates.Grabbed;

		rb.isKinematic = true;
		rb.useGravity = false;

		_followTarget = target;
	}
	public void StopFollow()
	{
		_state = PieceStates.Free;

		_followTarget = null;
	}
	public void SnapTo(Transform joint, Vector3 position = default, Vector3 eulerRotation = default)
	{
		_state = PieceStates.Fixed;

		rb.isKinematic = true;
		rb.linearVelocity = Vector3.zero;
		rb.angularVelocity = Vector3.zero;

		transform.parent = joint;
		transform.SetLocalPositionAndRotation(position, Quaternion.Euler(eulerRotation));
	}

	public void Detach()
	{
		_state = PieceStates.Free;

		transform.parent = null;
		transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
		rb.isKinematic = false;
		rb.useGravity = true;
	}

	public void FixedRotate(Vector2 delta)
	{
		transform.rotation = Quaternion.Euler(delta);
	}

	void Update()
	{
		if (_state == PieceStates.Grabbed && _followTarget)
		{
			transform.position = _followTarget.transform.position;
		}
	}
}
