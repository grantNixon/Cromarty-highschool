using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FOV_VisualBully : MonoBehaviour {

	public float length;
	public float angle;

	public Vector3 dFromAngle(float angle, bool isGlobal) {
		if (!isGlobal) {
			ComputerControllerBully C = GetComponent<ComputerControllerBully> ();
			if (C.getDirection() == Vector3.right)
				angle += 90;
			else if (C.getDirection() == Vector3.left)
				angle += -90;
			else if (C.getDirection() == Vector3.down)
				angle += 180;
			
		}
		return new Vector3 (Mathf.Sin (angle * Mathf.Deg2Rad), Mathf.Cos (angle * Mathf.Deg2Rad), -1);
	}
	// Use this for initialization
	void Start () {
		FieldOfViewBully player = GetComponent<FieldOfViewBully> ();
		length = player.viewRadius;
		angle = player.viewAngle;
	}

	// Update is called once per frame
	void Update () {
		
		LineRenderer render = GetComponent<LineRenderer> ();
		Rigidbody2D rb2d = GetComponent<Rigidbody2D> ();
		render.SetPosition (1, rb2d.transform.position);
		//if()
			//reflection
		render.SetPosition (0, rb2d.transform.position + dFromAngle(-angle / 2, false) * length);
		//if(direction)
			//reflection
		render.SetPosition (2, rb2d.transform.position + dFromAngle(angle / 2, false) * length );
	}
}
