using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;


[System.Serializable]
public class UserKeys
{
    public readonly static string UNLOCKED   = "Unlocked";
    public readonly static string HIGH_SCORE = "Highscore";
    public readonly static string MinTimeElapsed = "ElapsedTime";
    public readonly static string SCORE       = "Score";
    public readonly static string TimeElapsed = "ElapsedTime";

    public readonly static string LIVES      = "Lives";
	public readonly static string TIME      = "Time";
}


public static class PlayerPrefManager {

	public static int GetLives() {
		if (PlayerPrefs.HasKey(UserKeys.LIVES)) {
			return PlayerPrefs.GetInt(UserKeys.LIVES);
		} else {
			return 0;
		}
	}

	public static void SetLives(int lives) {
		PlayerPrefs.SetInt(UserKeys.LIVES, lives);
	}

	public static int GetTime(string levelName) {
		string key = levelName + "_" + UserKeys.TIME;
		if (PlayerPrefs.HasKey(key)) {
			return PlayerPrefs.GetInt(key);
		} else {
			return 0;
		}
	}

	public static int GetScore(string levelName) {
        string key = levelName + "_" + UserKeys.SCORE;
        if (PlayerPrefs.HasKey(key)) {
			return PlayerPrefs.GetInt(key);
		} else {
			return 0;
		}
	}

	public static void SetTime(string levelName, int time) {
		string key = levelName + "_" + UserKeys.TIME;
		PlayerPrefs.SetInt(key,time);
	}


	public static void SetScore(string levelName, int score) {
        string key = levelName + "_" + UserKeys.SCORE;
        PlayerPrefs.SetInt(key,score);
	}

	public static int GetHighScore(string levelName) {
        string key = levelName + "_" + UserKeys.HIGH_SCORE;

        if (PlayerPrefs.HasKey(key)) {
			return PlayerPrefs.GetInt(key);
		} else {
			return 0;
		}
	}

	public static void SetHighscore(string levelName, int highscore) {
        string key = levelName + "_" + UserKeys.HIGH_SCORE;
        PlayerPrefs.SetInt(key,highscore);
	}


    // story the current player state info into PlayerPrefs
    public static void Save()
    {
        string currentLevelName = SceneManager.GetActiveScene().name;    
        SetScore(currentLevelName, GameManager.LevelScore);
        SetHighscore(currentLevelName, GameManager.LevelHiScore);
		SetTime(currentLevelName,(int)GameManager.LevelTime);
		if( GameManager.getInstance() != null){
			SetLives(GameManager.getInstance().lives);
		}
    }

    public static void SavePlayerState(string levelName, int score, int highScore, int lives) {
        PlayerPrefs.SetInt(UserKeys.LIVES, lives);
        PlayerPrefs.SetInt(levelName + "_" + UserKeys.SCORE, score);
        PlayerPrefs.SetInt(levelName + "_" + UserKeys.HIGH_SCORE, highScore);
	}
	
	// reset stored player state and variables back to defaults
	public static void ResetPlayerState(int startLives, bool resetHighscore, bool relock) {

        Debug.Log ("Player State reset.");

        PlayerPrefs.SetInt(UserKeys.LIVES, startLives);
        MainMenuManager menueManager = getMenuManager();

        foreach (string levelName in menueManager.LevelNames)
        {
            SetScore(levelName,0);
        }

        if (resetHighscore)
        {
            foreach (string levelName in menueManager.LevelNames)
            {
                SetHighscore(levelName, 0);
            }
        }

        if (relock)
        {
            foreach (string levelName in menueManager.LevelNames)
            {
                PlayerPrefs.SetInt(levelName, 0);
            }
        }
    }

	//// store a key for the name of the current level to indicate it is unlocked
	//public static void UnlockLevel() {	
	//	PlayerPrefs.SetInt(Application.loadedLevelName,1);
	//}

	// store a key for the name of the current level to indicate it is unlocked
	public static void UnlockLevel(string level) {	
		PlayerPrefs.SetInt(level,1);
	}
	// determine if a levelname is currently unlocked (i.e., it has a key set)
	public static bool IsLevelUnlocked(string levelName) {
		return PlayerPrefs.GetInt(levelName) == 1 ? true : false; 
	}

	// output the defined Player Prefs to the console
	public static string ShowPlayerPrefs() {

        MainMenuManager mm = getMenuManager();

        string prefs = "Player Prefs:\n";

        foreach (string levelName in mm.LevelNames)
        {
            int hiScore = GetHighScore(levelName);
            int score = GetScore(levelName);
            bool isUnlocked = IsLevelUnlocked(levelName);
            prefs += "Level: " + levelName + "\n";
            prefs += "\t HiScore:" + hiScore + "\n";
            prefs += "\t Score:" + score + "\n";
            prefs += "\t Unlocked:" + isUnlocked + "\n";
        }
        prefs += "Lives: " + " = " + PlayerPrefs.GetInt("Lives") + "\n";
        Debug.Log(prefs);
        return prefs;
	}


    public static void DanRestAll(string predicate, int number)
    {
        for (int i = 0; i < number; i++)
        {
            string levelName = predicate + number;
            SetScore(levelName, 0);
            SetHighscore(levelName, 0);
            PlayerPrefs.SetInt(levelName, 0);
        }
    }

    public static string Sumary()
    {
        string summary = "Summay\n" ;

        MainMenuManager mm = getMenuManager();

        foreach (string levelName in mm.LevelNames)
        {
            summary += levelName + ": score" + PlayerPrefs.GetInt(levelName + "_" + "Score") + " Hi:" + PlayerPrefs.GetInt(levelName + "_" + "Highscore") +"\n";
        }
        
        return summary;
    }

    public static MainMenuManager getMenuManager()
    {
        MainMenuManager mm;
        if (MainMenuManager.mm == null)
        {
            mm = Resources.Load<MainMenuManager>("Prefabs/MenuManager");
        }
        else
        {
            mm = MainMenuManager.mm;
        }
        return mm;
    }
}
 