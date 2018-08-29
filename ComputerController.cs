using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComputerController : MonoBehaviour {
	public Vector3[] vertices = new Vector3[3];
	//public Rigidbody2D rb2d;
	public float speed = 1;
	public Vector3 direction;
	public float xpos;
	public float ypos;
	public Vector3 pos;
	public bool isAlerted;
	public GoalManager alertedStudent;
	float pause = 5;
	float count;
	bool stop;
	Vector3 directionToSet;
	public Animator anim;
	public PlayerController player;
	// Use this for initialization
	void Start () {
		pos = new Vector3 (xpos, ypos, 0);
		stop = false;
		isAlerted = false;
		count = 0;
		anim = GetComponent<Animator> ();
		player = GameObject.Find ("Player").GetComponent<PlayerController>();
		//rb2d = GetComponent <Rigidbody2D> ();
		vertices [0] = new Vector3 (-1.1800f, 0.5900f, 0f);
		vertices [1] = new Vector3 (1.26f, 0.5900f, 0f);
		vertices [2] = new Vector3 (1.26f, -1.44f, 0f);
		
	}
	
	// Update is called once per frame
	void Update () {
		int num = Random.Range (1, 1000000);
		if (!isAlerted)
			move (num);
		else
			AlertMove ();
	}


	public Vector3 getDirection() {
		return direction;
	}

	public Vector2 getDirection2D() {
		return new Vector2 (direction.x, direction.y);
	}
	void AlertMove() {
		alertedStudent = player.goal;
		if ((alertedStudent.transform.position.x + .25f) < transform.position.x) {
			transform.Translate (Vector3.left * (speed) * Time.deltaTime);
			directionToSet = Vector3.left;
			if (direction != directionToSet)
				direction = directionToSet;
		}else if ((alertedStudent.transform.position.x + .2f) > transform.position.x) {
			transform.Translate (Vector3.right * (speed) * Time.deltaTime);
			directionToSet = Vector3.left;
			if (direction != Vector3.right)
				direction = Vector3.right;
		}
		else if (player.transform.position.y + .11f < transform.position.y) {
			transform.Translate (Vector3.down * (speed) * Time.deltaTime);
			//directionToSet = Vector3.down;
		} else if (player.transform.position.y - .11f > transform.position.y) {
			transform.Translate (Vector3.up * (speed) * Time.deltaTime);
			//directionToSet = Vector3.up;
		}
		else {
			direction = directionToSet;
			StartCoroutine ("ReturnToPath");
		}

	}





	void move(int number) {
		xpos = transform.position.x;
		ypos = transform.position.y;

		if (true) {
			if (transform.position.x <= vertices [1].x && transform.position.y == vertices [0].y) {
				if (direction != Vector3.right && direction != Vector3.up)
					StartCoroutine ("MoveDelay", Vector3.down);
				direction = new Vector3 (1, 0, 0);
				//anim.SetFloat ("MoveX", 1);
				//anim.SetBool ("NPCMoving", true);
				xpos += speed * Time.deltaTime;
				pos = new Vector3 (xpos, ypos, 0);
				transform.position = pos;
			}
			if (transform.position.y > vertices [2].y && transform.position.x >= vertices [0].x) {
				if ((direction == new Vector3 (0, -1, 0)) && transform.position.x > 1.26f) {
					ypos += speed * -1 * Time.deltaTime;
					//anim.SetFloat ("MoveY", -1);
					//anim.SetBool ("NPCMoving", true);
					pos = new Vector3 (xpos, ypos, 0);
					transform.position = pos;
				} 
				if (transform.position.y > vertices [1].y) {
					if (direction == new Vector3 (0, 1, 0)) {
						StartCoroutine ("MoveDelay", Vector3.left);
					}
					if (direction == new Vector3 (-1, 0, 0)) {
						xpos += speed / -1 * Time.deltaTime;
						//anim.SetFloat ("MoveX", -1);
						//anim.SetBool ("NPCMoving", true);
						pos = new Vector3 (xpos, ypos, 0);
						transform.position = pos;
						if (transform.position.x <= vertices [0].x)
							transform.position = vertices [0];

					}

				}
			}



			if (transform.position.y <= vertices [1].y && transform.position.x > 1.26f) {
				if (transform.position.y < vertices [2].y) {
					direction = new Vector3 (0, 1, 0);
					//anim.SetFloat ("MoveY", 1);
					//anim.SetBool ("NPCMoving", true);
				}
				if (direction == new Vector3 (0, 1, 0)) {
					ypos += speed * Time.deltaTime;
					//anim.SetFloat ("MoveY", 1);
					//anim.SetBool ("NPCMoving", true);
					pos = new Vector3 (xpos, ypos, 0);
					transform.position = pos;
				}
			}


		}
	}

	
	IEnumerator MoveDelay(Vector3 dir) {
		yield return new WaitForSeconds (2f);
		direction = dir;
		StopCoroutine ("MoveDelay");

		
	}
	IEnumerator ReturnToPath() {
		yield return new WaitForSeconds (2f);
		if ((vertices [1].x) > transform.position.x) {
			transform.Translate (Vector3.right * (speed / 150));
		} else { 
			isAlerted = false;
			xpos = vertices [1].x;
			pos = new Vector3 (xpos, ypos, 0);
			transform.position = pos;

		}
	}

}
