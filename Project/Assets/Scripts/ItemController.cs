using UnityEngine;
using System.Collections;

public class ItemController : ItemGenerator {
	ItemType itc;
	float existTime = 5f;
	// Use this for initialization
	void Start () {
		collider.isTrigger = true;
		StartCoroutine (BlinkToDestroy());
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

	IEnumerator BlinkToDestroy() {
		yield return new WaitForSeconds(existTime);
		StartCoroutine(Blink ());

	}

	IEnumerator Blink() {
		for(int i = 0; i < 5; i++) {
			gameObject.GetComponent<MeshRenderer>().enabled = false;
			yield return new WaitForSeconds(0.1f);
			gameObject.GetComponent<MeshRenderer>().enabled = true;
			yield return new WaitForSeconds(0.1f);
		}
		Destroy (gameObject);
	}
}
