using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerControllerBully : MonoBehaviour {
	public Vector3[] vertices = new Vector3[2];
	public Rigidbody2D rb2d;
	public float speed = 1;
	public Vector3 direction;
	public float xpos;
	public float ypos;
	public Vector3 pos;
	// Use this for initialization
	void Start () {
		pos = new Vector3 (xpos, ypos, 0);
		rb2d = GetComponent <Rigidbody2D> ();
		vertices [0] = new Vector3 (-8f, -0.4f, 0f);
		vertices [1] = new Vector3 (8f, -0.4f, 0f);
		direction = Vector3.right;
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		move ();
	}

	public Vector3 getDirection() {
		return direction;
	}

	public Vector2 getDirection2D() {
		return new Vector2 (direction.x, direction.y);
	}

	void move() {
		xpos = transform.position.x;
		ypos = transform.position.y;
		if (transform.position.x < vertices [1].x && direction == Vector3.right) {
			direction = new Vector3 (1, 0, 0);
			xpos += speed / 50;
			pos = new Vector3 (xpos, ypos, 0);
			transform.position = pos;
		
		}
		if (transform.position.x > vertices [1].x) 
			direction = Vector3.left;
		if (transform.position.x < vertices [0].x)
			direction = Vector3.right;

		if (transform.position.x > vertices [0].x && direction == Vector3.left) {
			xpos -= speed / 50;
			pos = new Vector3 (xpos, ypos, 0);
			transform.position = pos;
		}
		


	}
}
