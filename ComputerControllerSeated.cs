using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerControllerSeated : MonoBehaviour {
	public Vector3[] vertices = new Vector3[3];
	public Rigidbody2D rb2d;
	public float speed = 1;
	public Vector3 direction;
	public float xpos;
	public float ypos;
	public float delay = 10;
	public float endDelay = 8;
	public Vector3 pos;
	float end;
	public float angle;
	public float radius;
	public bool paused;

	// Use this for initialization
	void Start () {
		direction = Vector3.down;
		pos = transform.position;
		FieldOfViewSeated fov = GetComponent<FieldOfViewSeated> ();
		angle = fov.viewAngle;
		radius = fov.viewRadius;
		end = 0;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		FieldOfViewSeated fov = GetComponent<FieldOfViewSeated> ();
		if (Time.time % delay == 0 && Time.time > 1 && !paused) {
			angle = 45;
			radius = 1.75f;
			end = Time.time + endDelay;
			fov.viewAngle = angle;
			transform.position = new Vector3 (transform.position.x, transform.position.y + .02f, 0);
		} 
		if (fov.viewRadius < radius)
			fov.viewRadius += .025f;

		if (Time.time > end) {
			//Time.timeScale = 0;
			transform.position = pos;
			fov.viewAngle = 0;
			fov.viewRadius = 0;

		}

	}

	public Vector3 getDirection() {
		return direction;
	}

	public Vector2 getDirection2D() {
		return new Vector2 (direction.x, direction.y);
	}
}
