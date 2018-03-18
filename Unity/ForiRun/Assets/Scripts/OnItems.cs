using UnityEngine;
using System.Collections;

public class OnItems : MonoBehaviour {

	private Animator animator;
	// Use this for initialization
	void Start () {
		animator = GetComponent<Animator> ();
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.tag == Tags.Runner) {
			animator.SetTrigger ("triggerIt");
		}
	}
}
