using UnityEngine;
using System.Collections;

public class SoundController : MonoBehaviour {

	public GameObject soundfx;
	public bool mute;

	// Use this for initialization
	void Start () {
		PlaySound ("Bgm_new",1f	, true);
		//PlaySound ("dig",1f	, true);
	}
	
	// Update is called once per frame
	void Update () {
		StartCoroutine (CleanUp());
	}

	public void PlaySound(string s, float vl = 1f, bool lop = false) { //value vloume
		if (mute) {
			return;
				}
		GameObject soundOutput = Instantiate (soundfx) as GameObject;
		soundOutput.transform.parent = GameObject.Find ("SoundSets").transform;
		AudioSource audioS = GetAS (soundOutput);
		audioS.clip = Load(s);
		audioS.volume = vl;
		audioS.loop = lop;
		GetAS (soundOutput).Play ();
	}

	AudioClip Load(string s) {
		if(!Resources.Load ("SoundTrack/" + s)) {
			return Resources.Load ("SoundTrack/" + s + Random.Range(0,6)) as AudioClip;
		}
		return Resources.Load ("SoundTrack/" + s) as AudioClip;
	}

	AudioSource GetAS(GameObject go) {
		return go.GetComponent<AudioSource> ();
	}

	IEnumerator CleanUp() {
		yield return new WaitForSeconds (1f);
		foreach(Transform tf in GameObject.Find("SoundSets").transform) {
			if(!tf.gameObject.audio.isPlaying) {
				Destroy(tf.gameObject,1f);
			}
		}
	}
}
