using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
using UnityEngine.SceneManagement;
using UnityEngine.SceneManagement;
public class FieldOfViewSeated : MonoBehaviour {
	public AudioSource Lose;
	public GameObject LoseScreen;
	public float viewRadius;
	[Range(0, 360)]
	public float viewAngle;
	public float restartTimer;
	public float restartDelay = 5f;
	public Vector3[] newVertices;
	public Vector2[] newUV;
	public Text ifDetected;
	bool isDetected = false;
	public int count= 1;
	public LayerMask playerMask;
	public LayerMask obstacleMask;

	public bool PlayMusic;
	public List<Transform> targetsInView = new List<Transform>();

	public float GetRadius() {
		return viewRadius;
	}

	void Start() {

		StartCoroutine ("FindWithDelay", 0f);
		isDetected = false;
		LoseScreen.SetActive (false);
	}

	void Update() {
		if (count == 1 && isDetected) {
			count = 0;
			Lose.Play ();

		}
		FindVisibleTargets ();
		if (isDetected) {
			restartTimer += Time.deltaTime;
			if (restartTimer >= restartDelay)
				SceneManager.LoadSceneAsync(Application.loadedLevel);
		}

	
	}
		
	IEnumerator FindWithDelay(float delay) {
		while (true) {
			yield return new WaitForSeconds (delay);
			FindVisibleTargets ();
		}
	}
		

	public Vector3 dFromAngle(float angle, bool isGlobal) {
		if (!isGlobal) {
			ComputerControllerSeated player = GetComponent<ComputerControllerSeated> ();
			if (player.getDirection() == Vector3.right)
				angle += 90;
			else if (player.getDirection() == Vector3.left)
				angle += -90;
			else if (player.getDirection() == Vector3.down)
				angle += 180;

			}

		return new Vector2 (Mathf.Sin (angle * Mathf.Deg2Rad), Mathf.Cos (angle * Mathf.Deg2Rad));
	}
	void FindVisibleTargets() {
	targetsInView.Clear ();
	Collider2D[] targetsInViewRadius = Physics2D.OverlapCircleAll ((new Vector2(transform.position.x, transform.position.y)), viewRadius, playerMask);
	ComputerControllerSeated npc = GetComponent<ComputerControllerSeated> ();
	for (int i = 0; i < targetsInViewRadius.Length; i++) {
		Transform target = targetsInViewRadius [i].transform;
		Vector3 directionToTarget = (target.position - transform.position);
		if (Vector2.Angle (npc.getDirection2D(), directionToTarget) < viewAngle / 2) {
			RaycastHit2D hit = Physics2D.Raycast (transform.position, npc.getDirection2D());
			if (hit.collider != null) {
				targetsInView.Add (target);
					if (target.gameObject.CompareTag ("useable") && target.gameObject.GetComponent<SpriteRenderer> ().enabled == true) {
						StartCoroutine ("UseableDelay", 10f);
					} else if (target.gameObject.CompareTag ("useable") && target.gameObject.GetComponent<SpriteRenderer> ().enabled == false)
					;else {	
					//ifDetected.text = "DETECTED! Restarting...";
					isDetected = true;
					PlayerData.Instance.coins = 0;
					LoseScreen.SetActive (true);
				}
			}
		}
	}


}

IEnumerator UseableDelay (float delay) {
	ComputerControllerSeated npc = GetComponent<ComputerControllerSeated> ();
		npc.angle = 0;
		npc.radius = 0;
		npc.paused = true;
	yield return new WaitForSeconds (delay);
		npc.paused = false;
}



}
