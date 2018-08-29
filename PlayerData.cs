using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerData : MonoBehaviour {
	public static PlayerData Instance;
	public List<GameObject> inventory;
	public List<Sprite> inventoryImg;
	public string lastScene;
	public int invCount = 0;
	public int itemIndex = 0;
	public int coins;
	public int successCount;
	public int successLimit;
	// Use this for initialization
	void Awake () {
		if (Instance == null) {
			DontDestroyOnLoad (gameObject);
			//DontDestroyOnLoad (inventory);
			Instance = this;
		} 
		else if (Instance != this) {
			Destroy (gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {


	}
}
