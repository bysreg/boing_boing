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
	protected float forwardSpeed = 60f;

	// parabolic
	protected float g = 15f;
	protected bool isParabolicAnimating = false;
	protected float elapsedTimeParabolic = 0f;
	protected float v0default = 6f;
	protected float v0;

	// forward
	protected float waitingTimeToMove = 0.2f;
	protected float elapsedTimeForward = 0f;

	bool fallDown;
	float respawnTime;
	const float MAX_RESPAWN_TIME = 3f;
	Vector3 capsuleCollRadius;

	GameController gameController;
	SoundController soundController;
	TileController tileController;

	float yTime;

	protected virtual void Awake() {
		if (isComputer) {
			AIController aicomp = gameObject.AddComponent<AIController>();
			PlayerController playercomp = gameObject.GetComponent<PlayerController>();

			aicomp.index = playercomp.index;

			Destroy(playercomp);
		}

		gameController = GameObject.Find("GameController").GetComponent<GameController>();
		soundController = gameController.gameObject.GetComponent<SoundController>();
		capsuleCollRadius = new Vector3(GetComponent<CapsuleCollider>().bounds.extents.x, 0, 0);
		tileController = gameController.gameObject.GetComponent<TileController>();
	}
	
	// Use this for initialization
	protected virtual void Start () {
		firstPosition = transform.position;

		Jump ();
	}

	// Update is called once per frame
	protected virtual void Update () {
		// idle floating
		if(!fallDown)
			Floating ();

		if(fallDown)
		{
			respawnTime -= Time.deltaTime;

			if(respawnTime <= 0)
			{
				gameController.SpawnPlayer(index);
			}
		}
	}

	protected virtual void FixedUpdate() {

	}
	
	protected void Floating() {
		//transform.position = new Vector3 (transform.position.x, firstPosition.y + (((Mathf.Sin (Time.time * floatingSpeed) + 1) / 2f) * maxHeight), transform.position.z);
		//transform.position = new Vector3 (transform.position.x, firstPosition.y + (Mathf.Abs (Mathf.Sin (Time.time * floatingSpeed)) * maxHeight), transform.position.z);

		if (isParabolicAnimating) {
			elapsedTimeParabolic += Time.deltaTime;
			//float y = (v0 * elapsedTimeParabolic) - (0.5f * g * Mathf.Pow(elapsedTimeParabolic, 2));

			Vector2 v = new Vector2(rigidbody.velocity.x, rigidbody.velocity.z);

			//yTime += Time.deltaTime * Mathf.Clamp(v.sqrMagnitude, 6, 10);
			yTime += Time.deltaTime * GetValue(new Vector2(7,17), new Vector2(0,50), v.sqrMagnitude);
//			if (index == 1) {
//				Debug.Log("velocity : " + v.sqrMagnitude);
//			}

			if (yTime >= Mathf.PI) {
				yTime -= Mathf.PI;

				isParabolicAnimating = false;
				 
				CheckGroundBelow();

				// next jump
				NextJump();

				//play toet sound
				soundController.PlaySound("SFX-Jump");
			}
			float y = Mathf.Abs(Mathf.Sin(yTime) * maxHeight);

			transform.position = new Vector3 (transform.position.x, firstPosition.y + y, transform.position.z);
		}
	}

	void CheckGroundBelow()
	{
		int layerMask = (1 << LayerMask.NameToLayer("Tile"));
		RaycastHit hitInfo;
		if(!Physics.Raycast(transform.position, - transform.up, out hitInfo, 2f, layerMask))
		{
			if(index == 1)
			{
				print ("nothing below");
			}
			if(!Physics.Raycast(transform.position - capsuleCollRadius, - transform.up, 1f, layerMask) && !Physics.Raycast(transform.position + capsuleCollRadius, - transform.up, 1f, layerMask))
			{
				// there is no tile below, so player falls down
				fallDown = true;
				respawnTime = MAX_RESPAWN_TIME;
			}
		}
		else
		{
			hitInfo.collider.gameObject.GetComponent<TileMovement>().ShakeTile();
		}
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

	public void Reset()
	{
		fallDown = false;
	}

	public int GetIndex()
	{
		return index;
	}

	public void PullMoleFreeze()
	{
		rigidbody.isKinematic = true;
	}

	public void StopPullMoleFreeze()
	{
		rigidbody.isKinematic = false;
	}
}
