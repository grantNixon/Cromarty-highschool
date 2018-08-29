using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class PlayerController : MonoBehaviour {
	public GameObject Miss1,Miss2,Miss3, WinScreen;
	public GameObject img0,img1,img2,img3,img4,img5, img6;
	public AudioSource Error, Ding, Detected;
	public float speed = 2;
	public float testSpeed = 2;
	public int successLimit;
	public int missedCount;
	public int missedLimit;
	public int playerCoins;
	public int goalCount = 0;
	Vector3 direction;
	public bool miniEnable;
	float sprintSpeed;
	float walkSpeed;
	public float sliderSpeed = 1;
	public Text gameMessage;
	public Text gameStatus;
	public int playerItemIndex;
	public bool objComplete;
	public bool lvlComplete;
	public int successCount;
	public bool isDetectable = true;
	public Text progressTimer;
	private Rigidbody2D rb2d;
	public GameObject item;
	public string lastScene;
	public bool use = false;
	GameObject[] useables;
	GameObject[] alerts;
	public List<GameObject> inventory;
	GameObject portrait;
	GameObject bar;
	GameObject slider;
	GameObject goalDesk;
	public GoalManager goal;
	public Scene classroom1;
	public Scene classroom2;
	public Scene classroom3;
	public List<GoalManager> goals;
	void Start () {
		Spawn ();
		InitializePlayer ();
		if (SceneManager.GetActiveScene() == classroom1 || SceneManager.GetActiveScene() == classroom2 || SceneManager.GetActiveScene() == classroom3) {
			bar = GameObject.Find ("Minigame Bar");
			slider = GameObject.Find ("Minigame Slider");
			bar.SetActive (false);
			slider.SetActive (false);
		} 
		else if (SceneManager.Equals(SceneManager.GetActiveScene(), SceneManager.GetSceneByName("Bathroom"))) {
			portrait = GameObject.Find ("MERCHANT");
			portrait.SetActive (false);
		}
		gameMessage.text = "Steal the test answers: " + successCount.ToString () + "/" + successLimit.ToString();
		gameStatus.text = ":   " + playerCoins.ToString ();
		useables = GameObject.FindGameObjectsWithTag ("useable");
		alerts = GameObject.FindGameObjectsWithTag ("Alert");
		for (int i = 0; i < alerts.Length; i++) {
			SpriteRenderer sp = alerts [i].GetComponent<SpriteRenderer> ();
			sp.enabled = false;
		}
		if(inventory.Capacity > 0)
			item = inventory [playerItemIndex - 1];
		for (int i = 0; i < useables.Length; i++) {
			SpriteRenderer sp = useables [i].GetComponent<SpriteRenderer> ();
			sp.enabled = false;
		}



	}
	void Awake() {

		//Spawn ();
	}


		
	void FixedUpdate () {
		float moveHorizontal = Input.GetAxisRaw ("Horizontal");
		float moveVertical = Input.GetAxisRaw ("Vertical");
		string playerState = getState ();
		Vector2 movement = new Vector2 (moveHorizontal, moveVertical).normalized * (speed) * Time.deltaTime;
		rb2d.MovePosition (rb2d.position + movement);


	}
	void Update () {
		if (Input.GetKey ("r")) {
			PlayerData.Instance.coins = 0;
			if (lastScene == "Bathroom" || lastScene == "HallwayF")
				lastScene = "";
			SceneManager.LoadSceneAsync (Application.loadedLevel);
		}
		if (Input.GetKey (KeyCode.Space) && lvlComplete == true) {
			SceneManager.LoadSceneAsync ("Classroom2");
		}
		if (Input.GetKey ("escape"))
			Application.Quit ();
		if (Input.GetKey ("left shift"))
			speed = sprintSpeed;
		else
			speed = walkSpeed;
		if (Input.GetKeyDown ("left ctrl")) {
			UseSelectedItem (inventory[playerItemIndex - 1], 0);
		}
		if (Input.GetKey ("e"))
			use = true;
		else
			use = false;
		if (miniEnable == true && successCount <= successLimit)
			MiniGame ();
		else if (successCount >= successLimit) {
			gameMessage.text = "Answers stolen! Return to your desk";
			objComplete = true;
		}
		gameStatus.text = playerCoins.ToString ();
	}

	public string getState () {
		string state = "idle";
		if (Input.GetAxisRaw ("Horizontal") > 0)
			state = "right";
		else if (Input.GetAxisRaw ("Horizontal") < 0)
			state = "left";
		else if (Input.GetAxisRaw ("Vertical") < 0)
			state = "down";
		else if (Input.GetAxisRaw ("Vertical") == 0 && Input.GetAxisRaw ("Horizontal") == 0)
			state = "idle";

		return state;
	}

	void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.CompareTag ("start")) {
			if (objComplete == true) {
				gameMessage.text = "YOU PASSED!";
				lvlComplete = true;

			}
	}
}

	void OnTriggerStay2D(Collider2D other) {
		if (other.gameObject.CompareTag("Shady Merchant")) {
			if (use == true) {
				gameMessage.text = "Hey kid, want something to help \n distract the teachers?" +
					"(Press Space to buy(30 Coins))";
				portrait.SetActive (true);
			}
			if (Input.GetKeyDown (KeyCode.Space)) {
				if (playerCoins >= 30) {
					item = GameObject.Find ("JeffD");
					addInventory (item);
					gameMessage.text = "Pleasure doing business with ya, kid";
					playerCoins -= 30;
				} else
					gameMessage.text = "Sorry kid, come back when you have more coin";
			}
		}
		if (other.gameObject.CompareTag ("goal")) {
			if (!goals.Contains (other.gameObject.GetComponent<GoalManager> ())) {
				goals.Add (other.gameObject.GetComponent<GoalManager> ());
			}
			goal = other.gameObject.GetComponent<GoalManager> ();
			goalCount = goal.goalCount;
			if (use == true) {
				miniEnable = true;
			}
		
				//}
				//else
				//YOU GOT THE ANSWERS ALREADY
				//;
			}

		if (other.gameObject.CompareTag("coins")) {
			if(use == true) {
				other.gameObject.SetActive(false);
				playerCoins += 10;
			}
		}
		if (other.gameObject.CompareTag ("start")) {
			if (objComplete == true) {
				gameMessage.text = "YOU PASSED!";
				lvlComplete = true;
			}
			isDetectable = false;

		}

	}
		

	void addInventory(GameObject item) {
		if (inventory.Count >= 5)
			inventory.Clear ();
		DontDestroyOnLoad (item);
		inventory.Add (item);
		playerItemIndex++;
	}

	void OnTriggerExit2D(Collider2D other) {
		if (other.gameObject.CompareTag ("goal")) {
			//Time.timeScale = 0;
			bar.SetActive (false);
			slider.SetActive (false);
			miniEnable = false;
			Miss1.SetActive (false);
			Miss2.SetActive (false);
			Miss3.SetActive (false);
			img0.SetActive (false);
			img1.SetActive (false);
			img2.SetActive (false);
			img3.SetActive (false);
			img4.SetActive (false);
			img5.SetActive (false);


		}
		if (other.gameObject.CompareTag ("start")) {
			isDetectable = true;
		}
		if (other.gameObject.CompareTag ("Shady Merchant")) {
			gameMessage.text = "Steal the test answers: " + successCount.ToString () + "/" + successLimit.ToString();
			portrait.SetActive (false);
		}

	}
		

	void UseSelectedItem (GameObject itemIn, int index) {
		Animator anim = GetComponent<Animator> ();
		bool facing = anim.GetBool ("Right");
		itemIn.transform.localScale = transform.localScale * 0.75f;
		itemIn.GetComponent<SpriteRenderer> ().sortingLayerName = "Player";
		itemIn.layer = 9;
		itemIn.tag = "useable";
		itemIn.AddComponent<BoxCollider2D> ();
		if (itemIn != null) {
			if (facing == true)
				itemIn.transform.position = transform.position + new Vector3 (0.3f, 0, 0);
			else if (facing == false)
				itemIn.transform.position = transform.position - new Vector3 (-0.3f, 0, 0);
			itemIn.SetActive (true);
			SpriteRenderer sp = itemIn.GetComponent<SpriteRenderer> ();
			sp.enabled = true;
		}
		inventory [playerItemIndex - 1] = null;
		playerItemIndex--;
		StartCoroutine ("ItemDeactivate", item);
		item = null;
	}

	IEnumerator ItemDeactivate(GameObject item) {
		yield return new WaitForSeconds (10f);
		item.SetActive (false);


	}
		
	void MiniGame () {
		bool pressed;
		img0.SetActive (true);
		if (Input.GetKey (KeyCode.Space))
			pressed = true;
		else
			pressed = false;
		Slider (pressed);
	}
	void NextItem () {
		playerItemIndex++;
		PlayerData.Instance.itemIndex = playerItemIndex;
	}
	void missCounter(int misses){
		if (misses == 1) {
			Miss1.SetActive (true);
		
		}
		 if (misses == 2) {
			Miss2.SetActive (true);
		}
		 if (misses == 3) {
			Miss3.SetActive (true);
		
		}
	}
	void goalCounter(int successCount)
	{

		if (successCount == 1) {
			img0.SetActive (false);
			img1.SetActive (true);

		} else if (successCount == 2) {
			img1.SetActive (false);
			img2.SetActive (true);
		} else if (successCount == 3) {
			img2.SetActive (false);
			img3.SetActive (true);
		} else if (successCount == 4) {
			img3.SetActive (false);
			img4.SetActive (true);
		} else if (successCount == 5) {
			img4.SetActive (false);
			img5.SetActive (true);
			img6.SetActive (true);
		} else if (successCount == 5) {
			img4.SetActive (false);
			img5.SetActive (true);
			//img6.SetActive (true);
		}
	}

	void Slider (bool stop) {
		float leftZone = 2.274f;
		float rightZone = 2.5f;
		bar.SetActive (true);
		slider.SetActive (true);
		RectTransform rectT = bar.GetComponent<RectTransform> ();
		float leftBound = bar.transform.position.x - rectT.rect.width / 2;
		float rightBound = bar.transform.position.x + rectT.rect.width / 2;
		Vector3 start = bar.transform.position;
		Vector3 curPos = slider.transform.position;
		goalCounter (goal.goalCount);
		missCounter (missedCount);
	
		if (goal.goalCount < 5) {
			if (!stop) {
				if (slider.transform.position.x > leftBound && direction == Vector3.left) {
					curPos.x -= sliderSpeed * Time.deltaTime;
					slider.transform.position = curPos;
				} else if (slider.transform.position.x < leftBound) {
					direction = Vector3.right;
					slider.transform.position = new Vector3 ((leftBound + .003f), 0.3f, 0);
				}
				if (slider.transform.position.x < rightBound && direction == Vector3.right) {
					curPos.x += sliderSpeed * Time.deltaTime;
					slider.transform.position = curPos;
				} else if (slider.transform.position.x > rightBound) {
					direction = Vector3.left;
					slider.transform.position = new Vector3 ((rightBound - .003f), (.3f - .223103f), 0);
				}
			} else if (slider.GetComponent<RectTransform> ().position.x >= leftZone && slider.GetComponent<RectTransform> ().position.x <= rightZone) {
				if (Input.GetKeyDown (KeyCode.Space)) {
					successCount++;
					goal.goalCount++;
					Ding.Play ();
				 /*if (successCount == 5) {
						img4.SetActive (false);
						img5.SetActive (true);
						img6.SetActive (true);
					}*/
						//Time.timeScale = 0;
						sliderSpeed += .1f;
						gameMessage.text = "Steal the test answers: " + goal.goalCount.ToString () + "/" + successLimit.ToString ();
					}
				} else if (!(slider.GetComponent<RectTransform> ().position.x >= leftZone && slider.GetComponent<RectTransform> ().position.x <= rightZone)) {
					if (Input.GetKeyDown (KeyCode.Space)) {
						missedCount++;
						progressTimer.text = missedCount.ToString ();
					Error.Play ();
				}
				if (missedCount >= missedLimit) {
					ComputerController c = GameObject.Find ("Prefect_0").GetComponent<ComputerController> ();
					c.isAlerted = true;
					goal.isAlerted = true;
					Detected.Play ();
					//activate GameObject for exclamation point over Smart Kid
					for (int i = 0; i < alerts.Length; i++) {
						SpriteRenderer sp = alerts [i].GetComponent<SpriteRenderer> ();
						sp.enabled = true;
					}

					}
				}
			} else {
				//goalCount = 0;
				miniEnable = false;
				bar.SetActive (false);
				slider.SetActive (false);
			Miss1.SetActive (false);
			Miss2.SetActive (false);
			Miss3.SetActive (false);
			}
		}

	
	void InitializePlayer() {
		rb2d = GetComponent<Rigidbody2D> ();
		missedLimit = 3;
		successCount = PlayerData.Instance.successCount;
		missedCount = 0;
		playerCoins = PlayerData.Instance.coins;
		direction = Vector3.left;
		objComplete = false;
		miniEnable = false;
		sprintSpeed = speed * 1.5f;
		walkSpeed = speed;
		inventory = PlayerData.Instance.inventory;
		playerItemIndex = PlayerData.Instance.itemIndex;
		classroom1 = SceneManager.GetSceneByName ("Jeff");
		classroom2 = SceneManager.GetSceneByName ("Classroom2");
		classroom3 = SceneManager.GetSceneByName ("CLassroom3");
		InitializeObjective (SceneManager.GetActiveScene ());
	}

	void Spawn() {
		if(PlayerData.Instance.lastScene == "Bathroom")
			transform.position = new Vector3 (1.42f, 1.37f, 0);
		if (PlayerData.Instance.lastScene == "classroom2")
			transform.position = new Vector3 (-8.92f, -0.91f, 0);
		if (PlayerData.Instance.lastScene == "classroom3")
			transform.position = new Vector3 (8.92f, -0.91f, 0);
		if (PlayerData.Instance.lastScene == "Bathroom2")
			transform.position = new Vector3 (1.31f, -0.54f, 0);
		if (PlayerData.Instance.lastScene == "HallwayF")
			transform.position = new Vector3 (-1.53f, -0.54f, 0);
		if (PlayerData.Instance.lastScene == "HallwayB")
			transform.position = new Vector3 (2.09f, 1.74f, 0);
		else
			;
			
	}

	void InitializeObjective(Scene currentLevel) {
		if (currentLevel == classroom1 || PlayerData.Instance.lastScene == "Bathroom" || PlayerData.Instance.lastScene == "Jeff")
			successLimit = 5;
		if (currentLevel == classroom2 || PlayerData.Instance.lastScene == "classroom2" || currentLevel == classroom3 ||PlayerData.Instance.lastScene == "classroom3f")
			successLimit = 15;
	
	}
}
