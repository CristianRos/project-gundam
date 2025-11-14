using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	public GameObject distancePoint;
	public IPiece _grabbedPiece;

	[SerializeField] private float _movementSpeed = 20f;
	[SerializeField] private float _rotationSpeed = 20f;
	[SerializeField] private float _distancePointSpeed = 20f;

	private bool _blockUpdate = false;

	float _cameraYaw, _cameraPitch, _cameraRoll;
	float _yaw, _pitch;

	void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		transform.SetPositionAndRotation(new Vector3(-2, 2, -8), Quaternion.identity);
	}

	void Update()
	{
		// Try block update
		if (Keyboard.current.escapeKey.wasPressedThisFrame)
		{
			_blockUpdate = !_blockUpdate;
		}

		if (_blockUpdate) return;

		MoveCamera();
		MoveDistancePoint();

		if (!Keyboard.current.rKey.isPressed && !Keyboard.current.tKey.isPressed) RotateCamera();
		else if (Keyboard.current.rKey.isPressed) OrbitDistancePoint();
		else RotateDistancePoint();


		TryGrabPiece();
		TrySnapPiece();
		TryDropPiece();
	}

	// TODO(Cris): Make this separated and a good system with full rotation of the object
	void OrbitDistancePoint()
	{
		if (_grabbedPiece == null) return;

		var delta = Mouse.current.delta.value;
		_yaw += delta.x * _rotationSpeed;
		_pitch -= delta.y * _rotationSpeed;

		distancePoint.transform.localRotation = Quaternion.Euler(_pitch, _yaw, 0);
	}

	// TODO(Cris): Make this separated and a good system with orbit rotation of the object
	void RotateDistancePoint()
	{
		if (_grabbedPiece == null) return;

		Vector2 delta = Mouse.current.delta.ReadValue();

		distancePoint.transform.rotation *= Quaternion.Euler(-delta.y, delta.x, 0);
	}

	void MoveCamera()
	{
		Vector3 velocity = new()
		{
			x = (Keyboard.current.dKey.value - Keyboard.current.aKey.value) * _movementSpeed * Time.deltaTime,
			y = (Keyboard.current.leftShiftKey.value - Keyboard.current.ctrlKey.value) * _movementSpeed * Time.deltaTime,
			z = (Keyboard.current.wKey.value - Keyboard.current.sKey.value) * _movementSpeed * Time.deltaTime,
		};

		transform.Translate(velocity, Space.Self);
	}

	void RotateCamera()
	{
		_cameraYaw = Mouse.current.delta.value.x * _rotationSpeed * Time.deltaTime;
		_cameraPitch = Mouse.current.delta.value.y * _rotationSpeed * Time.deltaTime;
		_cameraRoll = (Keyboard.current.qKey.value - Keyboard.current.eKey.value) * _rotationSpeed * Time.deltaTime;

		transform.rotation *= Quaternion.Euler(-_cameraPitch, _cameraYaw, _cameraRoll);
	}

	void MoveDistancePoint()
	{
		float mwheelDelta = Mouse.current.scroll.value.y * _distancePointSpeed * Time.deltaTime;

		distancePoint.transform.localPosition += Vector3.forward * mwheelDelta;
	}

	void TryGrabPiece()
	{
		Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 10f);
		if (hit.collider == null) return;

		Debug.DrawLine(transform.position, hit.point, Color.green);

		var piece = hit.collider.GetComponentInParent<IPiece>();
		if (piece == null) return;

		while (piece.ParentPiece != null) piece = piece.ParentPiece;

		if (!Mouse.current.leftButton.wasPressedThisFrame || piece.State != IPiece.PieceStates.Free)
			return;

		distancePoint.transform.rotation = piece.CurrentRotation;
		Vector3 e = distancePoint.transform.localEulerAngles;
		_yaw = RotationUtils.NormalizeAngle(e.x);
		_pitch = RotationUtils.NormalizeAngle(e.y);

		_grabbedPiece = piece;
		_grabbedPiece.TryStartFollow(distancePoint.transform);
	}

	void TrySnapPiece()
	{
		if (_grabbedPiece != null && Keyboard.current.spaceKey.wasPressedThisFrame)
			_grabbedPiece.TrySnap();
	}

	void TryDropPiece()
	{
		if (_grabbedPiece != null && Mouse.current.leftButton.wasReleasedThisFrame)
		{
			_grabbedPiece.StopFollow();
		}
	}
}