using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	// floating algorithm
	protected Vector3 firstPosition;
	protected float floatingSpeed = 6f;
	protected float maxHeight = 0.4f;

	protected float rotationSpeed = 3f;
	protected float forwardSpeed = 2500f;

	bool simulateWithKeyboard = false;

	//ps move
	Vector3 initialOrientation;

	// Use this for initialization
	void Start () {
		firstPosition = transform.position;

		try
		{
			if(PSMoveInput.MoveControllers[0].Connected)
			{
				simulateWithKeyboard = false;
			}
			else
			{
				simulateWithKeyboard = true;
			}
		}
		catch
		{
			simulateWithKeyboard = true;
		}

		if (!simulateWithKeyboard)
		{
			CalibratePsMove();
		}
	}
	
	// Update is called once per frame
	void Update () {
		// idle floating
		Floating ();

		// steering
		float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
		transform.Rotate(0, rotation, 0);
	}

	void CalibratePsMove()
//	{
//		if(PSMoveInput.IsConnected && PSMoveInput.MoveControllers[0].Connected) 
//		{
//			
//			Vector3 gemPos, handlePos;
//			MoveData moveData = PSMoveInput.MoveControllers[0].Data;
//			float zOffset = 20;
//			gemPos = moveData.Position;
//			handlePos = moveData.HandlePosition;
//		
//			gemPos.z = -gemPos.z + zOffset;
//			handlePos.z = -handlePos.z + zOffset;
//			gem.transform.localPosition = gemPos;
//			handle.transform.localPosition = handlePos;
//			handle.transform.localRotation = Quaternion.LookRotation(gemPos - handlePos);
//			handle.transform.Rotate(new Vector3(0,0,moveData.Orientation.z));
//
//			/* using quaternion rotation directly
//			* the rotations on the x and y axes are inverted - i.e. left shows up as right, and right shows up as left. This code fixes this in case 
//			* the object you are using is facing away from the screen. Comment out this code if you do want an inversion along these axes
//			* 
//			* Add by Karthik Krishnamurthy*/
//
//			temp = moveData.QOrientation;
//			temp.x = -moveData.QOrientation.x;
//			temp.y = -moveData.QOrientation.y;
//			handle.transform.localRotation = temp;
//		}	
	}

	void FixedUpdate() {
		// acceleration
		if (Input.GetKeyDown(KeyCode.UpArrow)) {
			MoveForward();
		}
	}

	void Floating() {
		transform.position = new Vector3 (transform.position.x, firstPosition.y + (((Mathf.Sin (Time.time * floatingSpeed) + 1) / 2f) * maxHeight), transform.position.z);
	}

	void MoveForward() {
		rigidbody.AddForce (transform.forward * forwardSpeed);
	}
}
