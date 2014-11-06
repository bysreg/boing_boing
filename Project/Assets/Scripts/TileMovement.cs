using UnityEngine;
using System.Collections;

public class TileMovement : MonoBehaviour {

	bool isMoving;
	float targetHeight; // y axis
	float moveTime = 2f;
	float normalY;

	void Awake()
	{
		normalY = transform.position.y;
	}

	public void SetTileHeight(float targetHeight)
	{
		if(isMoving)
			return;

		if(transform.position.y == targetHeight)
		{
			return;
		}

		this.targetHeight = targetHeight;
		isMoving = true;
		iTween.MoveTo(this.gameObject, iTween.Hash("y", targetHeight, "easeType", "easeInOutExpo", "time", moveTime, "oncomplete", "OnMoveComplete", "oncompletetarget", this.gameObject));
	}

	public void OnMoveComplete()
	{
		isMoving = false;
		this.transform.position = new Vector3(transform.position.x, targetHeight, transform.position.z);
	}

	public bool IsWalkable()
	{
		bool ret = isMoving == false;
		ret = ret && transform.position.y == normalY;

		return ret;
	}

	public void SetTileHeightToNormal()
	{
		SetTileHeight(normalY);
	}
}
