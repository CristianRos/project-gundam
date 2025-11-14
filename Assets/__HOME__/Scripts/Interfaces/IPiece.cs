using UnityEngine;

public interface IPiece
{
	enum PieceStates { Free, Grabbed, Locked, Fixed }

	PieceStates State { get; }
	bool IsFree { get; }
	bool IsGrabbed { get; }
	bool IsLocked { get; }
	bool IsFixed { get; }

	IPiece ParentPiece { get; }
	Transform FollowTarget { get; }

	void Lock();
	void TryStartFollow(Transform target);
	void StopFollow();
	void TrySnap();
	void Detach();

	void Rotate(Vector2 delta);

	void SetSnapZone(SnapZone zone);
	void RemoveSnapZone();
}