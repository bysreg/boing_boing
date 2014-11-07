using UnityEngine;
using System.Collections;

public class AIController : CharacterBaseController {

	private float minDistance = 0.7f;

	protected Vector3 targetPosition;

	// Use this for initialization
	protected override void Start () {
		base.Start ();

		// get first target point
		SetTargetPoint ();
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update ();

		if (GetDistance () <= minDistance) { // find the target point
			// set new target point
			SetTargetPoint ();
		} else { // go to target point
			// set rotation
			GoToTargetPosition ();

			// move forward
			elapsedTimeForward += Time.deltaTime;
			if (elapsedTimeForward >= waitingTimeToMove) {
				elapsedTimeForward -= waitingTimeToMove;

				MoveForward();
			}
		}
	}

	protected void SetTargetPoint() {
		targetPosition = new Vector3 (Random.Range(0f, 9f), firstPosition.y, Random.Range(0f, 9f));
	}

	protected float GetDistance() {
		return Vector2.Distance (new Vector2 (transform.position.x, transform.position.z), new Vector2 (targetPosition.x, targetPosition.z));
	}

	protected void GoToTargetPosition() {
		//find the vector pointing from our position to the target
		Vector3 direction = (targetPosition - transform.position).normalized;
		
		//create the rotation we need to be in to look at the target
		Quaternion lookRotation = Quaternion.LookRotation(direction);
		lookRotation = Quaternion.Euler (0f, lookRotation.eulerAngles.y, 0f);
		
		//rotate us over time according to speed until we are in the required rotation
		transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
	}
}
