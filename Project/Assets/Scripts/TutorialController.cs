using UnityEngine;
using System.Collections;

public class TutorialController : MonoBehaviour {
	
	public Texture[] tutorialTextures;

	private float distanceTutorial = 0.6f;
	private float animationTime = 1f;
	private float showTime = 3f;
	private float practiceTime = 4f;

	private int currentIndex = 0;

	// Use this for initialization
	void Start () {
		InitTutorial ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space)) {
			Application.LoadLevel(Application.loadedLevel + 1);		
		}
	}

	void ShowTutorial() {
		iTween.MoveTo(this.gameObject, iTween.Hash(
			"position", transform.parent.transform.TransformPoint(new Vector3(transform.localPosition.x, 0f, transform.localPosition.z)),
			"time", animationTime,
			"easetype", iTween.EaseType.easeOutElastic,
			"oncomplete", "HideTutorial"
		));
	}

	void HideTutorial() {
		if (currentIndex < tutorialTextures.Length) {
			iTween.MoveTo (this.gameObject, iTween.Hash (
				"position", transform.parent.transform.TransformPoint (new Vector3 (transform.localPosition.x, -distanceTutorial, transform.localPosition.z)),
				"delay", showTime,
				"time", animationTime/2f,
				"easetype", iTween.EaseType.linear,
				"oncomplete", "NextTutorial"
			));
		} else {
			StartCoroutine("GoToNextLevel");
		}
	}

	void InitTutorial() {
		// init tutorial
		renderer.material.mainTexture = tutorialTextures [currentIndex];
		currentIndex++;
		transform.localPosition = new Vector3 (transform.localPosition.x, distanceTutorial, transform.localPosition.z);
	}

	void NextTutorial() {
		InitTutorial ();

		StartCoroutine("ShowNextTutorial", practiceTime);
	}

	IEnumerator ShowNextTutorial(float delay) {
		yield return new WaitForSeconds(delay);
		ShowTutorial ();
	}

	IEnumerator GoToNextLevel() {
		yield return new WaitForSeconds(showTime);
		Application.LoadLevel (Application.loadedLevel + 1);
	}
}
