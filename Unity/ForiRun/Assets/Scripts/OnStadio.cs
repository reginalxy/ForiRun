using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OnStadio : MonoBehaviour {

	//cv_lab datas
	public Text txtProtector_lab;
	public Text txtBoost_lab;
	public Text txtDouble_lab;

	public Text txtLevelGreen_lab;
	public Text txtLevelBlue_lab;
	public Text txtLevelYellow_lab;

	public Button btCostGreen_lab;
	public Button btCostBlue_lab;
	public Button btCostYellow_lab;

	public Text txtCountGreen_lab;
	public Text txtCountBlue_lab;
	public Text txtCountYellow_lab;

	//cv_level datas
	public Text txtLevelProtector_level;
	public Text txtLevelBoost_level;
	public Text txtLevelDouble_level;

	public Text txtCountProtector_level;
	public Text txtCountBoost_level;
	public Text txtCountDouble_level;

	public Button btCostProtector_level;
	public Button btCostBoost_level;
	public Button btCostDouble_level;

	public GameObject pnProtector_level;
	public GameObject pnBoost_level;
	public GameObject pnDouble_level;

	public Text txtCoins_level;

	//cv_role datas
	public Image imgConfirm;

	public Button btRole0;
	public Button btRole1;
	public Button btRole2;
	public Button btRole1_purchase;
	public Button btRole2_purchase;

	public Image[] imgRoleSelected = new Image[3];
	public Text[] txtRoleSelected = new Text[3];
	public Text txtCoins_role;

	public AudioClip acButtons;
	public AudioClip acSwith;
	private SoundManager soundmanager;

	private int levelGreen;
	private int levelBlue;
	private int levelYellow;

	private int CountProtector;
	private int CountBoost;
	private int CountDouble;

	private GameObject[] ptcLevelBar = new GameObject[5];
	private GameObject[] bstLevelBar = new GameObject[5];
	private GameObject[] dbLevelBar = new GameObject[5];

	private int indexOfRoleToBuy;
	// Use this for initialization
	void Start () {
		soundmanager = GameObject.Find ("SoundManager").GetComponent<SoundManager> ();
		indexOfRoleToBuy = 0;

		levelGreen = GlobalSetting.protectorLevel;
		levelBlue = GlobalSetting.boostLevel;
		levelYellow = GlobalSetting.doubleLevel;

		CountProtector = GlobalSetting.protectorCount;
		CountBoost = GlobalSetting.boostCount;
		CountDouble = GlobalSetting.doubleCount;

		int i = 0;
		foreach (Transform child in pnProtector_level.transform) {
			ptcLevelBar [i] = child.gameObject;
			i++;
		}
		i = 0;
		foreach (Transform child in pnBoost_level.transform) {
			bstLevelBar [i] = child.gameObject;
			i++;
		}
		i = 0;
		foreach (Transform child in pnDouble_level.transform) {
			dbLevelBar [i] = child.gameObject;
			i++;
		}

		UpdateGreen ();
		UpdateBlue ();
		UpdateYellow ();
		txtCoins_level.text = GlobalSetting.coinsCount.ToString ();

		for (i = 0; i <= levelGreen; i++)
			ptcLevelBar [i].SetActive (true);
		for (i = 0; i <= levelBlue; i++)
			bstLevelBar [i].SetActive (true);
		for (i = 0; i <= levelYellow; i++)
			dbLevelBar [i].SetActive (true);

		for ( i = 0; i < 3; i++)
			CheckPlants (i);
		
		CheckCoins ();

		if (GlobalSetting.roleEnable1) {
			btRole1.gameObject.SetActive (true);
			btRole1_purchase.gameObject.SetActive (false);
		}
		if (GlobalSetting.roleEnable2) {
			btRole2.gameObject.SetActive (true);
			btRole2_purchase.gameObject.SetActive (false);
		}
		txtCoins_role.text = GlobalSetting.coinsCount.ToString ();
		imgRoleSelected [GlobalSetting.selectedRole].gameObject.SetActive (true);
		txtRoleSelected [GlobalSetting.selectedRole].text = "Selected";
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void bt_Home(){
		soundmanager.PlaySingle (acButtons);
		SceneManager.LoadScene (0);
	}

	public void GetBuff(int index){
		soundmanager.PlaySingle (acButtons);

		switch (index) {
		case 0:
			CountProtector++;
			GlobalSetting.protectorCount++;
			GlobalSetting.greenCount -= GlobalSetting.levelPlantsCost [levelGreen];
			UpdateGreen ();
			break;
		case 1:
			CountBoost++;
			GlobalSetting.boostCount++;
			GlobalSetting.blueCount -= GlobalSetting.levelPlantsCost [levelBlue];
			UpdateBlue ();
			break;
		case 2:
			CountDouble++;
			GlobalSetting.doubleCount++;
			GlobalSetting.yellowCount -= GlobalSetting.levelPlantsCost [levelYellow];
			UpdateYellow ();
			break;
		}
		CheckPlants (index);
	}

	public void LevelUp(int index){
		soundmanager.PlaySingle (acButtons);

		switch (index) {
		case 0:
			levelGreen++;
			//Debug.Log ("levelGreen=" + levelGreen);
			GlobalSetting.protectorLevel++;
			GlobalSetting.coinsCount -= GlobalSetting.levelCost [levelGreen];
			txtCoins_level.text = GlobalSetting.coinsCount.ToString ();
			ptcLevelBar [levelGreen].SetActive (true);
			UpdateGreen ();
			break;
		case 1:
			levelBlue++;
			GlobalSetting.boostLevel++;
			GlobalSetting.coinsCount -= GlobalSetting.levelCost [levelBlue];
			txtCoins_level.text = GlobalSetting.coinsCount.ToString ();
			bstLevelBar [levelBlue].SetActive (true);
			UpdateBlue ();
			break;
		case 2:
			levelYellow++;
			GlobalSetting.doubleLevel++;
			GlobalSetting.coinsCount -= GlobalSetting.levelCost [levelYellow];
			txtCoins_level.text = GlobalSetting.coinsCount.ToString ();
			dbLevelBar [levelYellow].SetActive (true);
			UpdateYellow ();
			break;
		}
		CheckPlants (index);
		CheckCoins ();
	}

	public void Switch(){
		soundmanager.PlaySingle (acSwith);
	}

	public void purchaseRole(int index){
		soundmanager.PlaySingle (acButtons);
	
		indexOfRoleToBuy = index;
		imgConfirm.gameObject.SetActive (true);
	}

	public void confirmPurchase(){
		soundmanager.PlaySingle (acButtons);

		switch(indexOfRoleToBuy){
		case 1:
			btRole1_purchase.gameObject.SetActive (false);
			btRole1.gameObject.SetActive (true);
			GlobalSetting.roleEnable1 = true;
			break;
		case 2:
			btRole2_purchase.gameObject.SetActive (false);
			btRole2.gameObject.SetActive (true);
			GlobalSetting.roleEnable2 = true;
			break;
		}
		imgConfirm.gameObject.SetActive (false);

		GlobalSetting.coinsCount -= 500;
		txtCoins_role.text = GlobalSetting.coinsCount.ToString ();
		txtCoins_level.text = GlobalSetting.coinsCount.ToString ();
		CheckCoins ();
	}

	public void cancelPurchase(){
		soundmanager.PlaySingle (acButtons);
	
		imgConfirm.gameObject.SetActive (false);
	}

	public void roleSelect(int index){
		soundmanager.PlaySingle (acButtons);

		imgRoleSelected [GlobalSetting.selectedRole].gameObject.SetActive (false);
		txtRoleSelected [GlobalSetting.selectedRole].text = "Unlocked";
		GlobalSetting.selectedRole = index;
		imgRoleSelected [GlobalSetting.selectedRole].gameObject.SetActive (true);
		txtRoleSelected [GlobalSetting.selectedRole].text = "Selected";
	}

	void CheckPlants(int index){
		if (index == 0 && GlobalSetting.greenCount < GlobalSetting.levelPlantsCost [levelGreen])
			btCostGreen_lab.interactable = false;
		if (index == 1 && GlobalSetting.blueCount < GlobalSetting.levelPlantsCost [levelBlue])
			btCostBlue_lab.interactable = false;
		if (index == 2 && GlobalSetting.yellowCount < GlobalSetting.levelPlantsCost [levelYellow])
			btCostYellow_lab.interactable = false;
	}

	void CheckCoins(){
		if (levelGreen == 4) 
			btCostProtector_level.gameObject.SetActive (false);
		else if (GlobalSetting.coinsCount < GlobalSetting.levelCost [levelGreen+1])
			btCostProtector_level.interactable = false;

		if (levelBlue == 4)
			btCostBoost_level.gameObject.SetActive (false);
		else if (GlobalSetting.coinsCount < GlobalSetting.levelCost [levelBlue+1])
			btCostBoost_level.interactable = false;

		if (levelYellow == 4)
			btCostDouble_level.gameObject.SetActive (false);
		else if (GlobalSetting.coinsCount < GlobalSetting.levelCost [levelYellow+1])
			btCostDouble_level.interactable = false;

		if (!GlobalSetting.roleEnable1 && GlobalSetting.coinsCount < 500)
			btRole1_purchase.interactable = false;
		
		if (!GlobalSetting.roleEnable2 && GlobalSetting.coinsCount < 500)
			btRole2_purchase.interactable = false;
	}

	void UpdateGreen(){
		//lab
		txtProtector_lab.text = CountProtector.ToString ();
		txtLevelGreen_lab.text = (levelGreen + 1).ToString ();
		btCostGreen_lab.GetComponentInChildren<Text> ().text = GlobalSetting.levelPlantsCost [levelGreen].ToString ();
		txtCountGreen_lab.text = GlobalSetting.greenCount.ToString ();

		//level
		txtLevelProtector_level.text = (levelGreen+1).ToString ();
		txtCountProtector_level.text = CountProtector.ToString ();
		if (levelGreen == 4) 
			btCostProtector_level.gameObject.SetActive (false);
		else 
			btCostProtector_level.GetComponentInChildren<Text> ().text = GlobalSetting.levelCost [levelGreen+1].ToString ();
	}

	void UpdateBlue(){
		//lab
		txtBoost_lab.text = CountBoost.ToString ();
		txtLevelBlue_lab.text = (levelBlue + 1).ToString ();
		btCostBlue_lab.GetComponentInChildren<Text>().text = GlobalSetting.levelPlantsCost [levelBlue].ToString ();
		txtCountBlue_lab.text = GlobalSetting.blueCount.ToString ();

		//level
		txtLevelBoost_level.text = (levelBlue + 1).ToString ();
		txtCountBoost_level.text = CountBoost.ToString ();
		if (levelBlue == 4)
			btCostBoost_level.gameObject.SetActive (false);
		else
			btCostBoost_level.GetComponentInChildren<Text> ().text = GlobalSetting.levelCost [levelBlue+1].ToString ();
	}

	void UpdateYellow(){
		//lab
		txtDouble_lab.text = CountDouble.ToString ();
		txtLevelYellow_lab.text = (levelYellow + 1).ToString ();
		btCostYellow_lab.GetComponentInChildren<Text>().text = GlobalSetting.levelPlantsCost [levelYellow].ToString ();
		txtCountYellow_lab.text = GlobalSetting.yellowCount.ToString ();

		//level
		txtLevelDouble_level.text = (levelYellow + 1).ToString ();
		txtCountDouble_level.text = CountDouble.ToString ();
		if (levelYellow == 4)
			btCostDouble_level.gameObject.SetActive (false);
		else
			btCostDouble_level.GetComponentInChildren<Text> ().text = GlobalSetting.levelCost [levelYellow+1].ToString ();
	}
}
