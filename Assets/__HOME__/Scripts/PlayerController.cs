using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
	public GrabAnchor grabAnchor;
	public Rotator rotator;

	private IPiece _grabbedPiece;


	void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
		Cursor.visible = false;
	}

	void Update()
	{
		TryGrabPiece();
		TrySnapPiece();
		TryDropPiece();
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

		// Set the grab anchor to the current position of the piece
		grabAnchor.transform.position = piece.GameObject.transform.position;

		// Set the rotator to the current rotation of the piece
		Vector3 e = piece.CurrentRotation.eulerAngles;
		rotator.SetRotation(e.x, e.y, e.z);

		_grabbedPiece = piece;
		_grabbedPiece.TryStartFollow(grabAnchor.transform, rotator.transform);
	}

	void TrySnapPiece()
	{
		if (_grabbedPiece != null && Keyboard.current.spaceKey.wasPressedThisFrame)
			_grabbedPiece.TrySnap();
	}

	void TryDropPiece()
	{
		if (_grabbedPiece != null && Mouse.current.leftButton.wasReleasedThisFrame)
			_grabbedPiece.StopFollow();
	}
}