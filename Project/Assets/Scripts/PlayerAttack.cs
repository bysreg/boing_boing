using UnityEngine;
using System.Collections;

public class PlayerAttack : MonoBehaviour {

	float freezeTime; // time until the player can attack
	const float MAX_FREEZE_TIME = 2f;
	float attackDistance = 2.5f;
	float forceMagnitude = 7f;

	PlayerController playerController;

	void Awake()
	{
		playerController = GetComponent<PlayerController>();
	}

	void Update()
	{
		Debug.DrawRay(transform.position, transform.forward * attackDistance, Color.blue);

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

		//test code
//		if(Input.GetKeyDown(KeyCode.F) && playerController.GetIndex() == 1)
//		{
//			this.rigidbody.AddForceAtPosition(transform.forward * forceMagnitude, transform.position, ForceMode.Impulse);
//		}
	}

	void Attack()
	{
		freezeTime = MAX_FREEZE_TIME;
		
		int layerMask = (1 << LayerMask.NameToLayer("Player"));
		RaycastHit hit;
		if(Physics.Raycast(transform.position, transform.forward, out hit, attackDistance, layerMask))
		{
			print ("someone is hit by the attack : " + hit.transform.name);
			hit.rigidbody.AddForceAtPosition(transform.forward * forceMagnitude, hit.point, ForceMode.Impulse);
        }
    }
    
    public float GetFreezeTime()
	{
		return freezeTime;
	}

}
