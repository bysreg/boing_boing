using UnityEngine;
using System.Collections;

public class PlayerController : CharacterBaseController
{
		bool simulateWithKeyboard = false;
		public GameObject handle, handle2;

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

						//float playerRotation = playerFirstRotation - (orientation - psMoveFirstRotation);
						//transform.localRotation = Quaternion.Euler (0, playerRotation, 0);

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

								float roll = Mathf.Rad2Deg * Mathf.Atan2 (2 * y * w - 2 * x * z, 1 - 2 * y * y - 2 * z * z);
								float pitch = Mathf.Rad2Deg * Mathf.Atan2 (2 * x * w - 2 * y * z, 1 - 2 * x * x - 2 * z * z);
								float yaw = Mathf.Rad2Deg * Mathf.Asin (2 * x * y + 2 * z * w);

								handle.transform.localRotation = Quaternion.Euler (pitch, roll, yaw);
								handle2.transform.localRotation = Quaternion.Euler (moveData.Orientation);
								handle2.transform.localRotation = Quaternion.Euler (moveData.Orientation.x, moveData.Orientation.y, moveData.Orientation.z);

								Vector3 forwardVector = Vector3.forward;
								Vector3 projectedVector = new Vector3 (handle2.transform.forward.x, 0f, handle2.transform.forward.z);
								Vector3 crossProduct = Vector3.Cross (forwardVector, projectedVector);

								float angle = Vector3.Angle (projectedVector, forwardVector);
				
								Debug.Log (angle + "; " + crossProduct.ToString ());

								float playerRotation = psMoveFirstRotation - angle;
								if (crossProduct.y < 0)
										playerRotation *= -1f;
								transform.localRotation = Quaternion.Euler (0, playerRotation, 0);
						}
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
