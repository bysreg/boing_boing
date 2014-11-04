using UnityEngine;
using System.Collections;

public class TileController : MonoBehaviour {

	public int boardWidth; // how many tiles for the board's width (x axis)
	public int boardHeight; // how many tiles for the board's height (z axis)
	public Transform tile;

	private GameObject tilesLayer;
	private GameObject[,] tilesObj;

	void Awake()
	{
		tilesLayer = GameObject.Find("Tiles");
	}

	void Start()
	{
		InitializeTiles();
	}

	void InitializeTiles()
	{
		float tileWidth = tile.renderer.bounds.size.x;
		float tileDepth = tile.renderer.bounds.size.z;
		tilesObj = new GameObject[boardHeight, boardHeight];

		for(int i=0; i<boardHeight; i++)
		{
			for(int j=0; j<boardWidth; j++)
			{
				Transform t = Instantiate(tile, new Vector3(j*tileWidth, 0, i*tileDepth), Quaternion.identity) as Transform;
				t.name = tile.name + (i*boardHeight + j);
				t.parent = tilesLayer.transform;
				tilesObj[i, j] = t.gameObject;
			}
		}

		//deactive place holder tile not in tiles layer
		GameObject placeHolderTile = GameObject.Find("Tile");
		placeHolderTile.SetActive(false);
	}

	void SetTileHeight(TileMovement tile, float targetHeight)
	{
		tile.SetTileHeight(targetHeight);
	}
}
