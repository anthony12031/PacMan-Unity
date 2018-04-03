using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nodo : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D co) {
		if (co.name == "Pacman") {
			Destroy (gameObject);
			GameScore.puntaje += 1;
			co.gameObject.GetComponent<AudioSource> ().Play ();
		}
		}

}
