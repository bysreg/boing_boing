using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAttack : MonoBehaviour {
	//static
	public static GameObject firstKillPlayer; // the player who kills first in the game

	const float MAX_FIST_TIME = 10f;
	private float fistTime = 0f;

	public GameObject punchEffect;
	public GameObject missEffect;
	private Transform cd;

	bool isMultipleFistExist = false;

	bool iscd = false;
	bool psMoveAvailable;
	GameObject fist;
	public GameObject[] additionalFist;
	float forceMagnitude = 10f;
	int index;
	int killCount;
	int deathCount;

	PlayerController playerController;
	GameController gameController;
	SoundController soundController;
	CharacterBaseController characterbaseController;

	//killing system
	GameObject lastHitFrom; // record who lands the last hit on this player, will reset back to null if lastHitExpireTime hits zero
	float lastHitExpireTime;
	float HIT_EXPIRE_TIME = 2f;

	void Awake()
	{
		playerController = GetComponent<PlayerController>();
		index = GetComponent<CharacterBaseController>().index;
		gameController = GameObject.Find("GameController").GetComponent<GameController>();
		soundController = GameObject.Find("GameController").GetComponent<SoundController>();		
		psMoveAvailable = PSMoveInput.IsConnected && PSMoveInput.MoveControllers[playerController.GetPSMoveIndex()].Connected;
		fist = transform.Find ("Fist").gameObject;

		//ShowMultipleFist ();
		HideMultipleFist ();

		//find cd animator
		//gameObject.GetComponentsInChildren<Animator> ().SetValue ();
//		foreach(Transform child in transform) {
//			if(child.name == "cd") {
//				cd = child;
//			}
//		}
	}

	void Start()
	{
		characterbaseController = GetComponent<CharacterBaseController>();
	}

	void Update()
	{
		// fist
		if (isMultipleFistExist) {
			fistTime -= Time.deltaTime;

			if (fistTime <= 0f) {
				HideMultipleFist();
			}
		}

		if(playerController != null)
		{
			if((index == 1 && Input.GetKeyDown(KeyCode.W)) ||
				(index == 2 && Input.GetKeyDown(KeyCode.A)) ||
				(index == 3 && Input.GetKeyDown(KeyCode.S)) ||
				(index == 4 && Input.GetKeyDown(KeyCode.D)) ||

			   (psMoveAvailable && PSMoveInput.MoveControllers[playerController.GetPSMoveIndex()].Data.ValueT > 0))
			{
				Attack();
			}
		}

		if(lastHitExpireTime > 0)
		{
			lastHitExpireTime -= Time.deltaTime;

			if(lastHitExpireTime <= 0)
			{
				lastHitFrom = null;
			}
		}
	}
    
	public void MissAnimation(Vector3 position) {
		soundController.PlaySound("whoosh", 0.6f, false);
		AnimateMiss(position);
	}

	public void HitAnimation(Vector3 position) {
		//play toet sound
		soundController.PlaySound("punch Sound", 0.6f, false);
		AnimateHit (position);
	}

    public void Attack()
	{
		fist.SendMessage("Attack");

		// multiple fist
		if (isMultipleFistExist) {
			for (int i = 0; i < additionalFist.Length; i++) {
				additionalFist[i].SendMessage("Attack", transform);
			}
		}
    }

	public void ShowMultipleFist() {
		isMultipleFistExist = true;
		fistTime = MAX_FIST_TIME;

		for (int i = 0; i < additionalFist.Length; i++) {
			additionalFist[i].SetActive(true);
		}
	}

	public void HideMultipleFist() {
		isMultipleFistExist = false;
		fistTime = 0f;

		for (int i = 0; i < additionalFist.Length; i++) {
			additionalFist[i].SetActive(false);
		}
	}

	public void KnockedDown(Vector3 direction, GameObject from)
	{
		lastHitFrom = from;
		lastHitExpireTime = HIT_EXPIRE_TIME;

		//there's a possibility that the player is knocked down while respawn in midair. 
		characterbaseController.SetHasTouchedTile(true);

		rigidbody.AddForce(direction * forceMagnitude, ForceMode.Impulse);
	}

	public void AnimateHit(Vector3 position) {
		GameObject tmp = Instantiate (punchEffect, position, Quaternion.identity) as GameObject;
		Destroy(tmp, 1f);
	}

	public void AnimateMiss(Vector3 position) {
		GameObject tmp =  Instantiate (missEffect, position, Quaternion.identity) as GameObject;
		Destroy(tmp, 1f);
    }

	public void AnimateCd(){
		StartCoroutine (AnimateCd_Routine());
	}

	IEnumerator AnimateCd_Routine() {
		if (!iscd) {
			iscd = true;
			cd.GetComponent<Animator> ().SetBool ("cd", true);
			yield return new WaitForSeconds (1f);
			iscd = false;
			cd.GetComponent<Animator> ().SetBool ("cd", false);
		} else {
			yield break;
		}
	}

	public int GetKillCount()
	{
		return killCount;
	}

	public void Killed()
	{
		if(lastHitFrom != null)
		{
			lastHitFrom.GetComponent<PlayerAttack>().IncKillCount();
			if(firstKillPlayer == null)
			{
				firstKillPlayer = lastHitFrom;
			}
		}

		deathCount++;
	}

	public void IncKillCount()
	{
		killCount++;
	}

	public GameObject GetLastHitFrom()
	{
		return lastHitFrom;
	}

	public int GetDeathCount()
	{
		return deathCount;
	}

	public bool IsFirstKill()
	{
		return firstKillPlayer == this.gameObject;
	}

	//-------test
//	public void SetValue(int killCount, int deathCount, bool isFirstKill)
//	{
//		this.killCount = killCount;
//		this.deathCount = deathCount;
//		if(isFirstKill)
//		{
//			firstKillPlayer = this.gameObject;
//		}
//	}
	//-------test
}
