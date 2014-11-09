using UnityEngine;
using System.Collections;

public class HDebug : MonoBehaviour {

	Transform p1;
	Transform p2;
	
	GUIText freezeTimeText;
	GUIText pullMoleTimeText;
	GUIText freezeTimeP2;
	GUIText pullMoleTimeP2;

	PlayerAttack p1Attack;
	PlayerAttack p2Attack;

	void Awake()
	{
		freezeTimeText = transform.Find("FreezeTime").GetComponent<GUIText>();
		pullMoleTimeText = transform.Find("PullMoleTime").GetComponent<GUIText>();
		freezeTimeP2 = transform.Find("FreezeTimeP2").GetComponent<GUIText>();
		pullMoleTimeP2 = transform.Find("PullMoleTimeP2").GetComponent<GUIText>();

		p1 = GameObject.Find("P1").transform;
		p1Attack = p1.GetComponent<PlayerAttack>();
		p2 = GameObject.Find("P2").transform;
		p2Attack = p2.GetComponent<PlayerAttack>();
	}

	void Update()
	{
		freezeTimeText.text = "FreezeTime : " + p1Attack.GetFreezeTime();
		pullMoleTimeText.text = "PullMoleTime : " + p1Attack.GetPullMoleTime();
		freezeTimeP2.text = "P2 FreezeTime : " + p2Attack.GetFreezeTime();
		pullMoleTimeP2.text = "P2 PullMoleTime : " + p2Attack.GetPullMoleTime();
	}

}
