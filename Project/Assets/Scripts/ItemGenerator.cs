using UnityEngine;
using System.Collections;

public class ItemGenerator : MonoBehaviour {

	public GameObject itemPrefab;
	public Vector3 offset;
	public float interve;
	public int amount_max;

	GameObject[] tilearr;

	public enum ItemType {
		SpeedUp = 0,
		Freeze,
		NumberOfTypes
	};

	void Start() {
		tilearr = GameObject.FindGameObjectsWithTag ("Tile");
		StartCoroutine (Drop());
	}

	void FixedUpdate() {
		
	}

	void DropItem() {
		int amount = Random.Range (1,amount_max);
		for(int i =0 ; i < amount; i++) {
			int tilenum = Random.Range(0, tilearr.Length);
			GameObject item = Instantiate(itemPrefab, tilearr[tilenum].transform.position + offset, Quaternion.identity) as GameObject;
			item.AddComponent("ItemController");
			int it = Random.Range(0, (int)ItemType.NumberOfTypes);
			item.SendMessage("SetType", it);  // item type can send int directly
		}
	}

	IEnumerator Drop() {
		while(true) {
			float interve_imp = Random.Range(5f, 5+interve);
			DropItem();
			yield return new WaitForSeconds(interve_imp);
		}
	}
}
