using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAttack : MonoBehaviour {

	float freezeTime; // time until the player can attack
	const float MAX_FREEZE_TIME = 2f;
	float attackDistance = 2.5f;
	float forceMagnitude = 7f;
	List<GameObject> playersInsideHitArea;

	PlayerController playerController;

	void Awake()
	{
		playerController = GetComponent<PlayerController>();
		playersInsideHitArea = new List<GameObject>();
	}

	void Update()
	{
		if(freezeTime == 0)
		{
			if(playerController.GetIndex() == 1 && Input.GetKeyDown(KeyCode.C))
			{
				Attack ();
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

		if(playersInsideHitArea.Count == 0)
		{
			return;
		}

		float minDistance = Mathf.Infinity;
		GameObject nearestPlayer = null;
		foreach(var player in playersInsideHitArea)
		{
			if((player.transform.position - transform.position).sqrMagnitude < minDistance)
			{
				nearestPlayer = player;
			}
		}

		nearestPlayer.rigidbody.AddForce(transform.forward * forceMagnitude, ForceMode.Impulse);
    }
    
    public float GetFreezeTime()
	{
		return freezeTime;
	}

}
