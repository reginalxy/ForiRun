using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class RunnerManager : MonoBehaviour {

	public float mixSpead;
	public float maxSpead;
	public Vector2 jumpingForce;
	public GameObject pointText;
	public Slider sldGreen;
	public Slider sldBlue;
	public Slider sldYellow;
	public Button btGreen;
	public Button btBlue;
	public Button btYellow;
	public RuntimeAnimatorController[] controller = new RuntimeAnimatorController[3];
	public AudioClip acJump;
	public AudioClip acClimb;
	public AudioClip acPlants;
	public AudioClip acCoins;
	public AudioClip acBuff;
	public AudioClip acBroken;
	public AudioClip acDie;
	[HideInInspector]public float spead;
	[HideInInspector]public int pointCounter;
	[HideInInspector]public float deltaPosX;

	//private int intSpead;
	private bool isJumping;
	private bool isClimbing;
	private Rigidbody2D rd;
	private Animator animator;
	private GameManager gm;
	private List<GameObject> buffPic;
	private float blueBuffTime;
	private float greenBuffTime;
	private float yellowBuffTime;
	private List<Button> btBuff;
	private float pointSpead;
	private SoundManager soundmanager;
	private Vector2 first;
	private Vector2 second;
//	private float currentPosX;
//	private float tempPosX;
//	private float runningDis;

	// Use this for initialization
	void Start () {
		spead = mixSpead;
		pointCounter = 0;

		isJumping = false;
		isClimbing = false;

		buffPic = new List<GameObject> ();
		foreach (Transform child in transform)
			buffPic.Add (child.gameObject);
		greenBuffTime = 500f * (1 + GlobalSetting.protectorLevel / 2);
		blueBuffTime = 500f * (1 + GlobalSetting.protectorLevel / 2);
		yellowBuffTime = 500 * (1 + GlobalSetting.protectorLevel / 2);
		pointSpead = 2f;

		//Debug.Log ("prtctcount=" + GlobalSetting.protectorCount);
		btBuff=new List<Button>();
		if (CheckBuffCount (0))
			btBuff.Add (btGreen);
		if (CheckBuffCount (1))
			btBuff.Add (btBlue);
		if (CheckBuffCount (2))
			btBuff.Add (btYellow);
		//Debug.Log ("listcount=" + btBuff.Count);

		rd = GetComponent<Rigidbody2D> ();
		animator = GetComponent<Animator> ();
		animator.runtimeAnimatorController = controller [GlobalSetting.selectedRole];
		gm = GameObject.FindGameObjectWithTag (Tags.MainCamera).GetComponent<GameManager> ();
		soundmanager = GameObject.Find ("SoundManager").GetComponent<SoundManager> ();

		first=Vector2.zero;
		second=Vector2.zero;
	}
	
	// Update is called once per frame
	void Update () {
		if (GameManager.onPlayersTurn) {
//			tempPosX = transform.position.x;
//			deltaPosX = tempPosX - currentPosX;
//			currentPosX = tempPosX;
			//pointCounter += deltaPosX;
			deltaPosX = spead*Time.deltaTime;

			transform.Translate (deltaPosX, 0f, 0f);

//			if (Input.GetMouseButton (0)) {
//				Jump ();
//			}
		}
	}

	void FixedUpdate(){
		if (GameManager.onPlayersTurn) {
			pointCounter = Convert.ToInt32 ((gm.transform.position.x * pointSpead));
			pointText.GetComponent<Text> ().text = pointCounter.ToString ();
		}
		if (sldGreen.IsActive ())
			sldGreen.value = Mathf.MoveTowards (sldGreen.value, 0f, 2f);
		else if (sldBlue.IsActive ())
			sldBlue.value = Mathf.MoveTowards (sldBlue.value, 0f, 2f);
		else if (sldYellow.IsActive ())
			sldYellow.value = Mathf.MoveTowards (sldYellow.value, 0f, 2f);
	}

	void OnCollisionEnter2D(Collision2D coll) {
		if (isJumping == true && coll.gameObject.tag == Tags.Floor) {
			isJumping = false;
			animator.SetBool ("isJumping", isJumping);
		}
	}

	void OnGUI(){
//		Debug.Log ("first=" + first);
//		if (GameManager.onPlayersTurn) {
//			first = Event.current.mousePosition;
//
//			while (Input.GetMouseButtonDown (0))
//				second = Event.current.mousePosition;
//			Debug.Log ("first=" + first);
//			Debug.Log ("second=" + second);
//			if (first.y < second.y)
//				Jump ();
//		}
		if(GameManager.onPlayersTurn){
			if (Input.GetTouch (0).deltaPosition.y > 0)
				Jump ();
			else if (Input.GetTouch (0).deltaPosition.y < 0 && !isClimbing) {
				isClimbing = true;
				Climb ();
				Invoke ("endClimb", 0.3f);
			}
		}
	}

	void endClimb(){
		isClimbing = false;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == Tags.Barriers) {
			if (sldGreen.IsActive ()) {
				CancelInvoke ();
				EndGreenBuff ();
				soundmanager.PlaySingle (acBroken);
			} 
			else if (sldBlue.IsActive ()) {
				//continue;
			}
			else {
				animator.SetTrigger ("die");
				soundmanager.PlaySingle (acDie);
				GameObject.FindGameObjectWithTag (Tags.MainCamera).GetComponent<GameManager> ().GameOver ();
			}
		} 
		else if (other.tag == Tags.Coins) {
			gm.UpdateCoinsCounter ();
			pointCounter += 10;
			soundmanager.RandomizeSfx (acCoins);
		}
		else if (other.tag==Tags.Plants){
			//Debug.Log ("plants");
			SpeadUp ();
			if (other.name == "planst_01")
				GlobalSetting.greenCount++;
			else if (other.name == "planst_02")
				GlobalSetting.blueCount++;
			else if (other.name == "planst_03")
				GlobalSetting.yellowCount++;
			soundmanager.PlaySingle (acPlants);
		}
	}

	public void Jump(){
		if (GameManager.onPlayersTurn == true && isJumping == false) {
			//Debug.Log ("rd=" + rd);
			rd.AddForce (jumpingForce);
			//transform.Translate(0f,2f,0f);
			isJumping = true;
			animator.SetBool ("isJumping", isJumping);
			soundmanager.PlaySingle (acJump);
		}
	}

	public void Climb(){
		if (GameManager.onPlayersTurn == true && isJumping == false) {
			animator.SetTrigger ("isClimbing");
			soundmanager.PlaySingle (acClimb);

			if (buffPic [0].activeSelf)
				buffPic [0].GetComponent<Animator> ().SetTrigger ("isClimbing");
			//Debug.Log ("chilid=" + buffPic [0]);
			if (buffPic [1].activeSelf)
				buffPic [1].GetComponent<Animator> ().SetTrigger ("isClimbing");
		}
	}

	public void Pause(bool value){
		animator.SetBool ("isPausing", value);
	}

	void SpeadUp(){
		//Debug.Log ("spead=" + spead);
		if (spead < maxSpead) {
			spead *= 1.02f;
			if (spead > maxSpead)
				spead = maxSpead;
			//Debug.Log ("spead=" + spead);
		}
	}

	public void SetBuff(int index){
		soundmanager.PlaySingle (acBuff);
		switch (index) {
		case 0:
			GlobalSetting.protectorCount--;
			buffPic [index].SetActive (true);

			sldGreen.maxValue = greenBuffTime;
			sldGreen.value = greenBuffTime;
			sldGreen.gameObject.SetActive (true);

			Invoke ("EndGreenBuff", greenBuffTime / 100);

			for (int i = 0; i < btBuff.Count; i++)
				btBuff [i].gameObject.SetActive (false);
			//Debug.Log ("switch value=0");
			break;
		case 1:
			GlobalSetting.boostCount--;
			buffPic [index].SetActive (true);

			spead *= 3f;

			sldBlue.maxValue = blueBuffTime;
			sldBlue.value = blueBuffTime;
			sldBlue.gameObject.SetActive (true);

			Invoke ("EndBlueBuff", blueBuffTime / 100);

			for (int i = 0; i < btBuff.Count; i++)
				btBuff [i].gameObject.SetActive (false);
			break;
		case 2:
			GlobalSetting.doubleCount--;

			pointSpead *= 2f;

			sldYellow.maxValue = yellowBuffTime;
			sldYellow.value = yellowBuffTime;
			sldYellow.gameObject.SetActive (true);

			Invoke ("EndYellowBuff", yellowBuffTime / 100);

			for (int i = 0; i < btBuff.Count; i++)
				btBuff [i].gameObject.SetActive (false);
			break;
		}
	}

	public void SetBuffActive(bool activeState){
		//Debug.Log ("count=" + btBuff.Count);
		for (int i = 0; i < btBuff.Count; i++)
			btBuff [i].gameObject.SetActive (activeState);
	}

	void EndGreenBuff(){
		sldGreen.gameObject.SetActive (false);
		buffPic [0].SetActive (false);
		CheckBuffCount (0);
		for (int i = 0; i < btBuff.Count; i++)
			btBuff [i].gameObject.SetActive (true);
		//Debug.Log ("listcount=" + btBuff.Count);
	}

	void EndBlueBuff(){
		spead /= 3;
		Invoke ("endbluebuff", 2f);
	}

	void endbluebuff(){
		sldBlue.gameObject.SetActive (false);
		buffPic [1].SetActive (false);
		CheckBuffCount (1);
		for (int i = 0; i < btBuff.Count; i++)
			btBuff [i].gameObject.SetActive (true);
	}

	void EndYellowBuff(){
		pointSpead /= 2;
		sldYellow.gameObject.SetActive (false);
		CheckBuffCount (2);
		for (int i = 0; i < btBuff.Count; i++)
			btBuff [i].gameObject.SetActive (true);
	}

	bool CheckBuffCount(int index){
		switch (index) {
		case 0:
			//Debug.Log ("prtctCount=" + GlobalSetting.protectorCount);
			if (GlobalSetting.protectorCount == 0) {
				btGreen.gameObject.SetActive (false);
				btBuff.Remove (btGreen);
				return false;
			}
			break;
		case 1:
			if (GlobalSetting.boostCount == 0) {
				btBlue.gameObject.SetActive (false);
				btBuff.Remove (btBlue);
				return false;
			}
			break;
		case 2:
			if (GlobalSetting.doubleCount == 0) {
				btYellow.gameObject.SetActive (false);
				btBuff.Remove (btYellow);
				return false;
			}
			break;
		}
		return true;
	}
}
