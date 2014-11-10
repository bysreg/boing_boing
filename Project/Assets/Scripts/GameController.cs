using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public int activePlayersCount = 4;

	Transform p1;
	Transform p2;
	Transform p3;
	Transform p4;
	Transform[] players;
	TileController tileController;

	bool initialized;
	float spawnYPos;

	void Awake()
	{
		p1 = GameObject.Find("P1").transform;
		p2 = GameObject.Find("P2").transform;
		p3 = GameObject.Find("P3").transform;
		p4 = GameObject.Find("P4").transform;
		tileController = GetComponent<TileController>();
		players = new Transform[4];
		players[0] = p1;
		players[1] = p2;
		players[2] = p3;
		players[3] = p4;

		for(int i=activePlayersCount; i < 4; i++)
		{
			players[i].gameObject.SetActive(false);
		}
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
		spawnYPos = p1.transform.position.y;

		p1.transform.position = tileController.GetWorldPos(0, 0) + new Vector3(0, spawnYPos, 0);
		p2.transform.position = tileController.GetWorldPos(boardWidth - 1, 0) + new Vector3(0, spawnYPos, 0);
		p3.transform.position = tileController.GetWorldPos(boardWidth - 1, boardHeight - 1) + new Vector3(0, spawnYPos, 0);
		p4.transform.position = tileController.GetWorldPos(0, boardHeight - 1) + new Vector3(0, spawnYPos, 0);
	}

	public void SpawnPlayer(int playerIndex)
	{
		Vector3 pos;
		GameObject player = players[playerIndex - 1].gameObject;
		int boardWidth = tileController.boardWidth;
		int boardHeight = tileController.boardHeight;
		float tileHeight = tileController.GetTileHeight();

		switch(playerIndex)
		{
		case 1 :
			pos = tileController.GetWorldPos(0, 0) + new Vector3(0, spawnYPos + tileHeight / 2, 0);
			break;
		case 2:
			pos = tileController.GetWorldPos(boardWidth - 1, 0) + new Vector3(0, spawnYPos + tileHeight / 2, 0);
			break;
		case 3:
			pos = tileController.GetWorldPos(boardWidth - 1, boardHeight - 1) + new Vector3(0, spawnYPos + tileHeight / 2, 0);
			break;
		case 4:
		default:
			pos = tileController.GetWorldPos(0, boardHeight - 1) + new Vector3(0, spawnYPos + tileHeight / 2, 0);
			break;
		}

		player.transform.position = pos;
		player.rigidbody.velocity = Vector3.zero;
		player.rigidbody.angularVelocity = Vector3.zero;
		player.GetComponent<CharacterBaseController>().Reset();
	}

	void Update()
	{
		if(!initialized)
		{
			Init();
		}
	}

	public void FinishGame(int winner)
	{
		print ("the winner is " + winner);
	}
}
