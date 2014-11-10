using UnityEngine;
using System.Collections;

public class TileMovement : MonoBehaviour {

	bool isMoving;
	float targetHeight; // y axis
	float moveTime = 2f;
	float normalY;

	Vector3 normalPos;
	float shakingTime;
	float shakeSpeed = 10f;
	float MAX_SHAKE_TIME = Mathf.PI;

	void Awake()
	{
		normalY = transform.position.y;
	}

	void Start()
	{
		normalPos = transform.position;
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

	void Update()
	{
		if(shakingTime > 0)
		{
			shakingTime -= Time.deltaTime * shakeSpeed;
			if(shakingTime < 0)
			{
				shakingTime = 0;
			}
			transform.position = normalPos + new Vector3(0, -Mathf.Sin(shakingTime) * 0.2f, 0);
		}
	}

	public void ShakeTile()
	{
		shakingTime = MAX_SHAKE_TIME;
	}
}
