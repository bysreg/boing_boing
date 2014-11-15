using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAttack : MonoBehaviour {

	//static
	public static GameObject firstKillPlayer; // the player who kills first in the game

	public GameObject punchEffect;
	public GameObject missEffect;
	private Transform cd;

	bool iscd = false;
	bool psMoveAvailable;
	GameObject fist;
	float freezeTime; // time until the player can attack
	const float MAX_FREEZE_TIME = 0.5f;
	float forceMagnitude = 14f;
	List<GameObject> playersInsideHitArea;
	float attackDistance;
	float sqrAttackDistance;
	int index;
	Vector3 fistTargetPos;
	Vector3 fistOriPos;
	int killCount;
	int deathCount;

	PlayerController playerController;
	GameController gameController;
	SoundController soundController;

	//killing system
	GameObject lastHitFrom; // record who lands the last hit on this player, will reset back to null if lastHitExpireTime hits zero
	float lastHitExpireTime;
	float HIT_EXPIRE_TIME = 2f;

	void Awake()
	{
		playerController = GetComponent<PlayerController>();
		playersInsideHitArea = new List<GameObject>();
		index = GetComponent<CharacterBaseController>().index;
		gameController = GameObject.Find("GameController").GetComponent<GameController>();
		attackDistance = GetComponent<BoxCollider>().size.z * transform.localScale.z;
		sqrAttackDistance = GetComponent<BoxCollider>().size.z * GetComponent<BoxCollider>().size.z * transform.localScale.z;
		soundController = GameObject.Find("GameController").GetComponent<SoundController>();		
		psMoveAvailable = PSMoveInput.IsConnected && PSMoveInput.MoveControllers[playerController.psMoveIndex].Connected;
		fist = transform.Find ("Fist").gameObject;
		fistOriPos = fist.transform.localPosition;
        //fist.SetActive(false);

		//find cd animator
		//gameObject.GetComponentsInChildren<Animator> ().SetValue ();
//		foreach(Transform child in transform) {
//			if(child.name == "cd") {
//				cd = child;
//			}
//		}
	}

	void Update()
	{
		if(freezeTime == 0)
		{
			if(playerController != null)
			{
				if((index == 1 && Input.GetKeyDown(KeyCode.W)) ||
					(index == 2 && Input.GetKeyDown(KeyCode.A)) ||
					(index == 3 && Input.GetKeyDown(KeyCode.S)) ||
					(index == 4 && Input.GetKeyDown(KeyCode.D)) ||

				   	(psMoveAvailable && PSMoveInput.MoveControllers[playerController.psMoveIndex].Data.ValueT > 0))
				{
					Attack();
				}
			}
		}
		else
		{
			AnimateFist();

			freezeTime -= Time.deltaTime;
			if(freezeTime <= 0)
			{
				freezeTime = 0;
                //fist.SetActive(false);
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

	void OnTriggerEnter(Collider other)
	{
		if(other.isTrigger)
			return;

		if(other.tag == "Player" || other.tag == "Boundary")
		{
			playersInsideHitArea.Add(other.gameObject);
        }
	}

	void OnTriggerExit(Collider other)
	{
		if(other.isTrigger)
			return;

		if(other.tag == "Player" || other.tag == "Boundary")
		{
			playersInsideHitArea.Remove(other.gameObject);
        }
    }
    
    public void Attack()
	{
		freezeTime = MAX_FREEZE_TIME;

		if(playersInsideHitArea.Count == 0)
		{
			soundController.PlaySound("whoosh");
			AnimateMiss();
            SetupFist(transform.position, transform.position + transform.forward * (attackDistance));

			//AnimateCd();
			return;
		}

		float sqrMinDistance = Mathf.Infinity;
		GameObject nearestPlayer = null;
		foreach(var player in playersInsideHitArea)
		{
			float sqrDistance = (player.transform.position - transform.position).sqrMagnitude;
			if(sqrDistance < sqrMinDistance)
			{
				sqrMinDistance = sqrDistance;
				nearestPlayer = player;
			}
		}

		if(sqrMinDistance > sqrAttackDistance)
		{
			// miss sound
			soundController.PlaySound("whoosh");
			AnimateMiss();
            SetupFist(transform.position, transform.position + transform.forward * attackDistance);

			//AnimateCd();
			return;
		}

        SetupFist(transform.position, nearestPlayer.transform.position);

		//play toet sound
		soundController.PlaySound("punch Sound");
		AnimateHit ();
		//AnimateCd();

		if(nearestPlayer.tag == "Player")
        	nearestPlayer.GetComponent<PlayerAttack>().KnockedDown((nearestPlayer.transform.position - transform.position).normalized, this.gameObject);
		else if(nearestPlayer.tag == "Boundary")
		{
			playersInsideHitArea.Remove(nearestPlayer);
			//Destroy(nearestPlayer);
			nearestPlayer.SetActive(false);
		}
    }
    
    public float GetFreezeTime()
	{
		return freezeTime;
	}

	public void KnockedDown(Vector3 direction, GameObject from)
	{
		lastHitFrom = from;
		lastHitExpireTime = HIT_EXPIRE_TIME;
		rigidbody.AddForce(direction * forceMagnitude, ForceMode.Impulse);
	}

	public void AnimateHit() {
		GameObject tmp = Instantiate (punchEffect, transform.position, Quaternion.identity) as GameObject;
		Destroy(tmp, 1f);
	}

	void AnimateFist()
	{
		if(freezeTime > 0)
		{
			//print (fistOriPos + " " + fistTargetPos + " " + freezeTime);
			if(freezeTime > MAX_FREEZE_TIME * 0.5f)
			{
				fist.transform.localPosition = Vector3.Lerp(fistOriPos, fistTargetPos, 2 * (1 - (freezeTime / MAX_FREEZE_TIME)));
			}
			else
			{
				fist.transform.localPosition = Vector3.Lerp(fistOriPos, fistTargetPos, 2 * (freezeTime / MAX_FREEZE_TIME));
			}
		}
	}

    void SetupFist(Vector3 from, Vector3 to)
    {
        //fist.SetActive(true);
        Vector3 dir = (to - from).normalized;
        dir.y = 0;
        fistTargetPos = transform.InverseTransformPoint(to);
        fist.transform.localPosition = fistOriPos;
		fistTargetPos.x = 0;
		fistTargetPos.y = 0;
		fistTargetPos.z = Mathf.Max(fistTargetPos.z, fistOriPos.z);
    }

	public void AnimateMiss() {
		GameObject tmp =  Instantiate (missEffect, transform.position, Quaternion.identity) as GameObject;
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

	public int GetHitableCount()
	{
		return playersInsideHitArea.Count;
	}
}
