using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // include EventSystems namespace so can set initial input for controller support
using UnityEngine.SceneManagement;


using sillymutts;
public enum MenuWindows {
	Splash,
	Loading,
	MainManu,
	LevelSelect,
	GamePaused,
	GameOver,
	GameWin,
	BetweenLevels,
	TutorialSelect,
	About
}

public class MenuManager : MonoBehaviour {


	public static MenuManager instance;
	public static LevelDescriptor[] sLevelDescriptors;

    public static LevelDescriptor GetLevelDescriptorByOrdnal(int i)
    {
        return sLevelDescriptors[i];
    }

    public static int GetLevelsOrdnalFromName(string name)
    {
        for(int i =0; i < sLevelDescriptors.Length; i++)
        {
            LevelDescriptor l = sLevelDescriptors[i];
            if (l.sceneDisplayName.Equals(name))
            {
                return i;
            }
        }
        return -1;
    }

    public static LevelDescriptor GetLevelDescriptorFromName(string name)
    {
        for (int i = 0; i < sLevelDescriptors.Length; i++)
        {
            LevelDescriptor l = sLevelDescriptors[i];
            if (l.sceneDisplayName.Equals(name))
            {
                return l;
            }
        }
        return null;
    }

    public static LevelDescriptor GetNextLevel(string currentLevelName)
    {
        int index = GetLevelsOrdnalFromName(currentLevelName);
        return sLevelDescriptors[index + 1];
    }

    public static LevelDescriptor GetNextLevel(LevelDescriptor currentLevelDescriptor)
    {

        int index = GetLevelsOrdnalFromName(currentLevelDescriptor.sceneDisplayName);
        return sLevelDescriptors[index + 1];
    }


    public static LevelDescriptor[] sTutorialNames;

	public GameObject MainMenuDefaultButton;
	public GameObject LevelSelectButton;
	public GameObject TutorialDefaultButton;
	public GameObject AboutDefaultButton;
    



    [Header("Major Windows")]
	public GameObject MainMenuWindow;
	public GameObject LevelSelectWindow;
	public GameObject TutorialWindow;
	public int levelsPerPage = 12;
	public GameObject LevelsPanel;
	public GameObject TutorialsPanel;
	public GameObject AboutWindow;
    public GameObject Credits;

	public bool OpenAllLevelsOverride = true;
 
	public static MenuManager mm;

	public static MenuManager getInstance(){
		return instance;
	}
 
	void Awake()
	{

		instance = this.GetComponent<MenuManager>();
		currentLevelPage = 0;
		currentTutorialPage = 0;
		LoadLevelsPage();
		LoadTutorialPage ();
		ShowMenu ("MainMenu");
	
		EventSystem.current.SetSelectedGameObject ( MainMenuDefaultButton);

		sLevelDescriptors = LevelDescriptors;
		sTutorialNames = TutorialNames;

	}

	// Show the proper menu
	public void ShowMenu(string name)
	{
		// turn all menus off
		MainMenuWindow.SetActive (false);
		LevelSelectWindow.SetActive(false);
		TutorialWindow.SetActive(false);
		AboutWindow.SetActive(false);
        Credits.SetActive(false);

        // turn on desired menu and set default selected button for controller input
        switch (name) {
		case "MainMenu":
			MainMenuWindow.SetActive (true);
			EventSystem.current.SetSelectedGameObject (MainMenuDefaultButton);
			break;
		case "LevelSelect":
			LevelSelectWindow.SetActive(true);
			EventSystem.current.SetSelectedGameObject (LevelSelectButton);
			break;
		case "TutorialSelect": // DSJ
			TutorialWindow.SetActive(true);
			EventSystem.current.SetSelectedGameObject (TutorialDefaultButton);
			break;
		case "About":
			AboutWindow.SetActive(true);
			EventSystem.current.SetSelectedGameObject (AboutDefaultButton);
			break;

        case "Credits":
            Credits.SetActive(true);
            EventSystem.current.SetSelectedGameObject(AboutDefaultButton);
            break;
        }
	}

    public void ShowCredits()
    {
        Credits.SetActive(true);
        EventSystem.current.SetSelectedGameObject(AboutDefaultButton);
    }

	public GameObject LevelButtonPrefab;

	[Header("level")]
	public LevelDescriptor[] LevelDescriptors;

	[Header("tutorials")]
	public LevelDescriptor[] TutorialNames;
 
	public int currentLevelPage;
	public int currentLevel;

	public int currentTutorialPage;
	public int currentTutorial;



	public void LoadPreviousLevelPage(){
		foreach (Transform child in LevelsPanel.transform) {
			GameObject.Destroy (child.gameObject);
		}
		currentLevelPage--;
		LoadPage (LevelsPanel, currentLevelPage, LevelDescriptors);//LevelNames);
	}

	public void LoadLevelsPage(){
		foreach (Transform child in LevelsPanel.transform) {
			GameObject.Destroy (child.gameObject);
		}
		LoadPage (LevelsPanel, currentLevelPage, LevelDescriptors);//LevelNames);
	}

	public void LoadNextLevelsPage(){
		foreach (Transform child in LevelsPanel.transform) {
			GameObject.Destroy (child.gameObject);
		}
		currentLevelPage++;
		LoadPage (LevelsPanel, currentLevelPage, LevelDescriptors);//LevelNames);
	}

    /// <summary>
    /// Loads the previous level page.
    /// </summary>
	/// 
	public void LoadPreviousTutorialPage(){
		foreach (Transform child in TutorialsPanel.transform) {
			GameObject.Destroy (child.gameObject);
		}
		currentTutorialPage--;
		LoadPage (TutorialsPanel,currentLevelPage,TutorialNames,true);
	}
	public void LoadTutorialPage(){
		foreach (Transform child in TutorialsPanel.transform) {
			GameObject.Destroy (child.gameObject);
		}
		LoadPage (TutorialsPanel,currentTutorialPage,TutorialNames, true);
	}
	public void LoadNextTutorialPage(){
		foreach (Transform child in TutorialsPanel.transform) {
			GameObject.Destroy (child.gameObject);
		}
		currentTutorialPage++;
		LoadPage (TutorialsPanel,currentLevelPage,TutorialNames, true);
	}


	void LoadPage(GameObject panel, int page, LevelDescriptor[] levels, bool openAll = false) {
 
		if (page < 0) {
			page = 0;
			currentLevelPage = page;
		}

		int maxPage = (int)Mathf.Floor( (float)levels.Length / (float)levelsPerPage) ;
		if (page > maxPage) {
			page = maxPage;
			currentLevelPage = page;
		}
		

		// loop through each levelName defined in the editor
		for( int j = 0, i= page * levelsPerPage ; (i < levels.Length) &&  (j< 12); i++, j++) {
			 

			LevelDescriptor levelDesc = levels[i];
            string levelName = levelDesc.sceneName;
            int levelHiScore = PlayerPrefManager.GetHighScore(levelName);
            bool isLevelLocked = ! PlayerPrefManager.IsLevelUnlocked(levelName);

            // dynamically create a button from the template
            GameObject levelButton = Instantiate(LevelButtonPrefab,Vector3.zero,Quaternion.identity) as GameObject;

			// name the game object
			levelButton.name = levelDesc.sceneName+"-Btn";

			// set the parent of the button as the LevelsPanel so it will be dynamically arrange based on the defined layout
			levelButton.transform.SetParent(panel.transform,false);

            levelButton.SetActive(true);

            // get the Button script attached to the button
            Button button = levelButton.GetComponent<Button>();

			// setup the listener to loadlevel when clicked
			button.onClick.RemoveAllListeners();
        
			button.onClick.AddListener(() =>LoadLevel(levelDesc.sceneName));

			LevelBtn levelBtnScript = levelButton.GetComponentInChildren<LevelBtn> ();

            levelBtnScript.ordinal = i;

            levelBtnScript.levelDesc = new LevelDescriptor(levelDesc);


            // determine if the button should be interactable based on if the level is unlocked
            //bool isUnLocked = PlayerPrefManager.LevelIsUnlocked (levelDesc.sceneName);
            if (isLevelLocked)
            {
                levelBtnScript.isLocked = true;
                levelBtnScript.score = 0;
                levelBtnScript.stars = 0;
                button.interactable = false;
            }else
            {
                button.interactable = true;
                levelBtnScript.isLocked = false;
                int hiScore = PlayerPrefManager.GetHighScore(levelDesc.sceneName);
                levelBtnScript.hiscore = hiScore;
                int maxScore = levelDesc.levelMaxScore;
                float grade = (float)hiScore / (float)maxScore;
                if (grade < .5)
                {
                    levelBtnScript.stars = 1;
                }
                else if (grade >= .5 && grade < .75)
                {
                    levelBtnScript.stars = 2;
                }
                else
                {
                    levelBtnScript.stars = 3;
                }
            }


			if (openAll || OpenAllLevelsOverride) {
				levelBtnScript.isLocked = false;
				button.interactable = true;
			}


			if (i == 0) {
				button.interactable = true;
                levelBtnScript.isLocked = false;
            }
            levelBtnScript.UpdateButtonData();
		}
	}


	public void NewGame(){
		PlayerPrefManager.ResetPlayerState(3,false,false);
		SceneManager.LoadScene (LevelDescriptors[0].sceneName);
	}


	// load the specified Unity level
    // TODO : delegate the loading scens to GameMnager
	public void LoadLevel(string leveltoLoad)
	{
        if (PlayerPrefManager.IsLevelUnlocked(leveltoLoad))
        {
            PlayerPrefManager.ResetPlayerState(3, false, false);
            GameObjectUtility.ClearPools();
            SceneManager.LoadScene(leveltoLoad);
        }else
        {
            SoundManager.getInstance().PlayBuzer();
        }

	}

	// quit the game
	public void QuitGame()
	{
		Application.Quit ();
	}


}
