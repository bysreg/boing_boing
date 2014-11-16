using UnityEngine;
using System.Collections;

public class ItemController : ItemGenerator {
	ItemType itc;
	float existTime = 5f;

	void Start () {
		collider.isTrigger = true;
		StartCoroutine (BlinkToDestroy());
	}

	void Update () {
		transform.Rotate (transform.up);
	}

	void SetType(ItemType it) {
		itc = it;
	}

	void OnTriggerEnter(Collider c) {
		GameObject.Find ("GameController").GetComponent<SoundController>().PlaySound("itempickup",0.8f, false);
		if(c.tag == "Player" && !c.collider.isTrigger) {
			switch(itc) 
			{
			case ItemType.Wings:
				ActivateWings(c);
				break;
			case ItemType.SpeedUp:
				SpeedUp(c);
				break;
			case ItemType.Bombs:
				AttachBomb(c);
				break;
			}
		}
	}

	void ActivateWings(Collider c){
		//print ("activate wings on " + c.name);
		c.gameObject.GetComponent<CharacterBaseController>().ActivateFlying();
		Destroy (gameObject, 0f);
	}

	void AttachBomb(Collider c){
		if(!c.gameObject.GetComponent<CharacterBaseController>().hasBomb) {
			c.gameObject.GetComponent<CharacterBaseController>().AttachBomb(c.gameObject);
			Destroy (gameObject, 0f);
		}
	}

	void SpeedUp(Collider c){
		//print ("speedup name" + c.name);
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
