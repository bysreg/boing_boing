using UnityEngine;
using System.Collections;

public class PlayerController : CharacterBaseController {

	public GameObject handle,handle2;

	bool simulateWithKeyboard = false;

	//ps move
	MoveController moveController;
	Vector3 initialOrientation;
	public int psMoveIndex = 0;
	protected float psMoveFirstRotation = 180f;
	protected float psMoveRange = 45f;
	protected float maxRotationSpeed = 250f;
	protected Vector2 rotationRange, speedRange;

	// Use this for initialization
	protected override void Start () {
		base.Start ();

		rotationRange = new Vector2(psMoveFirstRotation + psMoveRange, psMoveFirstRotation - psMoveRange);
		speedRange = new Vector2(-maxRotationSpeed, maxRotationSpeed);

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
			// avoid Gimbal Lock
			float orientation = GetRotationValue(Mathf.Rad2Deg * Mathf.Atan2(2*moveData.QOrientation.y*moveData.QOrientation.w - 2*moveData.QOrientation.x*moveData.QOrientation.z, 1 - 2*moveData.QOrientation.y*moveData.QOrientation.y - 2*moveData.QOrientation.z*moveData.QOrientation.z));
			float _rotationSpeed = GetValue(speedRange, rotationRange, orientation);
			if (_rotationSpeed < speedRange.x) 
				_rotationSpeed = speedRange.x;
			else if (_rotationSpeed > speedRange.y)
				_rotationSpeed = speedRange.y;

			Debug.Log("orientation : " + orientation + "; _rotationSpeed : " + _rotationSpeed);

			transform.Rotate(new Vector3(0f, _rotationSpeed * Time.deltaTime, 0f));

			if (handle != null) {
				/*
				 * roll  = Mathf.Atan2(2*y*w - 2*x*z, 1 - 2*y*y - 2*z*z);
				 * pitch = Mathf.Atan2(2*x*w - 2*y*z, 1 - 2*x*x - 2*z*z);
 				 * yaw   = Mathf.Asin(2*x*y + 2*z*w);
				 */

				float x = moveData.QOrientation.x;
				float y = moveData.QOrientation.y;
				float z = moveData.QOrientation.z;
				float w = moveData.QOrientation.w;

				handle.transform.localRotation = Quaternion.Euler(moveData.Orientation);
				handle2.transform.localRotation = Quaternion.Euler(270f, Mathf.Rad2Deg * Mathf.Atan2(2*y*w - 2*x*z, 1 - 2*y*y - 2*z*z), 0f);
			}
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
