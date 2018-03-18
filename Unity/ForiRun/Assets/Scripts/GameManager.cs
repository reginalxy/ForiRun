using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

	public static bool onPlayersTurn=false;
	public static int coinsCounter;

	public GameObject cv;
	public GameObject cv_main;
	public GameObject cv_pausing;
	public Text txtResume;
	public GameObject coinsText;
	public GameObject bestText;
	public GameObject pointTextOnGameOver;
	public GameObject coinsTextOnGameOver;
	public AudioClip acPause;
	public AudioClip acDong;
	public AudioClip acDing;
	//public AudioClip acOtherButtons;
	[HideInInspector]public GameObject runner;

	private SoundManager soundmanager;
	private bool isPausing;

	// Use this for initialization
	void Awake () {
		//Debug.Log ("awake");
		Application.targetFrameRate = 45;
		onPlayersTurn = true;
		isPausing = false;
		coinsCounter = 0;
		coinsText.GetComponent<Text> ().text = coinsCounter.ToString ();
		runner = GameObject.FindGameObjectWithTag (Tags.Runner);
		soundmanager = GameObject.Find ("SoundManager").GetComponent<SoundManager> ();
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnApplicationPause(bool pauseStatus) {
		if (pauseStatus && onPlayersTurn)
			pause ();
	}

	public void GameOver(){
		onPlayersTurn = false;
		soundmanager.musicSource.Stop ();
		Invoke ("gameover", 1);
		int point = runner.GetComponent<RunnerManager> ().pointCounter;
		pointTextOnGameOver.GetComponent<Text> ().text = point.ToString ();

		if (GlobalSetting.bestGrade < point)
			GlobalSetting.bestGrade = point;
		bestText.GetComponent<Text> ().text = GlobalSetting.bestGrade.ToString ();
		GlobalSetting.coinsCount += coinsCounter;
		coinsTextOnGameOver.GetComponent<Text> ().text = GlobalSetting.coinsCount.ToString ();
	}

	void gameover(){
		cv_main.gameObject.SetActive (false);
		cv.gameObject.SetActive (true);
	}

	public void UpdateCoinsCounter(){
		coinsCounter++;
		coinsText.GetComponent<Text> ().text = coinsCounter.ToString ();
	}

	public void pause(){
		soundmanager.PlaySingle (acPause);
		if (soundmanager.musicSource.isPlaying)
			soundmanager.musicSource.Pause ();
		else
			soundmanager.musicSource.Play ();
		
		isPausing = !isPausing;
		cv_pausing.gameObject.SetActive (isPausing);

		if (isPausing) {
			onPlayersTurn = !onPlayersTurn;
			runner.GetComponent<RunnerManager> ().Pause (isPausing);
			Debug.Log ("onPlayersTurn=" + onPlayersTurn);
		} 
		else {
			Debug.Log ("resume");
			txtResume.gameObject.SetActive (true);	//time=3
			soundmanager.PlaySingle (acDong);
			Invoke("deny",1);						//time=2
			Invoke("deny",2);						//time=1
			Invoke("deny",3);						//time=0
			Invoke("resume",3.15f);					//resume
		}
	}

	public void replay(){
		soundmanager.PlaySingle (acPause);
		SceneManager.LoadScene (1);
	}

	public void home(){
		soundmanager.PlaySingle (acPause);
		SceneManager.LoadScene (0);
	}

	void deny(){
		soundmanager.PlaySingle (acDong);

		int i = Convert.ToInt32 (txtResume.text);
		i--;
		txtResume.text = i.ToString ();
		if(i==0)
			soundmanager.PlaySingle (acDing);
	}

	void resume(){
		//soundmanager.PlaySingle (acDing);
		//CancelInvoke ();
		//isPausing = !isPausing;
		onPlayersTurn = !onPlayersTurn;

		runner.GetComponent<RunnerManager> ().Pause (isPausing);

		txtResume.gameObject.SetActive (false);
		txtResume.text = "3";
		Debug.Log ("onPlayersTurn=" + onPlayersTurn);
	}
}