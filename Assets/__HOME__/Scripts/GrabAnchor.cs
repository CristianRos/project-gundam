using UnityEngine;
using UnityEngine.InputSystem;

public class GrabAnchor : MonoBehaviour
{
	[SerializeField]
	private Vector2 _distanceLimits = new(1f, 10f);

	[SerializeField] private float _stepDistance = 0.25f;

	void Update()
	{
		// TODO(Cris): Change this to use a state
		if (Keyboard.current.rKey.isPressed) return;

		float delta = Mouse.current.scroll.value.y * _stepDistance;

		float desired = Mathf.Clamp(
			transform.localPosition.z + delta,
			_distanceLimits.x,
			_distanceLimits.y
		);

		transform.localPosition = transform.localPosition.With(z: desired);
	}
}