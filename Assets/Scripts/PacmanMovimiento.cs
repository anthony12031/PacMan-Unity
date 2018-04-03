using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacmanMovimiento : MonoBehaviour {

	public float velocidad;
	private Vector2 direccion = Vector2.zero;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		//verificar input y cambiar la direccion de pacman
		if (Input.GetKeyDown (KeyCode.UpArrow) ) {
			direccion = Vector2.up;
		}else if (Input.GetKeyDown (KeyCode.DownArrow) ){
			direccion = Vector2.down;
		}else if (Input.GetKeyDown (KeyCode.LeftArrow)){
			direccion = Vector2.left;
		}else if (Input.GetKeyDown (KeyCode.RightArrow)){
			direccion = Vector2.right;
		}

		//mover a pacman en la direccion actual
		//transform.position += (Vector3)(direccion*velocidad)*Time.deltaTime;

		GetComponent<Animator> ().SetFloat ("DirX", direccion.x);
		GetComponent<Animator> ().SetFloat ("DirY", direccion.y);
	}

	bool valid(Vector2 dir) {
		// Cast Line from 'next to Pac-Man' to 'Pac-Man'
		Vector2 pos = transform.position;
		RaycastHit2D hit = Physics2D.Linecast(pos + dir, pos);
		return (hit.collider == GetComponent<Collider2D>());
	}

	void FixedUpdate(){
		Vector2 newPos = (Vector2)transform.position + direccion*velocidad*Time.deltaTime;
		GetComponent<Rigidbody2D> ().MovePosition ((Vector2)newPos);
		//Vector2 p = Vector2.MoveTowards(transform.position, direccion, velocidad);
		//GetComponent<Rigidbody2D>().MovePosition(p);

	}



}
