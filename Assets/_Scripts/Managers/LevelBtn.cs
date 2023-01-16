using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LevelBtn : MonoBehaviour {

    public int ordinal;
    
    public int hiscore = 0;
    public int score = 0;
	public int stars = 0;

    public bool   isLocked;

	public Text  buttonText;
	public Text  scoreText;

	public Image locked;

	public Image star1;
	public Image star2;
	public Image star3;
    public Image hint;

    public LevelDescriptor levelDesc;

    public void  Listen()
    {
        StartCoroutine(update_btn());
    }
    private IEnumerator update_btn()
    {
        while (true)
        {
            yield return new WaitForSeconds(1f);
            if (levelDesc != null)
            {
                isLocked = !PlayerPrefManager.IsLevelUnlocked(levelDesc.sceneName);
                hiscore = PlayerPrefManager.GetHighScore(levelDesc.sceneName);
                score = PlayerPrefManager.GetScore(levelDesc.sceneName);
                UpdateButtonData();
            }
        }
    }


    public void UpdateButtonData () {

		buttonText.text = levelDesc.btnText;

		if (isLocked == true) {
			
			locked.gameObject.SetActive (true);

			star1.gameObject.SetActive (false);
			star2.gameObject.SetActive (false);
			star3.gameObject.SetActive (false);
			scoreText.text = "locked";

		} else {

            locked.gameObject.SetActive(false);
            scoreText.text = "" + hiscore;

			if (stars == 1) {
				star1.gameObject.SetActive (true);
			}

			if (stars == 2) {
				star1.gameObject.SetActive (true);
				star2.gameObject.SetActive (true);
			}

			if (stars == 3) {
				star1.gameObject.SetActive (true);
				star2.gameObject.SetActive (true);
				star3.gameObject.SetActive (true);
			}
		}
        //Listen();
    }

}
