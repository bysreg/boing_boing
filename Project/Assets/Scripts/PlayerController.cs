﻿using UnityEngine;
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

	private GameObject orientationGameobject;

	// Use this for initialization
	protected override void Start ()
	{
		base.Start ();

		orientationGameobject = new GameObject ();
		orientationGameobject.name = "PSMoveOrientation " + psMoveIndex;

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
			/*
			 * roll  = Mathf.Atan2(2*y*w - 2*x*z, 1 - 2*y*y - 2*z*z);
			 * pitch = Mathf.Atan2(2*x*w - 2*y*z, 1 - 2*x*x - 2*z*z);
			 * yaw   = Mathf.Asin(2*x*y + 2*z*w);
			 */

			//float orientation = GetRotationValue (Mathf.Rad2Deg * Mathf.Atan2 (2 * moveData.QOrientation.y * moveData.QOrientation.w - 2 * moveData.QOrientation.x * moveData.QOrientation.z, 1 - 2 * moveData.QOrientation.y * moveData.QOrientation.y - 2 * moveData.QOrientation.z * moveData.QOrientation.z));
			//float playerRotation = playerFirstRotation - (orientation - psMoveFirstRotation);

			orientationGameobject.transform.localRotation = Quaternion.Euler(moveData.Orientation);

			Vector3 projectedVector = new Vector3 (orientationGameobject.transform.forward.x, 0f, orientationGameobject.transform.forward.z);
			Vector3 crossProduct = Vector3.Cross (Vector3.forward, projectedVector);
			
			float angle = Vector3.Angle (Vector3.forward, projectedVector);
			
			float playerRotation = psMoveFirstRotation - angle;
			if (crossProduct.y < 0) {
				playerRotation *= -1f;
			}

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
