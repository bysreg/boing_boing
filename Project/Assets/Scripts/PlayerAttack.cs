using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAttack : MonoBehaviour {

	//static
	public static GameObject playerPullingMole;
	//public GameObject fist;
	public GameObject punchEffect;

	bool psMoveAvailable;
	float freezeTime; // time until the player can attack
	const float MAX_FREEZE_TIME = 2f;
	float forceMagnitude = 14f;
	List<GameObject> playersInsideHitArea;
	float sqrAttackDistance;
	int index;

	//pulling mole
	bool isPulling;
	GameObject mole;
	float sqrMoleCatchingDistance = 4.5f;
	float pullMoleTime;
	float MAX_PULL_MOLE_TIME = 5f;

	PlayerController playerController;
	CharacterBaseController characterBaseController;
	GameController gameController;
	MoleController moleController;
	SoundController soundController;
    
	bool initialized;

	void Awake()
	{
		playerController = GetComponent<PlayerController>();
		playersInsideHitArea = new List<GameObject>();
		index = GetComponent<CharacterBaseController>().index;
		mole = GameObject.Find("Mole");
		gameController = GameObject.Find("GameController").GetComponent<GameController>();
		moleController = gameController.gameObject.GetComponent<MoleController>();
		sqrAttackDistance = GetComponent<BoxCollider>().size.z * GetComponent<BoxCollider>().size.z;
		soundController = gameController.gameObject.GetComponent<SoundController>();

		psMoveAvailable = PSMoveInput.IsConnected && PSMoveInput.MoveControllers[index-1].Connected;
	}

	void Update()
	{
//		if(index == 1)
//		{
//			print (transform.position.x + " " + transform.position.z);
//			print(Mathf.Pow(mole.transform.position.x - transform.position.x, 2) + Mathf.Pow(mole.transform.position.z - transform.position.z, 2));
//		}

		if(!initialized)
		{
			characterBaseController = GetComponent<CharacterBaseController>();
			initialized = true;
		}

		if(isPulling)
		{
			pullMoleTime -= Time.deltaTime;
			if(pullMoleTime <= 0)
			{
				pullMoleTime = 0;
				isPulling = false;
				characterBaseController.StopPullMoleFreeze();
				//this player wins the game
				gameController.FinishGame(index);
			}
			return;
		}

		if(freezeTime == 0)
		{
			if(playerController != null)
			{
				if((index == 1 && Input.GetKeyDown(KeyCode.W)) ||
					(index == 2 && Input.GetKeyDown(KeyCode.A)) ||
					(index == 3 && Input.GetKeyDown(KeyCode.S)) ||
					(index == 4 && Input.GetKeyDown(KeyCode.D)) ||

				   	(psMoveAvailable && PSMoveInput.MoveControllers[index].Data.ValueT > 0))
				{
					float sqrDistance = Mathf.Pow(mole.transform.position.x - transform.position.x, 2) + Mathf.Pow(mole.transform.position.z - transform.position.z, 2);
					
					if(playerPullingMole == null && sqrDistance <= sqrMoleCatchingDistance)
					{
						PullMole();
					}
					else
					{
						Attack();
					}
				}
			}
		}
		else
		{
			freezeTime -= Time.deltaTime;
			if(freezeTime <= 0)
			{
				freezeTime = 0;
			}
		}
	}

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == "Player")
		{
			playersInsideHitArea.Add(other.gameObject);
		}
	}

	void OnTriggerExit(Collider other)
	{
		if(other.tag == "Player")
		{
			playersInsideHitArea.Remove(other.gameObject);
		}
	}

	void Attack()
	{
		freezeTime = MAX_FREEZE_TIME;

		soundController.PlaySound("whoosh");

		if(playersInsideHitArea.Count == 0)
		{
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
			return;
		}

		//play toet sound
		soundController.PlaySound("punch Sound");
		AnimateFist ();
        nearestPlayer.GetComponent<PlayerAttack>().KnockedDown((nearestPlayer.transform.position - transform.position).normalized);
    }

	void PullMole()
	{
		if(!isPulling)
		{
			pullMoleTime = MAX_PULL_MOLE_TIME;
			isPulling = true;
			characterBaseController.PullMoleFreeze();
			playerPullingMole = this.gameObject;
		}
	}
    
    public float GetFreezeTime()
	{
		return freezeTime;
	}

	public bool IsPulling()
	{
		return isPulling;
	}
	
	public float GetPullMoleTime()
	{
		return pullMoleTime;
	}

	public void KnockedDown(Vector2 direction)
	{
		//disrupt if the player is pulling mole
		if(isPulling)
		{
			playerPullingMole = null;
			characterBaseController.StopPullMoleFreeze();
			isPulling = false;
			pullMoleTime = 0f;

			//mole starts to run around
			moleController.MoleRunAround();
		}

		rigidbody.AddForce(direction * forceMagnitude, ForceMode.Impulse);
	}

	public void AnimateFist() {
		Instantiate (punchEffect, transform.position, Quaternion.identity);
	}
}
