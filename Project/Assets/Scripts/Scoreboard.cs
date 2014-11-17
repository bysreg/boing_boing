using UnityEngine;
using System.Collections;

public class Scoreboard : MonoBehaviour {

	PlayerAttack[] playerAttacks;
	GUIText[] killCountTexts;
	GUIText[] deathCountTexts;

	int activePlayerCount;
	GameController gameController;

	void Awake()
	{
		gameController = GameObject.Find("GameController").GetComponent<GameController>();
		activePlayerCount = gameController.activePlayersCount;
		playerAttacks = new PlayerAttack[activePlayerCount];
		killCountTexts = new GUIText[activePlayerCount];
		deathCountTexts = new GUIText[activePlayerCount];
	}

	void Start()
	{
		Transform scoreboard = GameObject.Find("Scoreboard").transform;
		for(int i=0; i<activePlayerCount; i++)
		{
			playerAttacks[i] = gameController.GetPlayer(i + 1).GetComponent<PlayerAttack>();
			killCountTexts[i] = scoreboard.Find("P" + (i + 1)).Find("Kills").GetComponent<GUIText>();
			deathCountTexts[i] = scoreboard.Find("P" + (i + 1)).Find("Deaths").GetComponent<GUIText>();
		}
	}

	void Update()
	{
		for(int i=0; i<activePlayerCount; i++)
		{
			killCountTexts[i].text = "Kills : " + playerAttacks[i].GetKillCount();
			deathCountTexts[i].text = "Deaths : " + playerAttacks[i].GetDeathCount();
		}
	}
}
