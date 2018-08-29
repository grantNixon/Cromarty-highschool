using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOV_Visual : MonoBehaviour {

	public float length;
	public float angle;
	float angleN;

	public Vector3 dFromAngle(float angle, bool isGlobal) {
		if (!isGlobal) {
			ComputerController player = GetComponent<ComputerController> ();
			if (player.getDirection() == Vector3.right)
				angle += 90;
			else if (player.getDirection() == Vector3.left)
				angle += -90;
			else if (player.getDirection() == Vector3.down)
				angle += 180;
		}
		return new Vector3 (Mathf.Sin (angle * Mathf.Deg2Rad), Mathf.Cos (angle * Mathf.Deg2Rad), -1);
	}
	// Use this for initialization
	void Start () {
		FieldOfView player = GetComponent<FieldOfView> ();
		length = player.viewRadius;
		angleN = player.viewAngle;

	}

	// Update is called once per frame
	void Update () {
		
		LineRenderer render = GetComponent<LineRenderer> ();
		//Rigidbody2D rb2d = GetComponent<Rigifdbody2D> ();

		render.SetPosition (1, transform.position);
		//if()
			//reflection
		render.SetPosition (0, transform.position + dFromAngle(-angleN / 2, false) * length);
		//if(direction)
			//reflection
		render.SetPosition (2, transform.position + dFromAngle(angleN / 2, false) * length );
	}
}
