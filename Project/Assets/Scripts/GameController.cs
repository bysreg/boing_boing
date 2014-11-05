using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	Transform p1;
	Transform p2;
	Transform p3;
	Transform p4;
	Transform[] players;
	TileController tileController;

	bool initialized;

	void Awake()
	{
		p1 = GameObject.Find("P1").transform;
		p2 = GameObject.Find("P2").transform;
		p3 = GameObject.Find("P3").transform;
		p4 = GameObject.Find("P4").transform;
		tileController = GetComponent<TileController>();
	}

	void Init()
	{
		SetupPlayers();
		
		initialized = true;
	}

	void SetupPlayers()
	{
		int boardWidth = tileController.boardWidth;
		int boardHeight = tileController.boardHeight;
		float oriY = p1.transform.position.y;

		p1.transform.position = tileController.GetWorldPos(0, 0) + new Vector3(0, oriY, 0);
		p2.transform.position = tileController.GetWorldPos(boardWidth - 1, 0) + new Vector3(0, oriY, 0);
		p3.transform.position = tileController.GetWorldPos(boardWidth - 1, boardHeight - 1) + new Vector3(0, oriY, 0);
		p4.transform.position = tileController.GetWorldPos(0, boardHeight - 1) + new Vector3(0, oriY, 0);
	}

	void Update()
	{
		if(!initialized)
		{
			Init();
		}
	}

}
