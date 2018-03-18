using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class BorderManager : MonoBehaviour {

	public int lengthToReaveal;	
	//skyline part
	public float sl_DistanceToNext;  				//distance of one skyline pic to next
	public float sl_DisToInstantiate;				//for how long runner moved should a new skyline pic to be inatantiated
	public GameObject SkylinePerfab;
	//floor part
	public float fl_DistanceToNext;  				//distance of one floor pic to next
	public float fl_DisToInstantiate;				//for how long runner moved should a new floor pic to be inatantiated
	public List<GameObject> FloorTiles = new List<GameObject>();
	public GameObject[] FirstFloorGroup = new GameObject[4];
	public GameObject[] GuidFloorGroup = new GameObject[4];
	//mountains part
	public float mt_DisToNext01;
	public float mt_DisToNext02;
	public GameObject Mountain01;
	public GameObject Mountain02;

	private RunnerManager runnerManager;
	private int revealItem;
	private GuideManager guidemanager;
	//skyline part
	private float sl_nextPos;
	private float sl_runnerMovDis;					// counter of how long runner has moved for skyline manager
	private Queue SkylineToReveal;
	//floor part
	private float fl_nextPos;
	private float fl_runnerMovDis;					// counter of how long runner has moved for floor manager
	private Queue FloorToReveal;
	//mountain part
	private float mt_nextPos01;
	private float mt_runnerMovDis01;
	private Queue MounToReveal01;
	private float mt_nextPos02;
	private float mt_runnerMovDis02;
	private Queue MounToReveal02;

	// Use this for initialization
	void Start () {

		runnerManager = GameObject.FindGameObjectWithTag (Tags.Runner).GetComponent<RunnerManager> ();
		guidemanager = GameObject.FindGameObjectWithTag(Tags.Runner).GetComponent<GuideManager> ();
		if (GlobalSetting.firstPlay) {
			FirstFloorGroup = GuidFloorGroup;
			runnerManager.SetBuffActive (false);
			guidemanager.Guide ();
		}
		
		//SKYLINE PART

		sl_runnerMovDis = 0f;

		SkylineToReveal = new Queue ();
		sl_nextPos = -sl_DistanceToNext;
		for (int i = 0; i < lengthToReaveal; i++) {
			GameObject skyline = (GameObject)Instantiate (SkylinePerfab, 
				                     new Vector3 (sl_nextPos, 0f, 8f), Quaternion.identity);
			SkylineToReveal.Enqueue (skyline);
			sl_nextPos += sl_DistanceToNext;
		}

		//FLOOR PART

		fl_runnerMovDis = 0f;

		FloorToReveal = new Queue ();
		fl_nextPos = -fl_DistanceToNext;
		for (int i = 0; i < lengthToReaveal; i++) {
			GameObject floor = (GameObject)Instantiate (FirstFloorGroup[i],new Vector3 (fl_nextPos, 0f), Quaternion.identity);
			FloorToReveal.Enqueue (floor);
			fl_nextPos += fl_DistanceToNext;
		}

		//MOUNTAIN PART

		mt_runnerMovDis01 = 0f;

		MounToReveal01 = new Queue ();
		mt_nextPos01 = -mt_DisToNext01;
		for (int i = 0; i < lengthToReaveal; i++) {
			GameObject mountain = (GameObject)Instantiate (Mountain01, new Vector3 (mt_nextPos01, 0f, 50f), Quaternion.identity);
			MounToReveal01.Enqueue (mountain);
			mt_nextPos01 += mt_DisToNext01;
		}

		mt_runnerMovDis02 = 0f;

		MounToReveal02 = new Queue ();
		mt_nextPos02 = -mt_DisToNext02;
		for (int i = 0; i < lengthToReaveal; i++) {
			GameObject mountain = (GameObject)Instantiate (Mountain02, new Vector3 (mt_nextPos02, 0f, 26f), Quaternion.identity);
			MounToReveal02.Enqueue (mountain);
			mt_nextPos02 += mt_DisToNext02;
		}
	}
	
	// Update is called once per frame
	void Update () {

		if (GameManager.onPlayersTurn) {

			//SKYLINE PART

			sl_runnerMovDis += runnerManager.deltaPosX;
			if (sl_runnerMovDis >= sl_DisToInstantiate) {
				sl_runnerMovDis = 0f;

				GameObject skyline = (GameObject)Instantiate (SkylinePerfab, 
					                    new Vector3 (sl_nextPos, 0f, 8f), Quaternion.identity);
				UpdateQueue (SkylineToReveal, skyline);
				sl_nextPos += sl_DistanceToNext;
			}

			//FLOOR PART

			fl_runnerMovDis += runnerManager.deltaPosX;
			if (fl_runnerMovDis >= fl_DisToInstantiate) {
				fl_runnerMovDis = 0f;

				GameObject floor = (GameObject)Instantiate (FloorTiles [Random.Range (0, FloorTiles.Count)],
					                  new Vector3 (fl_nextPos, 0f), Quaternion.identity);
				UpdateQueue (FloorToReveal, floor);
				fl_nextPos += fl_DistanceToNext;

				//revealItem = 0 for false, != 0 for true
				revealItem = Random.Range (0, 9);

				//to reveal item
				if (revealItem != 0)
					RevealItem (floor.transform);
			}

			//MOUNTAIN PART

			mt_runnerMovDis01 += runnerManager.deltaPosX;
			if (mt_runnerMovDis01 >= mt_DisToNext01) {
				mt_runnerMovDis01 = 0f;

				GameObject mountain = (GameObject)Instantiate (Mountain01, new Vector3 (mt_nextPos01, 0f, 50f), Quaternion.identity);
				UpdateQueue (MounToReveal01, mountain);
				mt_nextPos01 += mt_DisToNext01;
			}

			mt_runnerMovDis02 += runnerManager.deltaPosX;
			if (mt_runnerMovDis02 >= mt_DisToNext02) {
				mt_runnerMovDis02 = 0f;

				GameObject mountain = (GameObject)Instantiate (Mountain02, new Vector3 (mt_nextPos02, 0f, 26f), Quaternion.identity);
				UpdateQueue (MounToReveal02, mountain);
				mt_nextPos02 += mt_DisToNext02;
			}
		}
	}

	void UpdateQueue(Queue items,GameObject newItem){
		GameObject itemToDestroy = (GameObject)items.Dequeue ();
		Destroy (itemToDestroy);

		//GameObject newItem = (GameObject)Instantiate (SkylineTiles [Random.Range (0, SkylineTiles.Count)], 
							//new Vector3 (sl_nextPos, 0f, 8), Quaternion.identity);
		items.Enqueue (newItem);
	}

	void RevealItem(Transform parent){
		
		List<GameObject> items = new List<GameObject> ();
		foreach (Transform child in parent) {
			items.Add (child.gameObject);
		}
		//Debug.Log ("count=" + items.Count);
		items [Random.Range (0, items.Count)].SetActive (true);
	}
}
