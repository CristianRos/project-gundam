using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

	public GameObject distancePoint;
	public IPiece _grabbedPiece;

	[SerializeField] private float _movementSpeed = 50f;
	[SerializeField] private float _rotationSpeed = 50f;
	[SerializeField] private float _distancePointSpeed = 10f;

	private bool _blockUpdate = false;

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

		if (!Keyboard.current.rKey.isPressed)
			RotateCamera();


		TryGrabPiece();
		TryDropPiece();
	}

	void FixedUpdate()
	{
		if (_grabbedPiece == null) return;

		if (Keyboard.current.rKey.isPressed)
		{
			_grabbedPiece.FixedRotate(Mouse.current.delta.value);
		}
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
		Vector3 rotDelta = new()
		{
			x = Mouse.current.delta.value.x * _rotationSpeed * Time.deltaTime,
			y = Mouse.current.delta.value.y * _rotationSpeed * Time.deltaTime,
			z = (Keyboard.current.qKey.value - Keyboard.current.eKey.value) * _rotationSpeed * Time.deltaTime,
		};

		transform.rotation *= Quaternion.Euler(-rotDelta.y, rotDelta.x, rotDelta.z);
	}

	void MoveDistancePoint()
	{
		float mwheelDelta = Mouse.current.scroll.value.y * _distancePointSpeed * Time.deltaTime;

		distancePoint.transform.Translate(Vector3.forward * mwheelDelta, Space.Self);
	}

	void TryGrabPiece()
	{
		if (Physics.Raycast(transform.position, transform.forward, out RaycastHit hit, 10f))
		{
			Debug.DrawLine(transform.position, hit.point, Color.green);

			var piece = hit.collider.GetComponentInParent<IPiece>();
			if (piece != null && Mouse.current.leftButton.wasPressedThisFrame)
			{
				_grabbedPiece = piece;
				_grabbedPiece.StartFollow(distancePoint.transform);
			}
		}
	}

	void TryDropPiece()
	{
		if (_grabbedPiece != null && Mouse.current.leftButton.wasReleasedThisFrame)
		{
			_grabbedPiece.StopFollow();
		}
	}
}