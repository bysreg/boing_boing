using UnityEngine;
using System.Collections;

public class CameraMovement : MonoBehaviour {

	float panTime = 1f;
	Vector3 oriPos;
	float minZoom;
	float maxZoom;
	float deltaX;
	float deltaZ;

	int activePlayerCount;

	GameController gameController;

	void Awake()
	{
		oriPos = transform.position;
		gameController = GameObject.Find("GameController").GetComponent<GameController>();
		activePlayerCount = gameController.activePlayersCount;
	}

	public void PanTo(Vector3 target)
	{
		iTween.MoveTo(this.gameObject, iTween.Hash("position", target, "easeType", "easeInOutExpo", "time", panTime));
	}
	
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Q))
		{
			PanTo(oriPos + new Vector3(-5, -2, 2));
		}

		CalcDelta();

		Debug.DrawRay(transform.position, transform.forward * 15, Color.yellow);
	}

	void CalcDelta()
	{
		float minX = Mathf.Infinity, maxX = Mathf.NegativeInfinity, minZ = Mathf.Infinity, maxZ = Mathf.NegativeInfinity;

		for(int i=0; i<activePlayerCount; i++)
		{
			minX = Mathf.Min(minX, gameController.GetPlayer(i + 1).transform.position.x);
			maxX = Mathf.Max(maxX, gameController.GetPlayer(i + 1).transform.position.x);
			minZ = Mathf.Min(minZ, gameController.GetPlayer(i + 1).transform.position.z);
			maxZ = Mathf.Max(maxZ, gameController.GetPlayer(i + 1).transform.position.z);
		}

		deltaX = (minX + maxX) * 0.5f;
		deltaZ = (minZ + maxZ) * 0.5f;
	}
}
