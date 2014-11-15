using UnityEngine;
using System.Collections;

public class ItemGenerator : MonoBehaviour {

	public GameObject[] itemPrefab;
	public Vector3 offset;
	public float interve;
	public int amount_max;

	GameObject[] tilearr;

	public enum ItemType {
		SpeedUp = 0,
		Wings,
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
			int it = Random.Range(0, (int)ItemType.NumberOfTypes);
			int tilenum = Random.Range(0, tilearr.Length);
			GameObject item = Instantiate(itemPrefab[it], tilearr[tilenum].transform.position + offset, Quaternion.identity) as GameObject;
			item.AddComponent("ItemController");
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
