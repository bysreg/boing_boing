using UnityEngine;
using System.Collections;

public class TileController : MonoBehaviour {

	public int boardWidth; // how many tiles for the board's width (x axis)
	public int boardHeight; // how many tiles for the board's height (z axis)
	public Transform tile;
	public BoxCollider boundary;

	GameObject tilesLayer;
	GameObject[,] tilesObj;
	float tileWidth;
	float tileDepth;
	float tileHeight;

	GameObject placeHolderTile; // tile used as reference for the others. located on the bottom left of the board

	float time;

	void Awake()
	{
		tilesLayer = GameObject.Find("Tiles");

		InitializeTiles();
		CreateBoundaryCollider();
	}

	void InitializeTiles()
	{
		tileWidth = tile.renderer.bounds.size.x;
		tileDepth = tile.renderer.bounds.size.z;
		tileHeight = tile.renderer.bounds.size.y;
		tilesObj = new GameObject[boardHeight, boardHeight];

		//deactive place holder tile not in tiles layer
		GameObject placeHolderTile = GameObject.Find("Tile");
		placeHolderTile.SetActive(false);

		for(int i=0; i<boardHeight; i++)
		{
			for(int j=0; j<boardWidth; j++)
			{
				Transform t = Instantiate(tile, new Vector3(j*tileWidth, placeHolderTile.transform.position.y, i*tileDepth), Quaternion.identity) as Transform;
				t.name = tile.name + (i*boardHeight + j);
				t.parent = tilesLayer.transform;
				tilesObj[i, j] = t.gameObject;
			}
		}
	}

	void CreateBoundaryCollider()
	{
		BoxCollider left = Instantiate(boundary) as BoxCollider;
		BoxCollider right = Instantiate(boundary) as BoxCollider;
		BoxCollider top = Instantiate(boundary) as BoxCollider;
		BoxCollider down = Instantiate(boundary) as BoxCollider;
		left.name = "left";
		right.name = "right";
		top.name = "top";
		down.name = "down";

		float midZ = (tilesObj[0, 0].transform.position.z + tilesObj[boardHeight - 1, 0].transform.position.z) / 2;
		float midX = (tilesObj[0, 0].transform.position.x + tilesObj[0, boardWidth - 1].transform.position.x) / 2;

		left.gameObject.transform.localScale = new Vector3(1, tileHeight + 3, boardHeight);
		right.gameObject.transform.localScale = new Vector3(1, tileHeight + 3, boardHeight);
		top.gameObject.transform.localScale = new Vector3(boardWidth, tileHeight + 3, 1);
		down.gameObject.transform.localScale = new Vector3(boardWidth, tileHeight + 3, 1);

		left.gameObject.transform.position = tilesObj[0, 0].transform.position + new Vector3(-tileWidth / 2 - boundary.size.x / 2, 0, midZ);
		right.gameObject.transform.position = tilesObj[0, boardWidth - 1].transform.position + new Vector3(tileWidth / 2 + boundary.size.x / 2, 0, midZ);
		top.gameObject.transform.position = new Vector3(midX, 0, tileDepth / 2 + boundary.size.z / 2) + tilesObj[boardHeight - 1, 0].transform.position;
		down.gameObject.transform.position = new Vector3(midX, 0, - tileDepth / 2 - boundary.size.z / 2) + tilesObj[0, 0].transform.position;

		GameObject parent = new GameObject();
		parent.name = "Bounds";
		left.transform.parent = parent.transform;
		right.transform.parent = parent.transform;
		top.transform.parent = parent.transform;
		down.transform.parent = parent.transform;
	}

	void SetTileHeight(TileMovement tile, float targetHeight)
	{
		tile.SetTileHeight(targetHeight);
	}

	void SetTileUpAndDown()
	{
		int count = 10;
		float height = -20f;

		for(int i=0; i<count; i++)
		{
			tilesObj[Random.Range(1, boardHeight - 1), Random.Range(1, boardWidth - 1)].GetComponent<TileMovement>().SetTileHeight(height);
		}
	}

	//return tile pos given a world position
	//if it is outside the tile, then this function will return wrong value
	public Vector2 GetTilePos(Vector3 worldPos)
	{
		int x = (int)((worldPos.x - tilesObj[0, 0].transform.position.x) / tileWidth);
		int y = (int)((worldPos.z - tilesObj[0, 0].transform.position.z) / tileDepth);

		return new Vector2(x, y);
	}

	public Vector3 GetWorldPos(int tileX, int tileY)
	{
		return tilesObj[tileY, tileX].transform.position;
	}

	void Update()
	{
		time += Time.deltaTime;

		//cheat
		if(Input.GetKeyDown(KeyCode.Space))
		{
			SetTileUpAndDown();
		}

		if(Input.GetKeyDown(KeyCode.B))
		{
			for(int i=0; i<boardHeight; i++)
			{
				for(int j=0; j<boardWidth; j++)
				{
					tilesObj[i, j].GetComponent<TileMovement>().SetTileHeightToNormal();
				}
			}
		}
	}
}
