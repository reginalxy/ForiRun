using UnityEngine;
using System.Collections;
using System;
using System.Xml;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GlobalSetting : MonoBehaviour {

	//public Text Console;

	static public int bestGrade = 0;
	static public int coinsCount = 1000;
	static public bool firstPlay = true;
	static public int blueCount = 0;
	static public int yellowCount = 0;
	static public int greenCount = 0;
	static public int protectorLevel = 0;
	static public int doubleLevel = 3;
	static public int boostLevel = 0;
	static public int protectorCount = 0;
	static public int doubleCount = 3;
	static public int boostCount = 0;
	static public int[] levelCost = new int[]{ 0, 50, 100, 200, 500 };
	static public int[] levelPlantsCost = new int[]{ 1, 1, 2, 2, 4 };
	static public bool playBgm = true;
	static public bool playEfx = true;
	static public int selectedRole = 0;
	static public bool roleEnable0 = true;
	static public bool roleEnable1 = true;
	static public bool roleEnable2 = false;

	static private bool Loading = true;
	private XmlDocument xmlDoc;
	private string filename;
	private string filepath;

	void Awake(){
		#if UNITY_EDITOR
		filepath=Application.dataPath;
		#endif

		#if UNITY_IOS
		filepath=Application.persistentDataPath;
		#endif

		//Debug.Log ("rootcount=" + SceneManager.GetActiveScene ().buildIndex);
		if (Loading) {
			Loading = false;
			filename = "//config.xml";
			LoadXml ();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnApplicationQuit(){
		//Debug.Log ("Quit.");
		SaveXml();
	}

	void OnApplicationPause(bool pauseStatus) {
		if (pauseStatus)
			SaveXml ();
	}

	void OnDestroy(){
		SaveXml ();
	}

	public void Save(){
		//Console.text = "loaded";
		SaveXml();
	}

	void LoadXml(){
		if (System.IO.File.Exists (filepath + filename)) {
			//Debug.Log ("loaded.");
			xmlDoc = new XmlDocument ();
			xmlDoc.Load (Application.persistentDataPath + filename);
			//Console.text = "loaded";
		} 
		else {
			//Console.text = "created";
			//Debug.Log ("created.");
			CreateXml ();
			return;
		}

		//xmlDoc.Load (Application.dataPath + filename);


		XmlNode xnConfig = xmlDoc.SelectSingleNode ("config");

		XmlNode xnBest = xnConfig.SelectSingleNode ("bestgrade");
		XmlNode xnCoins = xnConfig.SelectSingleNode ("coinscount");
		XmlNode xnFirstPlay = xnConfig.SelectSingleNode ("firstplay");

		XmlNode xnBlue = xnConfig.SelectSingleNode ("bluecount");
		XmlNode xnYellow = xnConfig.SelectSingleNode ("yellowcount");
		XmlNode xnGreen = xnConfig.SelectSingleNode ("greencount");

		XmlNode xnProtector = xnConfig.SelectSingleNode ("protectorlevel");
		XmlNode xnDouble = xnConfig.SelectSingleNode ("doublelevel");
		XmlNode xnBoost = xnConfig.SelectSingleNode ("boostlevel");

		XmlNode xnPtcCount = xnConfig.SelectSingleNode ("protectorcount");
		XmlNode xnDbCount = xnConfig.SelectSingleNode ("doublecount");
		XmlNode xnBstCount = xnConfig.SelectSingleNode ("boostcount");

		XmlNode xnLevelCost = xnConfig.SelectSingleNode ("levelcost");
		XmlNode xnLevelPlantsCost = xnConfig.SelectSingleNode ("levelplantscost");

		XmlNode xnSelectedRole = xnConfig.SelectSingleNode ("selectedrole");
		XmlNode xnRole0 = xnConfig.SelectSingleNode ("role0");
		XmlNode xnRole1 = xnConfig.SelectSingleNode ("role1");
		XmlNode xnRole2 = xnConfig.SelectSingleNode ("role2");

		bestGrade = Convert.ToInt32 (xnBest.InnerText);
		coinsCount = Convert.ToInt32 (xnCoins.InnerText);
		firstPlay = Convert.ToBoolean (xnFirstPlay.InnerText);

		blueCount = Convert.ToInt32 (xnBlue.InnerText);
		yellowCount = Convert.ToInt32 (xnYellow.InnerText);
		greenCount = Convert.ToInt32 (xnGreen.InnerText);

		protectorLevel = Convert.ToInt32 (xnProtector.InnerText);
		doubleLevel = Convert.ToInt32 (xnDouble.InnerText);
		boostLevel = Convert.ToInt32 (xnBoost.InnerText);

		protectorCount = Convert.ToInt32 (xnPtcCount.InnerText);
		doubleCount = Convert.ToInt32 (xnDbCount.InnerText);
		boostCount = Convert.ToInt32 (xnBstCount.InnerText);

		int i = 0;
		foreach (XmlNode xn in xnLevelCost.ChildNodes) {
			levelCost [i] = Convert.ToInt32 (xn.InnerText);
			i++;
		}
		i = 0;
		foreach (XmlNode xn in xnLevelPlantsCost.ChildNodes) {
			levelPlantsCost [i] = Convert.ToInt32 (xn.InnerText);
			i++;
		}
//		for (i = 0; i < 5; i++)
//			Debug.Log ("items=" + levelCost [i]);
		//Debug.Log("bestgrade="+bestGrade);
		//bestGrade = 999;

		selectedRole = Convert.ToInt32 (xnSelectedRole.InnerText);
		roleEnable0 = Convert.ToBoolean (xnRole0.InnerText);
		roleEnable1 = Convert.ToBoolean (xnRole1.InnerText);
		roleEnable2 = Convert.ToBoolean (xnRole2.InnerText);
		//Console.text = "loaded";
	}

	void CreateXml(){
		xmlDoc = new XmlDocument ();
		//xmlDoc.Load (Application.persistentDataPath + filename);
		//xmlDoc.Load (Application.dataPath + filename);

		XmlNode xnConfig = xmlDoc.CreateNode (XmlNodeType.Element, "config", null);
		xmlDoc.AppendChild (xnConfig);

		XmlNode xnBest = xmlDoc.CreateNode (XmlNodeType.Element, "bestgrade", null);
		xnConfig.AppendChild (xnBest);
		XmlNode xnCoins = xmlDoc.CreateNode (XmlNodeType.Element, "coinscount", null);
		xnConfig.AppendChild (xnCoins);
		XmlNode xnFirstPlay = xmlDoc.CreateNode (XmlNodeType.Element, "firstplay", null);
		xnConfig.AppendChild (xnFirstPlay);

		XmlNode xnBlue = xmlDoc.CreateNode (XmlNodeType.Element, "bluecount", null);
		xnConfig.AppendChild (xnBlue);
		XmlNode xnYellow = xmlDoc.CreateNode (XmlNodeType.Element, "yellowcount", null);
		xnConfig.AppendChild (xnYellow);
		XmlNode xnGreen = xmlDoc.CreateNode (XmlNodeType.Element, "greencount", null);
		xnConfig.AppendChild (xnGreen);

		XmlNode xnProtector = xmlDoc.CreateNode (XmlNodeType.Element, "protectorlevel", null);
		xnConfig.AppendChild (xnProtector);
		XmlNode xnDouble = xmlDoc.CreateNode (XmlNodeType.Element, "doublelevel", null);
		xnConfig.AppendChild (xnDouble);
		XmlNode xnBoost = xmlDoc.CreateNode (XmlNodeType.Element, "boostlevel", null);
		xnConfig.AppendChild (xnBoost);

		XmlNode xnPtcCount = xmlDoc.CreateNode (XmlNodeType.Element, "protectorcount", null);
		xnConfig.AppendChild (xnPtcCount);
		XmlNode xnDbCount = xmlDoc.CreateNode (XmlNodeType.Element, "doublecount", null);
		xnConfig.AppendChild (xnDbCount);
		XmlNode xnBstCount = xmlDoc.CreateNode (XmlNodeType.Element, "boostcount", null);
		xnConfig.AppendChild (xnBstCount);

		XmlNode xnLevelCost = xmlDoc.CreateNode (XmlNodeType.Element, "levelcost", null);
		xnConfig.AppendChild (xnLevelCost);
		XmlNode xnLvCst1 = xmlDoc.CreateNode (XmlNodeType.Element, "level1", null);
		xnLevelCost.AppendChild (xnLvCst1);
		XmlNode xnLvCst2 = xmlDoc.CreateNode (XmlNodeType.Element, "level2", null);
		xnLevelCost.AppendChild (xnLvCst2);
		XmlNode xnLvCst3 = xmlDoc.CreateNode (XmlNodeType.Element, "level3", null);
		xnLevelCost.AppendChild (xnLvCst3);
		XmlNode xnLvCst4 = xmlDoc.CreateNode (XmlNodeType.Element, "level4", null);
		xnLevelCost.AppendChild (xnLvCst4);
		XmlNode xnLvCst5 = xmlDoc.CreateNode (XmlNodeType.Element, "level5", null);
		xnLevelCost.AppendChild (xnLvCst5);

		XmlNode xnLevelPlantsCost = xmlDoc.CreateNode (XmlNodeType.Element, "levelplantscost", null);
		xnConfig.AppendChild (xnLevelPlantsCost);
		XmlNode xnLvPltCst1 = xmlDoc.CreateNode (XmlNodeType.Element, "level1", null);
		xnLevelPlantsCost.AppendChild (xnLvPltCst1);
		XmlNode xnLvPltCst2 = xmlDoc.CreateNode (XmlNodeType.Element, "level2", null);
		xnLevelPlantsCost.AppendChild (xnLvPltCst2);
		XmlNode xnLvPltCst3 = xmlDoc.CreateNode (XmlNodeType.Element, "level3", null);
		xnLevelPlantsCost.AppendChild (xnLvPltCst3);
		XmlNode xnLvPltCst4 = xmlDoc.CreateNode (XmlNodeType.Element, "level4", null);
		xnLevelPlantsCost.AppendChild (xnLvPltCst4);
		XmlNode xnLvPltCst5 = xmlDoc.CreateNode (XmlNodeType.Element, "level5", null);
		xnLevelPlantsCost.AppendChild (xnLvPltCst5);

		XmlNode xnSelectedRole = xmlDoc.CreateNode (XmlNodeType.Element, "selectedrole", null);
		xnConfig.AppendChild (xnSelectedRole);
		XmlNode xnRole0 = xmlDoc.CreateNode (XmlNodeType.Element, "role0", null);
		xnConfig.AppendChild (xnRole0);
		XmlNode xnRole1 = xmlDoc.CreateNode (XmlNodeType.Element, "role1", null);
		xnConfig.AppendChild (xnRole1);
		XmlNode xnRole2 = xmlDoc.CreateNode (XmlNodeType.Element, "role2", null);
		xnConfig.AppendChild (xnRole2);

		xnBest.InnerText = bestGrade.ToString ();
		xnCoins.InnerText = coinsCount.ToString ();
		xnFirstPlay.InnerText = firstPlay.ToString ();

		xnBlue.InnerText = blueCount.ToString ();
		xnYellow.InnerText = yellowCount.ToString ();
		xnGreen.InnerText = greenCount.ToString ();

		xnProtector.InnerText = protectorLevel.ToString ();
		xnDouble.InnerText = doubleLevel.ToString ();
		xnBoost.InnerText = boostLevel.ToString ();

		xnPtcCount.InnerText = protectorCount.ToString ();
		xnDbCount.InnerText = doubleCount.ToString ();
		xnBstCount.InnerText = boostCount.ToString ();

		xnLvCst1.InnerText = levelCost [0].ToString ();
		xnLvCst2.InnerText = levelCost [1].ToString ();
		xnLvCst3.InnerText = levelCost [2].ToString ();
		xnLvCst4.InnerText = levelCost [3].ToString ();
		xnLvCst5.InnerText = levelCost [4].ToString ();

		xnLvPltCst1.InnerText = levelPlantsCost [0].ToString ();
		xnLvPltCst2.InnerText = levelPlantsCost [1].ToString ();
		xnLvPltCst3.InnerText = levelPlantsCost [2].ToString ();
		xnLvPltCst4.InnerText = levelPlantsCost [3].ToString ();
		xnLvPltCst5.InnerText = levelPlantsCost [4].ToString ();

		xnSelectedRole.InnerText = selectedRole.ToString ();
		xnRole0.InnerText = roleEnable0.ToString ();
		xnRole1.InnerText = roleEnable1.ToString ();
		xnRole2.InnerText = roleEnable2.ToString ();

		xmlDoc.Save (Application.persistentDataPath + filename);
	}

	void SaveXml(){
		xmlDoc = new XmlDocument ();
		xmlDoc.Load (Application.persistentDataPath + filename);
		//xmlDoc.Load (Application.dataPath + filename);

		XmlNode xnConfig = xmlDoc.SelectSingleNode ("config");

		XmlNode xnBest = xnConfig.SelectSingleNode ("bestgrade");
		XmlNode xnCoins = xnConfig.SelectSingleNode ("coinscount");
		XmlNode xnFirstPlay = xnConfig.SelectSingleNode ("firstplay");

		XmlNode xnBlue = xnConfig.SelectSingleNode ("bluecount");
		XmlNode xnYellow = xnConfig.SelectSingleNode ("yellowcount");
		XmlNode xnGreen = xnConfig.SelectSingleNode ("greencount");

		XmlNode xnProtector = xnConfig.SelectSingleNode ("protectorlevel");
		XmlNode xnDouble = xnConfig.SelectSingleNode ("doublelevel");
		XmlNode xnBoost = xnConfig.SelectSingleNode ("boostlevel");

		XmlNode xnPtcCount = xnConfig.SelectSingleNode ("protectorcount");
		XmlNode xnDbCount = xnConfig.SelectSingleNode ("doublecount");
		XmlNode xnBstCount = xnConfig.SelectSingleNode ("boostcount");

		XmlNode xnSelectedRole = xnConfig.SelectSingleNode ("selectedrole");
		XmlNode xnRole0 = xnConfig.SelectSingleNode ("role0");
		XmlNode xnRole1 = xnConfig.SelectSingleNode ("role1");
		XmlNode xnRole2 = xnConfig.SelectSingleNode ("role2");

		//		XmlNode xnLevelCost = xnConfig.SelectSingleNode ("levelcost");
		//		XmlNode xnLevelPlantsCost = xnConfig.SelectSingleNode ("levelplantscost");

		//Console.text = "node Created";
	
		xnBest.InnerText = bestGrade.ToString ();
		xnCoins.InnerText = coinsCount.ToString ();
		xnFirstPlay.InnerText = firstPlay.ToString ();

		xnBlue.InnerText = blueCount.ToString ();
		xnYellow.InnerText = yellowCount.ToString ();
		xnGreen.InnerText = greenCount.ToString ();

		xnProtector.InnerText = protectorLevel.ToString ();
		xnDouble.InnerText = doubleLevel.ToString ();
		xnBoost.InnerText = boostLevel.ToString ();

		xnPtcCount.InnerText = protectorCount.ToString ();
		xnDbCount.InnerText = doubleCount.ToString ();
		xnBstCount.InnerText = boostCount.ToString ();

		xnSelectedRole.InnerText = selectedRole.ToString ();
		xnRole0.InnerText = roleEnable0.ToString ();
		xnRole1.InnerText = roleEnable1.ToString ();
		xnRole2.InnerText = roleEnable2.ToString ();

		//Console.text = "node updated";

		//Debug.Log("saved");
		//Console.text = "saved";
		xmlDoc.Save (Application.persistentDataPath + filename);
	}
}
