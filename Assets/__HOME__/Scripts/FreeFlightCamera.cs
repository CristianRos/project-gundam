using UnityEngine;
using UnityEngine.InputSystem;

public class FreeFlightCamera : MonoBehaviour
{
	[SerializeField] private float _movementSpeed = 35f;
	[SerializeField] private float _rotationSpeed = 50f;

	private float _yaw, _pitch = 0f;

	void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;

		Vector3 e = transform.localEulerAngles;
		_yaw = e.x;
		_pitch = -e.y;
	}

	void Update()
	{
		Move();

		// TODO(Cris): Change this to use a state
		bool isRotateObjectKeyPressed = Keyboard.current.rKey.isPressed || Keyboard.current.tKey.isPressed;
		if (!isRotateObjectKeyPressed) Rotate();
	}

	void Move()
	{
		Vector3 delta = new()
		{
			x = (Keyboard.current.dKey.value - Keyboard.current.aKey.value) * _movementSpeed * Time.deltaTime,
			y = (Keyboard.current.eKey.value - Keyboard.current.qKey.value) * _movementSpeed * Time.deltaTime,
			z = (Keyboard.current.wKey.value - Keyboard.current.sKey.value) * _movementSpeed * Time.deltaTime,
		};

		transform.Translate(delta, Space.Self);
	}

	void Rotate()
	{
		_yaw += Mouse.current.delta.value.x * _rotationSpeed * Time.deltaTime;
		_pitch += Mouse.current.delta.value.y * _rotationSpeed * Time.deltaTime;

		transform.localRotation = Quaternion.AngleAxis(_yaw, Vector3.up);
		transform.localRotation *= Quaternion.AngleAxis(-_pitch, Vector3.right);
	}
}