using UnityEngine;
using System.Collections;

public class TileMovement : MonoBehaviour {

	bool isMoving;
	float targetHeight; // y axis
	float moveTime = 2f;

	float isWalkable;

	public void SetTileHeight(float targetHeight)
	{
		if(isMoving)
			return;

		this.targetHeight = targetHeight;
		isMoving = true;
		iTween.MoveTo(this.gameObject, iTween.Hash("y", targetHeight, "easeType", "easeInOutExpo", "time", moveTime, "oncomplete", "OnMoveComplete", "oncompletetarget", this.gameObject));
	}

	public void OnMoveComplete()
	{
		isMoving = false;
	}
}
