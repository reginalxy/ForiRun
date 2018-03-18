using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class HomepageManager : MonoBehaviour {

	public AudioClip acButtons;
	public Button music;
	public Button efx;
	public Canvas cvSetting;

	private SoundManager soundmanager;

	// Use this for initialization
	void Start () {
		soundmanager = GameObject.Find ("SoundManager").GetComponent<SoundManager> ();

		if (!GlobalSetting.playBgm)
			music.GetComponent<onButton> ().click (0);
		if (!GlobalSetting.playEfx)
			efx.GetComponent<onButton> ().click (1);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void play(){
		soundmanager.PlaySingle (acButtons);
		SceneManager.LoadScene (1);
	}

	public void studio(){
		soundmanager.PlaySingle (acButtons);
		SceneManager.LoadScene (2);
	}

	public void setting(){
		soundmanager.PlaySingle (acButtons);
		cvSetting.gameObject.SetActive (!cvSetting.isActiveAndEnabled);
	}
}
