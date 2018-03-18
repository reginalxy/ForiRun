using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GuideManager : MonoBehaviour {

	public Canvas cvGuide;
	public Image imgGuide;
	public Sprite[] GuideImg=new Sprite[4];

	private bool isGuiding;
	//private BorderManager bordermanager;
	private RunnerManager runnermanager;
	private int GuideImgIndexToShow;

	// Use this for initialization
	void Start () {
		isGuiding = false;
		GuideImgIndexToShow = 0;
		runnermanager = gameObject.GetComponent<RunnerManager> ();
		//bordermanager = GameObject.FindGameObjectWithTag (Tags.MainCamera).GetComponent<BorderManager> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (isGuiding && other.tag==Tags.Plants){
			UnshowImg ();
		}
		if (isGuiding && other.tag == Tags.Barriers) {
			cvGuide.gameObject.SetActive (false);
			Destroy (this);
		}
	}

	public void Guide(){
		//cvGuide.gameObject.SetActive (true);
		isGuiding = true;
		Invoke ("ShowImg", 0.8f);
	}

	public void UnshowImg(){
		//Debug.Log ("click");
		cvGuide.gameObject.SetActive (false);

		if (GuideImgIndexToShow < 4)
			Invoke ("ShowImg", 0.5f);
	}

	void ShowImg(){
		imgGuide.GetComponent<Image> ().sprite = GuideImg [GuideImgIndexToShow];
		cvGuide.gameObject.SetActive (true);
		if (GuideImgIndexToShow == 3) {
			runnermanager.SetBuffActive (true);
			Invoke ("EndGuide", 2f);
		}
		GuideImgIndexToShow++;
	}

	void EndGuide(){
		GlobalSetting.firstPlay = false;
		cvGuide.gameObject.SetActive (false);
		Destroy (this);
	}
}
