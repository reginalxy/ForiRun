using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CloudsMove : MonoBehaviour {

	public float speadX;
	public float speadY;
	public float MaxX;
	public float MinX;
	public float MaxY;
	public float MinY;
//	public Vector3 instPos;
//	public Vector3 destPos;
//	public GameObject Clouds;
//
//	private List<GameObject> CloudsList;

	// Use this for initialization
	void Start () {
//		CloudsList = new List<GameObject> ();
//		GameObject clouds = (GameObject)Instantiate (Clouds, new Vector3 (0f, 0f), Quaternion.identity);
//		CloudsList.Add (clouds);
//		clouds = (GameObject)Instantiate (Clouds, new Vector3 (-23f, 0f), Quaternion.identity);
//		CloudsList.Add (clouds);
//
//		//golstCoulds = new List<GameObject> ();
//		//golstCoulds.Add (this.gameObject);
	}
	
	// Update is called once per frame
	void Update () {
		float randomSpead = Random.value;
		gameObject.transform.Translate (speadX * randomSpead * Time.deltaTime, 
										speadY * randomSpead * Time.deltaTime, 0f);
		if (gameObject.transform.position.x >= MaxX || gameObject.transform.position.x <= MinX) {
			speadX *= -1f;
		}
		if (gameObject.transform.position.y >= MaxY || gameObject.transform.position.y <= MinY) {
			speadY *= -1f;
		}
			
//		Debug.Log (CloudsList [0].transform.localPosition);
//		for (int i = 0; i < CloudsList.Count; i++) {
////			a += names[i];
//			CloudsList[i].transform.Translate(spead*Time.deltaTime, 0f, 0f);
//		}
//		if (CloudsList [0].transform.localPosition.x >= destPos.x) {
//			GameObject cloudsToRemove = CloudsList [0];
//			CloudsList.Remove (cloudsToRemove);
//			Destroy (cloudsToRemove);
//
//			GameObject clouds = (GameObject)Instantiate (Clouds, instPos, Quaternion.identity);
//			CloudsList.Add (clouds);
//		}
	}
}
