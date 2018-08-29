using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour {
	PlayerData player;
	public Image itemImg;
	public List<Sprite> inventoryImg;
	PlayerController playerC;
	public int disp;
	public GameObject item;
	public Sprite testvar;
	// Use this for initialization
	void Awake () {
		itemImg = GetComponent<Image> ();
		playerC = FindObjectOfType<PlayerController> ();
		item = playerC.item;
		inventoryImg = PlayerData.Instance.inventoryImg;
	}
	
	// Update is called once per frame
	void Update () {
		playerC = FindObjectOfType<PlayerController> ();
		disp = playerC.playerItemIndex;
		item = playerC.inventory[disp - 1];
		if (item == null)
			testvar = null;
		else
			testvar = item.GetComponent<SpriteRenderer> ().sprite;
		if (!inventoryImg.Contains (testvar)) {
			inventoryImg.Add (testvar);
			itemImg.sprite = testvar;
			PlayerData.Instance.inventoryImg = inventoryImg;

		}
		else if (item == null) {
			itemImg.sprite = null;
			//Time.timeScale = 0;
		}
		else if (itemImg.sprite != testvar)
			itemImg.sprite = testvar;
}
}
