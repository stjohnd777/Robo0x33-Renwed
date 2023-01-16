using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
public class MenuButtonLoadLevel : MonoBehaviour {

	public void loadLevel(string leveltoLoad)
	{
		SceneManager.LoadScene (leveltoLoad);
	}


    public void loadLastLevel()
    {
        SceneManager.LoadScene(GlobalsDND.Get(GlobalsDND.NEXT_LEVEL));
    }
}
