using UnityEngine;
using System.Collections;

public class CharacterBaseController : MonoBehaviour {

	// AI
	public bool isComputer = false;
	public int index; //player's number

	// floating algorithm
	protected Vector3 firstPosition;
	protected float floatingSpeed = 6f;
	protected float maxHeight = 0.75f;
	
	protected float rotationSpeed = 3f;
	protected float forwardSpeed = 45f;

	// parabolic
	protected float g = 15f;
	protected bool isParabolicAnimating = false;
	protected float elapsedTimeParabolic = 0f;
	protected float v0default = 6f;
	protected float v0;

	protected virtual void Awake() {
		if (isComputer) {
			gameObject.AddComponent<AIController>();
			Destroy(gameObject.GetComponent<PlayerController>());
		}
	}
	
	// Use this for initialization
	protected virtual void Start () {
		firstPosition = transform.position;

		Jump ();
	}

	// Update is called once per frame
	protected virtual void Update () {
		// idle floating
		Floating ();
	}

	protected virtual void FixedUpdate() {

	}
	
	protected void Floating() {
		//transform.position = new Vector3 (transform.position.x, firstPosition.y + (((Mathf.Sin (Time.time * floatingSpeed) + 1) / 2f) * maxHeight), transform.position.z);
		//transform.position = new Vector3 (transform.position.x, firstPosition.y + (Mathf.Abs (Mathf.Sin (Time.time * floatingSpeed)) * maxHeight), transform.position.z);

		if (isParabolicAnimating) {
			elapsedTimeParabolic += Time.deltaTime;
			float y = (v0 * elapsedTimeParabolic) - (0.5f * g * Mathf.Pow(elapsedTimeParabolic, 2));

			if (y <= 0f) {
				y = 0;

				isParabolicAnimating = false;
				 
				CheckGroundBelow();

				// next jump
				NextJump();
			}

			transform.position = new Vector3 (transform.position.x, firstPosition.y + y, transform.position.z);
		}
	}

	void CheckGroundBelow()
	{
		//TODO :
    }
    
	protected void NextJump () {
		float speed = Vector2.SqrMagnitude (new Vector2 (rigidbody.velocity.x, rigidbody.velocity.z));
		Vector2 speedRange = new Vector2 (v0default, v0default / 2f);
		float v0 = CharacterBaseController.GetValue (speedRange, new Vector2(0f, 50f), speed);
		if (v0 < speedRange.y) {
			v0 = speedRange.y;
		} else if (v0 > speedRange.x) {
			v0 = speedRange.x;
		}
		
		Jump (v0);
	}

	protected void Jump() {
		Jump (v0default);
	}

	protected void Jump(float v0) {
		this.v0 = v0;
		isParabolicAnimating = true;
		elapsedTimeParabolic = 0f;
	}

	protected void MoveForward() {
		rigidbody.AddForce (transform.forward * forwardSpeed);
		//if (transform.position.y <= 1.1f) {
		//	rigidbody.AddForce (transform.up * 5.5f * forwardSpeed);
		//}
	}

	public static float GetValue(Vector2 firstVector, Vector2 secondVector, float secondValue) {
		return -((firstVector.y - firstVector.x) * (secondVector.y - secondValue) / (secondVector.y - secondVector.x)) + firstVector.y;
	}
}
