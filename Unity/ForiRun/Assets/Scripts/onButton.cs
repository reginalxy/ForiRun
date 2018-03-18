using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class onButton : MonoBehaviour {

	public Sprite[] buttonPic = new Sprite[2];
	public AudioClip acClick;

	private SoundManager soundmanager;
	//private bool buttonOn;

	// Use this for initialization
	void Start () {
		//buttonOn = true;
		soundmanager = GameObject.Find ("SoundManager").GetComponent<SoundManager> ();
	}

	public void click(int index){
		Sprite temp = buttonPic [0];
		buttonPic [0] = buttonPic [1];
		buttonPic [1] = temp;

		gameObject.GetComponent<Image> ().sprite = buttonPic [0];

		soundmanager.SetSounds (index);
		soundmanager.PlaySingle (acClick);
	}
}
