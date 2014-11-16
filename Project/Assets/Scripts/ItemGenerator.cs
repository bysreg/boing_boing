using UnityEngine;
using System.Collections;

public class ItemGenerator : MonoBehaviour {

	public GameObject[] itemPrefab;
	public Vector3 offset;
	public float interve;
	public int amount_max;
	public bool enabled;

	struct tilestruct{
		public bool has_item;
		public GameObject tile;
	};

	tilestruct[] tilearr;

	public enum ItemType {
		Fist,
		Wings,
		Bombs,
		NumberOfTypes
	};

	void Start() {
		//tilearr = GameObject.FindGameObjectsWithTag ("Tile");
		int i = 0;
		int length = GameObject.FindGameObjectsWithTag ("Tile").Length;
		tilearr = new tilestruct[length];
		foreach (GameObject go in GameObject.FindGameObjectsWithTag ("Tile")) {
			tilearr[i++].tile = go;
		}
		i = 0;
		StartCoroutine (Drop());
	}

	void FixedUpdate() {
		
	}

	void DropItem() {
		int amount = Random.Range (3,amount_max);

		if(!enabled)
		{
			return;
		}

		for(int i =0 ; i < amount; i++) {
			int it = Random.Range(0, (int)ItemType.NumberOfTypes);
			int tilenum = Random.Range(0, tilearr.Length);
			if(tilearr[i].has_item) {
				return;
			}
			GameObject item = Instantiate(itemPrefab[it], tilearr[tilenum].tile.transform.position + offset, Quaternion.identity) as GameObject;
			tilearr[i].has_item = true;
			iTween.MoveTo(item, tilearr[tilenum].tile.transform.position + new Vector3(0,1,0), 2);
			item.AddComponent("ItemController");
			item.SendMessage("SetType", it);  // item type can send int directly
		}
	}

	IEnumerator Drop() {
		while(true) {
			float interve_imp = Random.Range(5f, 5+interve);
			DropItem();
			yield return new WaitForSeconds(1f);
			Clean();
			yield return new WaitForSeconds(interve_imp + 5f); //item exist time
		}
	}

	public void SetEnabled(bool value)
	{
		this.enabled = value;
	}

	void Clean() 
	{
		for(int i = 0; i < tilearr.Length; i++) {
			tilearr[i].has_item = false;
		}
	}
}
