using UnityEngine;
using System.Collections;

public class ItemController : ItemGenerator {
	ItemType itc;
	// Use this for initialization
	void Start () {
		collider.isTrigger = true;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate (transform.up);
	}

	void SetType(ItemType it) {
		itc = it;
	}

	void OnTriggerEnter(Collider c) {
		switch(itc) {
		case ItemType.Freeze:
			Freeze();
			break;
		case ItemType.SpeedUp:
			SpeedUp(c);
			break;
		}
	}

	void Freeze(){
		print ("Freeze all");
		Destroy (gameObject, 0f);
	}

	void SpeedUp(Collider c){
		print ("speedup name" + c.name);
		Destroy (gameObject, 0f);
	}
}
