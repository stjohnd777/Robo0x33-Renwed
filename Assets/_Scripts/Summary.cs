using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Summary : MonoBehaviour {

    //public Text UISummary;

	public GameObject SummaryPanel;

	public GameObject prefabUISummaryText;

    void Awake () {

		if (MenuManager.sLevelDescriptors == null) {
			return;
		}

		int count = MenuManager.sLevelDescriptors.Length;
		LevelDescriptor[] descs = MenuManager.sLevelDescriptors;
		for (int index = 0; index < count ; index++) {
			
			LevelDescriptor desc = descs [index];

			//string sceneName = desc.sceneName;
			string levelName = desc.sceneDisplayName;
			int score = PlayerPrefManager.GetScore (desc.sceneName);
			int hi = PlayerPrefManager.GetHighScore (desc.sceneName);
			int time = PlayerPrefManager.GetTime (desc.sceneName);

//			GameObject goIndex = GameObject.Instantiate (prefabUISummaryText,Vector3.zero,Quaternion.identity) as GameObject;
//			goIndex.name = "index." + index;
//			goIndex.transform.SetParent(SummaryPanel.transform,false);
//			Text txtIndex = goIndex.GetComponent<Text> ();
//			txtIndex.text = "" + index;

			GameObject goName = GameObject.Instantiate (prefabUISummaryText,Vector3.zero,Quaternion.identity) as GameObject;
			goName.name = "name." + levelName;
			goName.transform.SetParent(SummaryPanel.transform,false);
			Text txtName = goName.GetComponent<Text> ();
			txtName.text = levelName;

			GameObject goScore = GameObject.Instantiate (prefabUISummaryText,Vector3.zero,Quaternion.identity) as GameObject;
			goScore.name = "score." + levelName;
			goScore.transform.SetParent(SummaryPanel.transform,false);
			Text txtScore = goScore.GetComponent<Text> ();
			txtScore.text = "" + score;

			GameObject gohi = GameObject.Instantiate (prefabUISummaryText,Vector3.zero,Quaternion.identity) as GameObject;
			gohi.name = "hi." + levelName;
			gohi.transform.SetParent(SummaryPanel.transform,false);
			Text txthi = gohi.GetComponent<Text> ();
			txthi.text = "" + hi;

			GameObject goTime = GameObject.Instantiate (prefabUISummaryText,Vector3.zero,Quaternion.identity) as GameObject;
			goTime.name = "time." + levelName;
			goTime.transform.SetParent(SummaryPanel.transform,false);
			Text txtTime = goTime.GetComponent<Text> ();
			txtTime.text = "" + time;

		}

//        string stats = PlayerPrefManager.Sumary();
//        UISummary.text = stats;

    }
	
 
}
