using UnityEngine;
using System.Collections;

public class PlayerController : CharacterBaseController {

	bool simulateWithKeyboard = false;

	//ps move
	MoveController moveController;
	Vector3 initialOrientation;
	public int psMoveIndex = 0;

	// Use this for initialization
	protected override void Start () {
		base.Start ();

		try {
			if(PSMoveInput.IsConnected && PSMoveInput.MoveControllers[psMoveIndex].Connected) {
				simulateWithKeyboard = false;
			} else {
				simulateWithKeyboard = true;
			}
		} catch {
			simulateWithKeyboard = true;
		}

		if (!simulateWithKeyboard) {
			moveController = PSMoveInput.MoveControllers[psMoveIndex];
		}
	}
	
	// Update is called once per frame
	protected override void Update () {
		base.Update ();

		// keyboard
		if (simulateWithKeyboard) {
			// forward
			if (Input.GetKeyDown(KeyCode.UpArrow) && index == 1) {
				MoveForward();
			}

			// steering
			if(index == 1)
			{
				float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
				transform.Rotate(0, rotation, 0);
			}
		} else { // ps move
			// forward
			float value = moveController.Data.Velocity.y;
			MoveData moveData = moveController.Data;

			// move forward
			elapsedTimeForward += Time.deltaTime;

			if (Mathf.Abs(value) >= 7f && elapsedTimeForward >= waitingTimeToMove) {
				elapsedTimeForward -= 0f;
				MoveForward();
			}

			// steering
			transform.localRotation = Quaternion.Euler(Vector3.zero);
			transform.Rotate(new Vector3(0f,moveData.Orientation.z,0f));

			Quaternion temp = new Quaternion(0,0,0,0);
			temp = moveData.QOrientation;
			temp.x = -moveData.QOrientation.x;
			temp.y = -moveData.QOrientation.y;
			transform.localRotation = temp;

			//transform.rotation = Quaternion.Euler(transform.rotation.eulerAngles.x, moveController.Data.Orientation.z, transform.rotation.eulerAngles.z);
		}
	}

	protected float GetRotationValue(float value) {
		if (value < 0) {
			float result = value % 360;
			return result + 360;
		} else {
			return value % 360f;
		}
	}
}
