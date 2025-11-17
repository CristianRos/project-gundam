using UnityEngine;
using UnityEngine.InputSystem;

public class Rotator : MonoBehaviour
{
	public float speed = 20f;

	float _yaw, _pitch, _roll = 0f;

	void Update()
	{
		// TODO(Cris): Change this to use a state
		if (!Keyboard.current.rKey.isPressed) return;

		_yaw += Mouse.current.delta.value.x * speed * Time.deltaTime;
		_pitch += Mouse.current.delta.value.y * speed * Time.deltaTime;
		_roll += Mouse.current.scroll.value.y * speed * 100f * Time.deltaTime;

		transform.localRotation = Quaternion.Euler(_yaw, _pitch, _roll);
	}

	public void SetRotation(float yaw, float pitch, float roll)
	{
		_yaw = yaw;
		_pitch = pitch;
		_roll = roll;

		transform.localRotation = Quaternion.Euler(_pitch, -_yaw, _roll);
	}
}