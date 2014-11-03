using UnityEngine;
using System.Collections;

public class TileSetup : MonoBehaviour {

	public int width;
	public int height;
	public Transform tile;

	private float tileWidth;
	private float tileHeight;

	void Start()
	{
		ArrangeTile();
	}

	void ArrangeTile()
	{
		float tileWidth = tile.renderer.bounds.size.x;
		float tileDepth = tile.renderer.bounds.size.z;
		for(int i=0; i<height; i++)
		{
			for(int j=0; j<width; j++)
			{
				Transform t = Instantiate(tile, new Vector3(j*tileWidth, 0, i*tileDepth), Quaternion.identity) as Transform;
			}
		}
	}

}
