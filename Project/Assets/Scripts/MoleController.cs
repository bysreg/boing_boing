using UnityEngine;
using System.Collections;

public class MoleController : MonoBehaviour {

	TileController tileController;
	MoleMovement moleMovement;

	void Awake()
	{
		tileController = GameObject.Find("GameController").GetComponent<TileController>();
		moleMovement = GameObject.Find("Mole").GetComponent<MoleMovement>();
	}

	void MoveToTilePos(int x, int y)
	{
		moleMovement.SetTargetMoveTilePos(x, y);
	}

}
