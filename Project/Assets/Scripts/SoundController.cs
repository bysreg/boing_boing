using UnityEngine;
using System.Collections;

public class SoundController : MonoBehaviour {

	public GameObject soundfx;
	public bool mute;

	// Use this for initialization
	void Start () {
		if(Application.loadedLevel == 1) {
			PlaySound ("Bgm_new",0.2f, true);
		}else if(Application.loadedLevel == 0) {
			PlaySound ("select_bgm",0.5f, true);
		}
		//PlaySound ("dig",1f	, true);
	}
	
	// Update is called once per frame
	void Update () {
		StartCoroutine (CleanUp());
	}

	public GameObject PlaySound(string s, float vl = 1f, bool lop = false) { //value vloume
		if (mute) {
			return null;
				}
		GameObject soundOutput = Instantiate (soundfx) as GameObject;
		soundOutput.transform.parent = GameObject.Find ("SoundSets").transform;
		AudioSource audioS = GetAS (soundOutput);
		audioS.clip = Load(s);
		audioS.volume = vl;
		audioS.loop = lop;
		GetAS (soundOutput).Play ();
		return soundOutput;
	}

	AudioClip Load(string s) {
		string path = "SoundTrack/" + s;
		if (!Resources.Load (path)) {
			string path_new = "SoundTrack/" + s + "/" + s + Random.Range (0, 6);
			return Resources.Load (path_new) as AudioClip;
		} else {
			return Resources.Load ("SoundTrack/" + s) as AudioClip;
		}
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
