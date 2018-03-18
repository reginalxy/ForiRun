using UnityEngine;
using System.Collections;

public class CameraMove : MonoBehaviour {

	private RunnerManager runnerManager;
	//private BorderManager bm;

	// Use this for initialization
	void Start () {
//		GameObject runner = GameObject.FindGameObjectWithTag (Tags.Runner);
//		Debug.Log ("=" + runner);
		runnerManager = GameObject.FindGameObjectWithTag (Tags.Runner).GetComponent<RunnerManager> ();
		//Debug.Log ("=" + runnerManager);

	}
	
	// Update is called once per frame
	void Update () {
		if (GameManager.onPlayersTurn) {
			transform.Translate (runnerManager.deltaPosX, 0f, 0f);
		}
	}
}
