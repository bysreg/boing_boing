using UnityEngine;
using System.Collections;

public class PlayerController : CharacterBaseController
{

		bool simulateWithKeyboard = false;

		//ps move
		MoveController moveController;
		Vector3 initialOrientation;
		public int psMoveIndex = 0;
		protected float psMoveFirstRotation = 180f;
		protected float playerFirstRotation = 0f;
		protected float psMoveRange = 45f;
		protected float maxRotationSpeed = 250f;
		protected Vector2 rotationRange, speedRange;

		// Use this for initialization
		protected override void Start ()
		{
				base.Start ();

				rotationRange = new Vector2 (psMoveFirstRotation + psMoveRange, psMoveFirstRotation - psMoveRange);
				speedRange = new Vector2 (-maxRotationSpeed, maxRotationSpeed);

				try {
						if (PSMoveInput.IsConnected && PSMoveInput.MoveControllers [psMoveIndex].Connected) {
								simulateWithKeyboard = false;
						} else {
								simulateWithKeyboard = true;
						}
				} catch {
						simulateWithKeyboard = true;
				}

				if (!simulateWithKeyboard) {
						moveController = PSMoveInput.MoveControllers [psMoveIndex];
				}
		}
	
		// Update is called once per frame
		protected override void Update ()
		{
				base.Update ();

				// keyboard
				if (simulateWithKeyboard) {
						// forward
						if ((Input.GetKeyDown (KeyCode.UpArrow) && index == 1) || 
								(Input.GetKeyDown (KeyCode.Keypad8) && index == 2)) {
								MoveForward ();
						}

						// steering
						float rotation = 0f;
						if (index == 1) {
								rotation = Input.GetAxis ("Horizontal");
						} else if (index == 2) {
								rotation = Input.GetAxis ("HorizontalP2");
						}
						transform.Rotate (0, rotation * rotationSpeed, 0);
				} else { // ps move
						// FORWARD
						float velocity = moveController.Data.Velocity.y;
						//Debug.Log("velocity : " + velocity);
						MoveData moveData = moveController.Data;

						// move forward
						elapsedTimeForward += Time.deltaTime;

						if (Mathf.Abs (velocity) >= 3f && elapsedTimeForward >= waitingTimeToMove) {
								elapsedTimeForward = 0f;
								MoveForward ();
						}

						// STEERING
						// avoid Gimbal Lock
						float orientation = GetRotationValue (Mathf.Rad2Deg * Mathf.Atan2 (2 * moveData.QOrientation.y * moveData.QOrientation.w - 2 * moveData.QOrientation.x * moveData.QOrientation.z, 1 - 2 * moveData.QOrientation.y * moveData.QOrientation.y - 2 * moveData.QOrientation.z * moveData.QOrientation.z));
						//Debug.Log("orientation : " + orientation);

						/*
			float _rotationSpeed = GetValue(speedRange, rotationRange, orientation);
			if (Mathf.Abs(orientation - psMoveFirstRotation) <= 15f) {
				_rotationSpeed = 0f;
			} else {
				if (_rotationSpeed < speedRange.x) 
					_rotationSpeed = speedRange.x;
				else if (_rotationSpeed > speedRange.y)
					_rotationSpeed = speedRange.y;
			}
			transform.Rotate(new Vector3(0f, _rotationSpeed * Time.deltaTime, 0f));
			*/

						float playerRotation = playerFirstRotation - (orientation - psMoveFirstRotation);
						transform.localRotation = Quaternion.Euler (0, playerRotation, 0);
				}
		}

		protected float GetRotationValue (float value)
		{
				if (value < 0) {
						float result = value % 360;
						return result + 360;
				} else {
						return value % 360f;
				}
		}
}
