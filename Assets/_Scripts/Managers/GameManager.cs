using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;
using UnityEngine.UI; // include UI namespace so can reference UI elements

using sillymutts;

#if UNITY_EDITOR
using UnityEditor;
#endif

/**
 * Singelton Usage, 
 * 
 * Quite the class, whell ....
 * 
 * Manages Pausing the Game
 * Manages Player Inventory
 * Manages the Game Score and updating the related HUD elements
 * Manages the Players Lives and updating the related HUD elements
 * Manages the Players Score
 * Manages updateing the player HUD, score, hi-score, level, lives if applicable, and level message
 * Manages the Player HighScore 
 * 
 * Holds Reference to the Player and Players Spawn Location
 */

[System.Serializable]
public class InventoryItem
{
    public string itemKey;
    public int number;
}

public class GameManager : MonoBehaviour, IInventoryManager ,ICollectPickUp, IScoreManager{

	
	private static GameManager gm;

	public int _InstanceID;

    public int lives = 3;

    float timer = 0;

    LevelDescriptor currentLevelDesciptor = null;


    [Tooltip("Game Paused Panel")]
    public GameObject UIGamePaused;

	[Tooltip("Game Over Scene")]
	public string levelAfterGameOver;

	[Tooltip("If isContinous is true the infinit lives")]
	public bool IsInfinitLives = false;


	[Tooltip("If isContinous is false number of lives")]
	public int startLives = 3;

	//[System.Serializable]
	//public class InventoryItem
	//{
	//	public  string itemKey;
	//	public  int    number;
	//}

	[Tooltip("Tracked Inventory Key and number, generally number starts at 0 or seeded with init n")]
	public InventoryItem[] items;


    // Upper Left
	[Tooltip("Current Score UI Text")]
	public Text UIScore;
	[Tooltip("Inventory/Hint UI Text")]
	public Text UIKeys;

    // Upper Center
	[Tooltip("Level Goal")]
	public Text UIGoal;
    [Tooltip("Time Text")]
    public Text UITimer;
    [Tooltip("ShieldUI Text")]
    public Text UIShield;
    [Tooltip("Health  ")]
    public Text UIHealth;


    // Upper Right
	[Tooltip("Hi Score UI Text")]
	public Text UIHighScore;
	[Tooltip("Current Level UI Text")]
	public Text UILevel;

    // Bottom Left
    public GameObject[] UIExtraLives;




    // RunningScore
    // The RunningScore is Across All Level, this score is carried over
    // from the level to level, score = ScoreLevel1 + ScoreLevel2 + ...
    public static int RunningScore = 0;

    // Hi HiRunningScore
    public static int RunningHiScore = 0;

    // Score for the level
    public static int LevelScore = 0;

    // Hi Score for this Level
    public static int LevelHiScore = 0;

    // Running time for Level
    public static float LevelTime;

	

	

    public bool IsUsingMinMap = false;
    public KeyCode toggleMiniMap;
    public  bool activeMinCamera = false;

    // private variables
    GameObject _player;

	public static  Vector3 playerSpawnLocation;

	public static GameManager getInstance(){

		Assert.IsNotNull(gm);
		return gm;
	}

    GameObject minCamera;
    // set things up here
    void Awake () {
		// setup reference to game manager
		if (gm == null){
     		gm = this.GetComponent<GameManager>();
            setupDefaults();
            _InstanceID = gm.GetInstanceID();
		} else
        {
            Debug.Log("We have GameManager Setup Allready!!!");
        }

		// setup all the variables, the UI, and provide errors if things not setup properly.
		// setupDefaults();

        if (IsUsingMinMap)
        {
            minCamera = GameObject.FindGameObjectWithTag("MiniCamera");
			if (minCamera){
            	minCamera.SetActive(activeMinCamera);
			}
        }

       DontDestroyOnLoad(this.gameObject);
    }

	void OnEnable(){
		SceneManager.sceneLoaded += OnSceneLoaded;

        //GetActiveCoinsStart();

    }

	void OnDisable(){
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}
		
	void  OnSceneLoaded(Scene currentScene, LoadSceneMode aMode){

		string currentSceneName = currentScene.name;

		if ( MenuManager.sLevelDescriptors != null ) {

            // find the LevelDescriptor to the currunt level
			for ( int index = 0; index <  MenuManager.sLevelDescriptors.Length ; index++) {
				if (currentSceneName == MenuManager.sLevelDescriptors [index].sceneName) {
					currentLevelDesciptor = MenuManager.sLevelDescriptors [index];
					break;
				}
			}

            if (currentLevelDesciptor != null)
            {       
                UIGoal.text = currentLevelDesciptor.levelGoal;  ;
                UILevel.text = currentLevelDesciptor.sceneDisplayName;
            }

		}
        /*
        if (currentLevelName == null)
        {
            currentLevelName = currentSceneName;
            UILevel.text = currentLevelName;
        }
        */
        GetActiveCoinsStart();
    }


    public string GetCurrentScene(){
		Assert.AreEqual( SceneManager.GetActiveScene().name , currentLevelDesciptor.sceneName);
		return currentLevelDesciptor.sceneName;
	}


    // Refactor to be DRY
    // no Tuple in unity, what ?
    
    public /*Tuple<int,int>*/ int[] GetActiveCoins()
    {
        int totalValue = 0;
        int totalCoins = 0;

        CoinBlock[] coinBlocks = FindObjectsOfType(typeof(CoinBlock)) as CoinBlock[];
        foreach (CoinBlock aCoinBlock in coinBlocks)
        {
            int coinInThisBlock = aCoinBlock.rows * aCoinBlock.cols;
            totalCoins += coinInThisBlock;
            PickUp pickUpScript = aCoinBlock.coinPrefab.GetComponent<PickUp>();
            totalValue += pickUpScript.pickupValue * coinInThisBlock;

        }
        int[] ret = new int[2];
        return new int[] { totalValue, totalCoins };
    }
    

    public int GetActiveCoinsStart()
    {
        int totalValue = 0;
        int totalCoins = 0;

        CoinBlock[] coinBlocks = FindObjectsOfType(typeof(CoinBlock)) as CoinBlock[];
        foreach (CoinBlock aCoinBlock in coinBlocks)
        {
            int coinInThisBlock = aCoinBlock.GetCount();
            totalCoins += coinInThisBlock;
            PickUp pickUpScript = aCoinBlock.coinPrefab.GetComponent<PickUp>();
            totalValue += pickUpScript.pickupValue * coinInThisBlock;
        }
        //GlobalsDND.Add(GlobalsDND.TOTAL_COIN_CONTIBUTION, totalValue.ToString());
        GlobalsDND.Add(GlobalsDND.TOTAL_COINS_START, totalCoins.ToString());
        return totalCoins;
    }
    public int GetActiveCoinsEnd()
    {
        //int totalValue = 0;
        int totalCoins = 0;

        CoinBlock[] coinBlocks = FindObjectsOfType(typeof(CoinBlock)) as CoinBlock[];
        foreach (CoinBlock aCoinBlock in coinBlocks)
        {
            int coinInThisBlock = aCoinBlock.GetActiveCount();
            totalCoins += coinInThisBlock;
            //PickUp pickUpScript = aCoinBlock.coinPrefab.GetComponent<PickUp>();
            //totalValue += pickUpScript.pickupValue * coinInThisBlock;
        }
       
        //GlobalsDND.Add(GlobalsDND.TOTAL_COIN_END, totalValue.ToString());
        GlobalsDND.Add(GlobalsDND.TOTAL_COINS_END, totalCoins.ToString());
        return totalCoins;
    }

    public string GetNextSceneName(){	 
		return MenuManager.GetNextLevel(currentLevelDesciptor.sceneName).sceneName;

	}
		

	// game loop
	void Update() {
		// TODO : Refactor Game manager
		RefreshGUI();
        ManageHUDTime();
        ManageHUDShield();
        ManageHUDHealth();

        PoolForTogglePause();

        PollForToggleMinMap();

        PollforQuitCondition();
    }

    void PollForToggleMinMap()
    {
        if (Input.GetKeyDown(toggleMiniMap))
        {
            ToggleMinCamera();
        }
    }


    void PoolForTogglePause()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetButton("Escape"))
        {
            TogglePause();

        }
    }

    void ManageHUDHealth()
    {
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            IHealth healthScript = GameObject.FindGameObjectWithTag("Player").GetComponent<IHealth>();
            if (healthScript != null && UIHealth != null)
            {
                float healthPercentage = healthScript.GetHealth();
                //string.Format("{0:0}:{1:00}", healthPercentage);
                UIHealth.text = "Health:" + healthPercentage;

            }
        }
    }

    void ManageHUDShield()
    {
        GameObject shieldScriptContainer = GameObject.FindGameObjectWithTag("ShieldContainer");
        if (shieldScriptContainer)
        {
            ShieldController shieldScript = shieldScriptContainer.GetComponent<ShieldController>();
            if (shieldScript != null && UIShield != null)
            {
                float shield = shieldScript.CurrentShieldPoint;//* 100;
                int shieldInt = (int)shield;
                UIShield.text = "Shield:" + shieldInt + "%";

            }
        }
    }

    void  ManageHUDTime()
    {
        LevelTime += Time.deltaTime;

        if (UITimer != null)
        {
            int minutes = Mathf.FloorToInt(LevelTime / 60F);
            int seconds = Mathf.FloorToInt(LevelTime - minutes * 60);
            string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);
            UITimer.text = "Elapsed Time" + niceTime;
        }
    }

    public void PollforQuitCondition()
    {
        if (Time.timeScale == 0f)
        {
            if (Input.GetKeyDown(KeyCode.X))
            {
                Application.Quit();
            }
            if (Input.GetKeyDown(KeyCode.M))
            {
                UIGamePaused.SetActive(true);
                Time.timeScale = 1f;
                LoadScene("MainMenu");
                //Application.LoadLevel ("MainMenu");
            }
        }
    }

    public void TogglePause()
    {
        if (Time.timeScale > 0f)
        {
            UIGamePaused.SetActive(true); // this brings up the pause UI
            Time.timeScale = 0f; // this pauses the game action
        }
        else
        {
            Time.timeScale = 1f; // this unpauses the game action (ie. back to normal)
            UIGamePaused.SetActive(false); // remove the pause UI
        }
    }

    public void ToggleMinCamera()
    {
        if (minCamera)
        {
            activeMinCamera = !activeMinCamera;
            minCamera.SetActive(activeMinCamera);
        }
    }

    static public void _GoToMainMenu()
    {
        gm.UIGamePaused.SetActive(true);
        Time.timeScale = 1f;
		LoadScene ("MainMenu");
        //Application.LoadLevel("MainMenu");
    }

    static public void _QuitGame()
    {
        Application.Quit();
    }

    public void GoToMainMenu()
    {
        UIGamePaused.SetActive(true);
        Time.timeScale = 1f;
		LoadScene ("MainMenu");
        //Application.LoadLevel("MainMenu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    // setup all the variables, the UI, and provide errors if things not setup properly.
    void setupDefaults() {
		
        
        
        // setup reference to player
		if (_player == null)
			_player = GameObject.FindGameObjectWithTag("Player");
		
		if (_player==null)
			Debug.LogError("Player not found in Game Manager");
		else
		    // get initial _spawnLocation based on initial position of player
		    playerSpawnLocation = _player.transform.position;
 
		if (levelAfterGameOver=="") {
			Debug.LogWarning("levelAfterGameOver not specified, defaulted to current level");
			levelAfterGameOver = SceneManager.GetActiveScene().name;
		}

		// friendly error messages
		if (UIScore==null)
			Debug.LogError ("Need to set UIScore on Game Manager.");
		
		if (UIHighScore==null)
			Debug.LogError ("Need to set UIHighScore on Game Manager.");
		
		if (UILevel==null)
			Debug.LogError ("Need to set UILevel on Game Manager.");
		
		if (UIGamePaused==null)
			Debug.LogError ("Need to set UIGamePaused on Game Manager.");
		
		// get stored player prefs
		RefreshPlayerPrefs();

		// get the UI ready for the game
		RefreshGUI();
	}

	// get stored Player Prefs if they exist, otherwise go with defaults set on gameObject
	void RefreshPlayerPrefs() {

		lives = PlayerPrefManager.GetLives();
 
        string currentScene = SceneManager.GetActiveScene().name;

        //ScoreLevel = PlayerPrefManager.GetScore(currentScene);

        LevelHiScore = PlayerPrefManager.GetHighScore(currentScene);

		// save that this level has been accessed so the MainMenu can enable it
		PlayerPrefManager.UnlockLevel(currentScene);
	}

    // refresh all the GUI element

	/*
	 * Update the score, time, and inventory
	 */
	void RefreshGUI() {

        string currentLevelName = "";
        if (currentLevelDesciptor!=null)
        {
            currentLevelName = currentLevelDesciptor.sceneDisplayName;
        }
		UIScore.text     = currentLevelName + " Score: "     + LevelScore.ToString();
		UIHighScore.text = currentLevelName + " Highscore: " + LevelHiScore.ToString ();
		UILevel.text     = currentLevelName;

        UIKeys.text      = "Keys :" + GetInventory ("key");
		// update shield display
        ShieldController shieldScript = GameObject.FindGameObjectWithTag("Player").GetComponent<ShieldController>();
        if (shieldScript!=null && UIShield!= null)
        {
            float shield = shieldScript.CurrentShieldPoint;
            UIShield.text = "Shield:" + shield  + "%";
        }

		// update health display
        IHealth healthScript = GameObject.FindGameObjectWithTag("Player").GetComponent<IHealth>();
        if (healthScript!=null && UIHealth!=null)
        {
            if (UIShield != null)
            {
                float healthPercentage = healthScript.GetHealthPercent();
                //string.Format("{0:0}:{1:00}", healthPercentage);
                UIHealth.text = "Health:" + healthPercentage + "%";
            }
        }

		// update timer
        if (UITimer)
        {
            int minutes = Mathf.FloorToInt(timer / 60F);
            int seconds = Mathf.FloorToInt(timer - minutes * 60);
            string niceTime = string.Format("{0:0}:{1:00}", minutes, seconds);
            UITimer.text = "Elapsed Time: " + niceTime;
        }

		// update extra lives indicator
        // turn on the appropriate number of life indicators in the UI 
        // based on the number of lives left
		for(int i=0;i<UIExtraLives.Length;i++) {
			if (i<(lives-1)) { // show one less than the number of lives since you only typically show lifes after the current life in UI
				UIExtraLives[i].SetActive(true);
			} else {
				UIExtraLives[i].SetActive(false);
			}
		}

	}

	public void CollectPickUp(string type, int amount, AudioClip sfx) {
		//PlaySound(sfx);  the pick up plays its own sound
        CollectPickUp(type, amount);
	}

    public void CollectPickUp(string type, int amount)
    {
        AddPoints(amount);
    }

    // IScoreManager
    public void AddPoints(int amount)
	{
        FadingTextFactory.CreateFloatingTextScore("+" + amount, GameObject.FindGameObjectWithTag("Player").transform);
        LevelScore += amount;
 
        if (LevelScore > LevelHiScore) {
            LevelHiScore = LevelScore;
        }
        RefreshGUI();
	}

	public int GetPoint(){
		return LevelScore;
	}

	public void RemovePoints(int amount){
        LevelScore -= amount;
        RefreshGUI();
    }
 
    // IInventoryManager
    public void AddInventory(string type, int amount, AudioClip sfx)
    {
        FadingTextFactory.CreateFloatingTextScore("Added " + type + " " + amount, GameObject.FindGameObjectWithTag("Player").transform);
        AddInventory(type, amount);


    }

    public void AddInventory(string type, int amount)
	{
        switch (type)
        {
            case "Health":
                IHealth healthScript = GameObject.FindGameObjectWithTag("Player").GetComponent<IHealth>();
                if (healthScript != null && UIHealth != null)
                {
                    //FadingTextFactory.CreateFloatingTextScore("+ " + amount, GameObject.FindGameObjectWithTag("Player").transform);
                    healthScript.ApplyHeal(amount);
                }
                    break;
            default:
                break;

        }
        //FloatingTextController.AddToScoreFloatingText("Added " + type + " " + amount, GameObject.FindGameObjectWithTag("Player").transform);
        string i = "inventory \n";
		// todo add inventory map, we only have keys currently
		foreach( InventoryItem item in items){
			if ( type == item.itemKey){
				string key = item.itemKey;
                item.number = item.number + amount;
				i = i + key + ": "+ item.number.ToString() + "\n";
			}
		}
		UIKeys.text = i; 
	}

	public void RemoveInventory(string type, int amount)
	{
		foreach( InventoryItem item in items){
			if ( type == item.itemKey){
				 item.number--;
			}
		}

		string i = "inventory \n";
		foreach( InventoryItem item in items){
			string key = item.itemKey;
			if ( type == item.itemKey){
				i = i + key + ": "+ item.number.ToString() + "\n";
			}
		}
		UIKeys.text = i;

	}

	public int GetInventory(string type)
	{
		foreach( InventoryItem item in items){
			if ( type == item.itemKey){
				return item.number;
			}
		}
		return 0;
	}
	 
	// public function to remove player life and reset game accordingly
	public void PlayerHasDied() {
		
        if (IsInfinitLives) {
			RespawnPlayer();
		} else {  
			lives--;
			if (lives <= 0){
				ClearPoolsSaveState();
				GameOver();
			}else{
				RespawnPlayer();
			}
		}
			
	}

	private void ClearPoolsSaveState(){
		timer = 0;
		sillymutts.GameObjectUtility.ClearPools ();
		Scene scene = SceneManager.GetActiveScene();
		PlayerPrefManager.SavePlayerState(scene.name, LevelScore, LevelHiScore, lives);

	}


	private void RespawnPlayer(){
		IRespawn respawn = _player.GetComponent<IRespawn>();
		if (respawn != null)
		{
			respawn.Respawn(GameManager.playerSpawnLocation);//PlayerRespawnLocaton.respawn);
		}else
		{
			string currentSceneName = SceneManager.GetActiveScene().name;
			LoadScene(currentSceneName);
		}
	}
		
	private void GameOver(){
		LevelScore = 0;
		LoadScene(levelAfterGameOver);
	}

	// public function for level complete
	public void LevelCompete() {

        GetActiveCoinsEnd();

        // save the current player prefs before moving to the next level
        PlayerPrefManager.UnlockLevel(SceneManager.GetActiveScene().name);

		PlayerPrefManager.SavePlayerState(SceneManager.GetActiveScene().name, LevelScore,LevelHiScore,lives);
      

        GlobalsDND.Add(GlobalsDND.LAST_LEVEL_TIME2, LevelTime.ToString());
        GlobalsDND.Add(GlobalsDND.LAST_LEVEL_TIME, UITimer.text);

        Assert.IsNotNull(currentLevelDesciptor);
        GlobalsDND.Add(GlobalsDND.LAST_LEVEL, currentLevelDesciptor.sceneDisplayName.ToString());
        GlobalsDND.Add(GlobalsDND.LAST_LEVEL_SCORE, LevelScore.ToString());

        // use a coroutine to allow the player to get fanfare 
        // before moving to next level
        StartCoroutine(LoadNextLevel());
	}

	// load the nextLevel after delay
	IEnumerator LoadNextLevel() {

        PlayerPrefManager.Save();

        yield return new WaitForSeconds(1f);

        currentLevelDesciptor = MenuManager.GetNextLevel(currentLevelDesciptor);

        string nextSceneName = currentLevelDesciptor.sceneName;

        GlobalsDND.Add(GlobalsDND.NEXT_LEVEL, nextSceneName);

        LoadScene("InBetweenLevels");

        LevelScore = 0;
    }

    public void ReloadCurrentLevel()
    {
        LevelScore = 0;
        string currentSceneName = SceneManager.GetActiveScene().name;
        LoadScene(currentSceneName);

    }

    public static void LoadScene(string sceneName){
        Assert.IsNotNull(sceneName);
        PlayerPrefManager.Save();
        getInstance().currentLevelDesciptor = MenuManager.GetLevelDescriptorFromName(sceneName);
        SceneManager.LoadScene (sceneName);
	}

    public void OnDestroy()
    {
        PlayerPrefManager.Save();
    }
}

#if UNITY_EDITOR
[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor { // exten d the Editor class

	// called when Unity Editor Inspector is updated
	public override void OnInspectorGUI()
	{
		// show the default inspector stuff for this component
		DrawDefaultInspector();

		// get a reference to the GameManager script on this target gameObject
		GameManager myGM = (GameManager)target;

		// add a custom button to the Inspector component
		if(GUILayout.Button("Reset Player State"))
		{
			// if button pressed, then call function in script
			PlayerPrefManager.ResetPlayerState(myGM.startLives, false,false);
		}

		// add a custom button to the Inspector component
		if (GUILayout.Button("Reset All Scores = 0"))
		{
			MainMenuManager mm = PlayerPrefManager.getMenuManager();

			foreach (string levelName in mm.LevelNames)
			{
				PlayerPrefManager.SetScore(levelName, 0);
			}
		}

		// add a custom button to the Inspector component
		if (GUILayout.Button("Reset All Highscores = 0"))
		{
			MainMenuManager mm = PlayerPrefManager.getMenuManager();

			foreach (string levelName in mm.LevelNames)
			{
				PlayerPrefManager.SetHighscore(levelName,0); 
			}
		}

		// add a custom button to the Inspector component
		if (GUILayout.Button("Lock All Levels"))
		{
			MainMenuManager mm = PlayerPrefManager.getMenuManager();

			foreach (string levelName in mm.LevelNames)
			{
				PlayerPrefs.SetInt(levelName, 0);
			}
		}

        // add a custom button to the Inspector component
        if (GUILayout.Button("UnLock All Levels"))
        {
            MainMenuManager mm = PlayerPrefManager.getMenuManager();

            foreach (string levelName in mm.LevelNames)
            {
                PlayerPrefs.SetInt(levelName, 1);
            }
        }

        // add a custom button to the Inspector component
        if (GUILayout.Button("Output Player State"))
		{
			// if button pressed, then call function in script
			PlayerPrefManager.ShowPlayerPrefs();
		}

		if (GUILayout.Button("Game Summary"))
		{
			// if button pressed, then call function in script
			Debug.Log( PlayerPrefManager.Sumary() );
		}
	}
}


#endif