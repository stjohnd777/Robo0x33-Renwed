#if UNITY_EDITOR
using UnityEditor;
#endif

using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Assertions;
using UnityEngine.UI;



public class DelayedSceneLoader : MonoBehaviour {

	
	public float delay = 2f;

    [Tooltip("Name")]
    public Text lastLevelNameText;

    [Tooltip("Score")]
    public Text scoreLastLevelText;

    [Tooltip("Time")]
    public Text timeLastLevelText;

    [Tooltip("Total Coins")]
    public Text coinsTotal;

    [Tooltip("Missed Coins")]
    public Text coinsMissed;

    public Image star1;
    public Image star2;
    public Image star3;



    public bool isGetSceneFromGameManager = false;

    [HideInInspector]
    public string levelToLoad;

	void Awake()
	{
		if (isGetSceneFromGameManager){

			levelToLoad = GlobalsDND.Get(GlobalsDND.NEXT_LEVEL );

            lastLevelNameText.text = GlobalsDND.Get(GlobalsDND.LAST_LEVEL);

            scoreLastLevelText.text = "Score : " + GlobalsDND.Get(GlobalsDND.LAST_LEVEL_SCORE);

            timeLastLevelText.text = GlobalsDND.Get(GlobalsDND.LAST_LEVEL_TIME);

            coinsTotal.text = "Total Coins:" + GlobalsDND.Get(GlobalsDND.TOTAL_COINS_START);
            coinsMissed.text = "Coins Missed:" + GlobalsDND.Get(GlobalsDND.TOTAL_COINS_END);

            float total = float.Parse(GlobalsDND.Get(GlobalsDND.TOTAL_COINS_START));
            float missed = float.Parse(GlobalsDND.Get(GlobalsDND.TOTAL_COINS_END));

            float grade = (total - missed) / total;

            if ( grade < .50)
            {
                Destroy(star2); 
                Destroy(star3);

            }
            else if (grade > .50 && grade < .80)
            {
                Destroy(star3);
            }
            else if (grade > 80 )
            {

            }



        }
		Assert.IsNotNull(levelToLoad);
        Invoke("LoadLevel", delay);
    }


    // load the specified level
    public void LoadLevel() {
        sillymutts.GameObjectUtility.ClearPools();
		SceneManager.LoadScene(levelToLoad,LoadSceneMode.Single);
	}
	void LoadLevel(string name ) {
        sillymutts.GameObjectUtility.ClearPools();
        SceneManager.LoadScene(name,LoadSceneMode.Single);
	}
}

#if UNITY_EDITOR
[CustomEditor(typeof(DelayedSceneLoader))]
class DelayedSceneLoaderEditor : Editor
{
    DelayedSceneLoader dsl;
    public void OnEnable()
    {
        dsl = target as DelayedSceneLoader;
    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        base.OnInspectorGUI();

        if (!dsl.isGetSceneFromGameManager)
        {
            dsl.levelToLoad = EditorGUILayout.TextField("Scene Name", dsl.levelToLoad);
        } else
        {
            EditorGUILayout.HelpBox("Scene Pulled From GameManager", MessageType.Info);
        }

        serializedObject.ApplyModifiedProperties();
    }

}

#endif


