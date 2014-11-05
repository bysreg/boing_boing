using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MoleMovement : MonoBehaviour {

	bool isMoving;
	Vector2 targetTilePos;
	Vector2[] path;

	TileController tileController;

	void Awake()
	{
		tileController = GameObject.Find("GameController").GetComponent<TileController>();
	}

	public void SetTargetMoveTilePos(int x, int y)
	{
		targetTilePos = new Vector2(x, y);
		isMoving = true;
		CreatePath();
	}

	void Update()
	{
		if(isMoving)
		{

		}
	}

	void CreatePath()
	{
		Vector2 currentPos = tileController.GetTilePos(transform.position);
		Queue<Vector2> queue = new Queue<Vector2>();
		bool[, ] visited = new bool[tileController.boardHeight, tileController.boardWidth]; 
		queue.Enqueue(currentPos);

		while(queue.Count > 0)
		{
			Vector2 node = queue.Dequeue();

			if(targetTilePos.x == node.x && targetTilePos.y == node.y)
			{

			}

			visited[(int)node.y, (int)node.x] = true;


		}
	}

}
