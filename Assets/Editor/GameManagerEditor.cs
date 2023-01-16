//using UnityEngine;
//using System.Collections;
//
//using UnityEditor; // this is needed since this script references the Unity Editor
//[CustomEditor(typeof(GameManager))]
//public class GameManagerEditor : Editor { // exten d the Editor class
//
//	// called when Unity Editor Inspector is updated
//	public override void OnInspectorGUI()
//	{
//		// show the default inspector stuff for this component
//		DrawDefaultInspector();
//
//		// get a reference to the GameManager script on this target gameObject
//		GameManager myGM = (GameManager)target;
//
//		// add a custom button to the Inspector component
//		if(GUILayout.Button("Reset Player State"))
//		{
//			// if button pressed, then call function in script
//			PlayerPrefManager.ResetPlayerState(myGM.startLives, false,false);
//		}
//
//        // add a custom button to the Inspector component
//        if (GUILayout.Button("Reset All Scores = 0"))
//        {
//            MainMenuManager mm = PlayerPrefManager.getMenuManager();
//
//            foreach (string levelName in mm.LevelNames)
//            {
//                PlayerPrefManager.SetScore(levelName, 0);
//            }
//        }
//
//        // add a custom button to the Inspector component
//        if (GUILayout.Button("Reset All Highscores = 0"))
//		{
//            MainMenuManager mm = PlayerPrefManager.getMenuManager();
//
//            foreach (string levelName in mm.LevelNames)
//            {
//                PlayerPrefManager.SetHighscore(levelName,0); 
//            }
//		}
//
//        // add a custom button to the Inspector component
//        if (GUILayout.Button("Lock All Levels"))
//        {
//            MainMenuManager mm = PlayerPrefManager.getMenuManager();
//
//            foreach (string levelName in mm.LevelNames)
//            {
//                PlayerPrefs.SetInt(levelName, 0);
//            }
//        }
//
//        // add a custom button to the Inspector component
//        if (GUILayout.Button("Output Player State"))
//		{
//			// if button pressed, then call function in script
//			PlayerPrefManager.ShowPlayerPrefs();
//		}
//
//        if (GUILayout.Button("Game Summary"))
//        {
//            // if button pressed, then call function in script
//            Debug.Log( PlayerPrefManager.Sumary() );
//        }
//    }
//}
