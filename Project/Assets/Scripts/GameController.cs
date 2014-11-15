using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

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
	float remainingGameTime;
	float MAX_GAME_TIME = 90f; // in seconds

	WinCameraController winCameraController;

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

		winCameraController = Camera.main.GetComponent<WinCameraController>();
	}

	void Init()
	{
		SetupPlayers();
		remainingGameTime = MAX_GAME_TIME;
		
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

		if(remainingGameTime > 0)
		{
			remainingGameTime -= Time.deltaTime;
			if(remainingGameTime <= 0f)
			{
				remainingGameTime = 0;
				FinishGame();
			}
		}
	}

	public void FinishGame()
	{
		//copy the players array
		Transform[] arr = new Transform[4];

		for(int i=0; i<4; i++)
		{
			arr[i] = players[i];
		}

		//search for player with highest kill, if they have the same kill count then compare the least death count, if it is still the same, compare who has the first kill

		Array.Sort(arr, CompareWinner);

		for(int i=0; i<activePlayersCount; i++)
		{
			PlayerAttack pa = arr[i].GetComponent<PlayerAttack>();
			print (i + " place : " + arr[i].name + " " + pa.GetKillCount() + " " + pa.GetDeathCount() + " " + pa.IsFirstKill());
		}

		winCameraController.Winneris(players[0].gameObject);
	}

	public float GetRemainingGameTime()
	{
		return remainingGameTime;
	}

	// 1 - based
    public GameObject GetPlayer(int index)
    {
        if(index <= players.Length)
        {
            return players[index-1].gameObject;
        }
        else
        {
            return null;
        }
    }

	public static int CompareWinner(Transform t1, Transform t2)
	{
		PlayerAttack pa1 = t1.GetComponent<PlayerAttack>();
		PlayerAttack pa2 = t2.GetComponent<PlayerAttack>();

		if(pa1.GetKillCount() != pa2.GetKillCount())
		{
			return (pa1.GetKillCount() > pa2.GetKillCount()) ? -1 : 1; // the most kill count
		}
		else if(pa1.GetDeathCount() != pa2.GetDeathCount())
		{
			return (pa1.GetDeathCount() < pa2.GetDeathCount()) ? -1 : 1; // the lease kill count
		}
		else if(pa1.IsFirstKill())
		{
			return -1; // the one who kills first
		}
		else
		{
			return 1;
		}
	}
}
