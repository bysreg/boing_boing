using UnityEngine;
using System.Collections;

public class HDebug : MonoBehaviour {

	Transform p1;
	
	GUIText freezeTimeText;
	PlayerAttack p1Attack;

	void Awake()
	{
		freezeTimeText = transform.Find("FreezeTime").GetComponent<GUIText>();
		p1 = GameObject.Find("P1").transform;
		p1Attack = p1.GetComponent<PlayerAttack>();
	}

	void Update()
	{
		freezeTimeText.text = "FreezeTime : " + p1Attack.GetFreezeTime();
	}

}
