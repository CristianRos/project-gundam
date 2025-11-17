using UnityEngine;

public interface IPiece
{
	enum PieceStates { Free, Grabbed, Locked, Fixed }

	PieceStates State { get; }
	bool IsFree { get; }
	bool IsGrabbed { get; }
	bool IsLocked { get; }
	bool IsFixed { get; }

	Quaternion CurrentRotation { get; }

	IPiece ParentPiece { get; }
	GameObject GameObject { get; }

	void Lock();
	void TryStartFollow(Transform target, Transform rotator);
	void StopFollow();
	void TrySnap();
	void Detach();

	void SetSnapZone(SnapZone zone);
	void RemoveSnapZone();
}