using UnityEngine;

public interface IPiece
{
	enum PieceStates { Free, Grabbed, Locked, Fixed }

	PieceStates State { get; }
	bool IsFree { get; }
	bool IsGrabbed { get; }
	bool IsLocked { get; }
	bool IsFixed { get; }

	Transform FollowTarget { get; }

	void Lock();
	void StartFollow(Transform target);
	void StopFollow();
	void SnapTo(Transform joint, Vector3 position = default, Vector3 eulerRotation = default);
	void Detach();

	void FixedRotate(Vector2 delta);
}