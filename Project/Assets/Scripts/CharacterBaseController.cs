using UnityEngine;
using System.Collections;

public class CharacterBaseController : MonoBehaviour {

	// Character Selection
	public bool isCharacterSelection = false;
	protected float a = 0.3f;
	protected float velocity0 = 0f;
	protected float elapsedTimeSelection = 0f;

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

	protected bool isFreezeMovement;

	bool isFlying;
	float flyTime;
	const float MAX_FLY_TIME = 10f;

	GameController gameController;
	SoundController soundController;
	TileController tileController;
	PlayerAttack playerAttack;

	float yTime;

	//Bomb
	public bool hasBomb = false;
	public GameObject bomb;
	public Vector3 bombOffset;
	public GameObject bombFrom;

	private int bombTime = 5;
	private GameObject bombinst;
	private float bombPassInterve = 2f;
	private float bombGetTimeRecord= 0f;

	public GameObject explosion;
	private GameObject sf;

	protected virtual void Awake() {
		if (isComputer) {
			AIController aicomp = gameObject.AddComponent<AIController>();
			PlayerController playercomp = gameObject.GetComponent<PlayerController>();

			aicomp.index = playercomp.index;

			Destroy(playercomp);
		}

		gameController = GameObject.Find("GameController").GetComponent<GameController>();
		capsuleCollRadius = new Vector3(GetComponent<CapsuleCollider>().bounds.extents.x, 0, 0);
		playerAttack = gameObject.GetComponent<PlayerAttack>();
		soundController = GameObject.Find("GameController").GetComponent<SoundController>();

		if (!isCharacterSelection) {
			tileController = gameController.gameObject.GetComponent<TileController>();
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
		if(!fallDown)
			Floating ();

		if(isFreezeMovement)
			return;

		if(fallDown)
		{
			respawnTime -= Time.deltaTime;

			if(respawnTime <= 0)
			{
				gameController.SpawnPlayer(index);
			}
		}

		if(isFlying)
		{
			flyTime -= Time.deltaTime;
			if(flyTime <= 0)
			{
				flyTime = 0;
				StopFlying();
			}
		}
	}

	protected virtual void FixedUpdate() {

	}
	
	protected void Floating() {
		//transform.position = new Vector3 (transform.position.x, firstPosition.y + (((Mathf.Sin (Time.time * floatingSpeed) + 1) / 2f) * maxHeight), transform.position.z);
		//transform.position = new Vector3 (transform.position.x, firstPosition.y + (Mathf.Abs (Mathf.Sin (Time.time * floatingSpeed)) * maxHeight), transform.position.z);

		if (isCharacterSelection) {
			elapsedTimeSelection += Time.deltaTime;
			velocity0 -= a * elapsedTimeSelection;

			if (velocity0 <= 0f) {
				velocity0 = 0f;
				elapsedTimeSelection = 0f;
			}
		}

		if (isParabolicAnimating) {
			elapsedTimeParabolic += Time.deltaTime;
			//float y = (v0 * elapsedTimeParabolic) - (0.5f * g * Mathf.Pow(elapsedTimeParabolic, 2));

			Vector2 v = new Vector2(rigidbody.velocity.x, rigidbody.velocity.z);
			float vMagnitude;

			if (isCharacterSelection) {
				vMagnitude = velocity0;
			} else {
				vMagnitude = v.sqrMagnitude;
			}

			//yTime += Time.deltaTime * Mathf.Clamp(v.sqrMagnitude, 6, 10);
			yTime += Time.deltaTime * GetValue(new Vector2(7,17), new Vector2(0,50), vMagnitude);
//			if (index == 1) {
//				Debug.Log("velocity : " + v.sqrMagnitude);
//			}

			if (yTime >= Mathf.PI) {
				yTime -= Mathf.PI;

				isParabolicAnimating = false;
				 
				if (!isCharacterSelection && !isFlying)
				{
					CheckGroundBelow();
				}

				// next jump
				NextJump();

				//play toet sound
				soundController.PlaySound("SFX-Jump", 0.1f, false);
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
			if(!Physics.Raycast(transform.position - capsuleCollRadius, - transform.up, 1f, layerMask) && !Physics.Raycast(transform.position + capsuleCollRadius, - transform.up, 1f, layerMask))
			{
				// there is no tile below, so player falls down
				fallDown = true;
				respawnTime = MAX_RESPAWN_TIME;

				playerAttack.Killed();
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
		if (isCharacterSelection) {
			velocity0 += 5f;
		} else {
			rigidbody.AddForce (transform.forward * forwardSpeed);
			//if (transform.position.y <= 1.1f) {
			//	rigidbody.AddForce (transform.up * 5.5f * forwardSpeed);
			//}
		}
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

	public void FreezeMovement()
	{
		rigidbody.isKinematic = true;
		isFreezeMovement = true;
	}

	public void ActivateFlying()
	{
		StartFlying();
		flyTime = MAX_FLY_TIME;
	}

	void StartFlying()
	{
		isFlying = true;
	}

	void StopFlying()
	{
		isFlying = false;
	}

	//bomb controller

	void PassBomb(int bombT ,GameObject passedBy) {
		bombTime = bombT;
		bombinst = Instantiate (bomb,transform.position + bombOffset, Quaternion.identity) as GameObject;
		bombinst.transform.parent = gameObject.transform;
		this.bombFrom = passedBy;
		StartCoroutine(CountDown(bombT));
	}

	GameObject Explode() {
		Vector3 explodeForce = new Vector3 (Random.Range(-2,3), Random.Range(4,10), Random.Range(-2,3));
		this.gameObject.rigidbody.AddForce (explodeForce * 900);
		GameObject explosioninst = Instantiate (explosion, gameObject.transform.position, Quaternion.identity) as GameObject;
		soundController.PlaySound ("explode");
		Destroy (explosioninst, 3f);
		print (bombFrom.name);
		return bombFrom;
	}

	public void AttachBomb(GameObject go) {
		this.hasBomb = true;
		PassBomb (bombTime, go);
	}

	IEnumerator CountDown(int bombT) {
		sf = soundController.PlaySound("clock_long");
		while(true) {
			if(bombT >= 0 && hasBomb){
				yield return new WaitForSeconds (1);
				print( bombT+ "remaining");
				bombT--;
				bombTime--;
			}else {
				if(hasBomb){
					Explode();
					//Destroy(sf);
					sf.GetComponent<AudioSource>().Stop();
					this.hasBomb = false;
				}
				Destroy(bombinst);
				bombTime = 5;
				break;
			}
		}
	}

	void OnTriggerEnter(Collider c) { // Trigger Enter of Collision Enter
		if(c.gameObject.tag == "Player" && hasBomb && !c.gameObject.GetComponent<CharacterBaseController>().hasBomb) {
			if(bombPassInterve + bombGetTimeRecord < Time.time) {
				bombGetTimeRecord = Time.time;
				c.gameObject.GetComponent<CharacterBaseController>().bombGetTimeRecord = Time.time;
				this.hasBomb = false;
				sf.GetComponent<AudioSource>().Stop();
				soundController.PlaySound("passbomb", 0.2f, false);
				c.gameObject.GetComponent<CharacterBaseController>().hasBomb = true;
				c.gameObject.GetComponent<CharacterBaseController>().PassBomb(bombTime, gameObject);
				Destroy(this.bombinst);
			}else {
				return;
			}
		}
	}
}
