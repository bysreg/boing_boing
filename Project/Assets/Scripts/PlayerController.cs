using UnityEngine;
using System.Collections;

public class PlayerController : CharacterBaseController {

	bool simulateWithKeyboard = false;

	//ps move
	Vector3 initialOrientation;

	// Use this for initialization
	protected override void Start () {
		base.Start ();

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
			//CalibratePsMove(); // todo : unfinished
		}
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update ();

		// steering
		float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
		transform.Rotate(0, rotation, 0);
	}

	void CalibratePsMove()
	{
		MoveController moveController = null;
		switch(index)
		{
		case 1:
			moveController = PSMoveInput.MoveControllers[0];
			break;
		case 2:
			moveController = PSMoveInput.MoveControllers[1];
			break;
		case 3:
			moveController = PSMoveInput.MoveControllers[2];
			break;
		case 4:
			moveController = PSMoveInput.MoveControllers[3];
			break;
		}


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

	protected override void FixedUpdate() {
		base.FixedUpdate ();

		// acceleration
		if (Input.GetKeyDown(KeyCode.UpArrow)) {
			MoveForward();
		}
	}
}
