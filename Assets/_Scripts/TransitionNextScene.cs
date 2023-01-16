using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class TransitionNextScene : MonoBehaviour {

    //delegate void LevelCompleteDelegate( int levelName, int score,int stars);
    //LevelCompleteDelegate notify;

    public bool UseGameManagerForTransition = false;

	//public Texture2D fadeTexture;

	public float transitionTime = 1.0f;

	public string lextLevel ;
 
	public AudioClip  sfxTransition;

	public AudioClip  sfxShallNotPass;

	float alpha = 0.0f;

	[System.Serializable]
	public class ExitCondition
	{
		public  string key;
		public  int number;
	}
	public ExitCondition[] conditions;

 

	void DrawOverlay(float alpha){
		
		//texture.color = new Color(texture.color.r,texture.color.g,texture.color.b,alpha);
	}

	//GUITexture texture;

	//IInventoryManager inventoryManager;

	void Awake(){
		
	    //texture = GameObject.Find("Overlay").GetComponent<GUITexture>();
	
		//inventoryManager = GameObject.FindGameObjectWithTag("Managers").GetComponent<IInventoryManager>();
	}

	// public function for level complete
	public void NextLevel() {
 
		StartCoroutine(LoadNextLevel());
	}

    GameObject subject;
	// if Player hits the stun point of the enemy, then call Stunned on the enemy
	void OnCollisionEnter2D(Collision2D collision)
	{
		if (locked) return;

		if ( collision.gameObject.tag == "Player" )
		{
			subject = collision.gameObject;

			//if (MeetsRequirements())
			//{
			//    FloatingTextController.InfoFloatingText("You Shall Psss", gameObject.transform);
			//    StartCoroutine(LoadNextLevel());
			//}
			//else
			//{
			//    FloatingTextController.InfoFloatingText("You Do Not Have " + required + " " + key, gameObject.transform);
			//    SoundManager.instance.PlaySound(sfxShallNotPass);
			//}

			if (conditions.Length > 0)
			{
				// requirment to transition
				foreach (ExitCondition c in conditions)
				{

					string key = c.key;
					int required = c.number;
					int inInventory = GameManager.getInstance().GetInventory(key);

					if (inInventory >= required)
					{
						FadingTextFactory.CreateFloatingTextInfo("You Shall Pass", gameObject.transform);
						StartCoroutine(LoadNextLevel());
					}
					else
					{

						if (sfxShallNotPass)
						{
							FadingTextFactory.CreateFloatingTextInfo("You Do Not Have " + required + " " + key, gameObject.transform);
							SoundManager.instance.PlaySoundOnce(sfxShallNotPass);
						}
					}


				}
			}
			else
			{
				// no requirment to transition
				FadingTextFactory.CreateFloatingTextInfo("You Shall Psss", gameObject.transform);
				StartCoroutine(LoadNextLevel());
			}
		}

	}

	// Attack player
	void OnTriggerEnter2D(Collider2D collision)
	{
         if (locked) return;

		if (collision.gameObject.tag == "Player" ) 
		{
            subject = collision.gameObject;

            //if (MeetsRequirements())
            //{
            //    FloatingTextController.InfoFloatingText("You Shall Psss", gameObject.transform);
            //    StartCoroutine(LoadNextLevel());
            //}
            //else
            //{
            //    FloatingTextController.InfoFloatingText("You Do Not Have " + required + " " + key, gameObject.transform);
            //    SoundManager.instance.PlaySound(sfxShallNotPass);
            //}

            if (conditions.Length > 0)
            {
                // requirment to transition
                foreach (ExitCondition c in conditions)
                {
 
                    string key = c.key;
                    int required = c.number;
					int inInventory = GameManager.getInstance().GetInventory(key);

                    if (inInventory >= required)
                    {
                        FadingTextFactory.CreateFloatingTextInfo("You Shall Pass", gameObject.transform);
                        StartCoroutine(LoadNextLevel());
                    }
                    else
                    {

                        if (sfxShallNotPass)
                        {
                            FadingTextFactory.CreateFloatingTextInfo("You Do Not Have " + required + " " + key, gameObject.transform);
                            SoundManager.instance.PlaySoundOnce(sfxShallNotPass);
                        }
                    }


                }
            }
            else
            {
                // no requirment to transition
                FadingTextFactory.CreateFloatingTextInfo("You Shall Psss", gameObject.transform);
                StartCoroutine(LoadNextLevel());
            }

        }
	}

    bool MeetsRequirements() {

        bool ret = true;

        if (conditions.Length > 0) {
            foreach (ExitCondition c in conditions) {
                string key = c.key;
                int required = c.number;
				int inInventory = GameManager.getInstance().GetInventory(key);
                if (inInventory < required)
                {
                    ret = false;
                } 
            }
        }

        return ret;
    }

	bool isFading = false;
	int direction = 1;
	// Update is called once per frame
	void FixedUpdate () {
		if ( alpha == 1) {
			isFading = false;
		}
		if ( isFading){
			alpha = alpha +  direction * .01f ;//alpha * Time.deltaTime;
			DrawOverlay(alpha);
		}
	}


    bool locked = false;
	// load the nextLevel after delay
	IEnumerator LoadNextLevel() {

        locked = true;

        IFreezeUnFreeze freeze = subject.GetComponent<IFreezeUnFreeze>();
        if (freeze != null)
        {
            freeze.FreezeMotion();

        }

        IHealth health = subject.GetComponent<IHealth>();
        if ( health != null)
        {
            health.SetImmunityForSec(4);
        }

        IVictory victory = subject.GetComponent<IVictory>();
        if (victory != null)
        {
            victory.Victory();

        }

        if (UseGameManagerForTransition)
        {
            if (sfxTransition)
            {
                SoundManager.instance.PlaySoundOnce(sfxTransition);
            }
			//GameManager.getInstance().levelAfterVictory = lextLevel;
			yield return new WaitForSeconds(1f);
			GameManager.getInstance().LevelCompete();
        }
        else
        {
            isFading = true;
            direction = 1;
            PlayerPrefs.Save();
            yield return new WaitForSeconds(transitionTime);
			SceneManager.LoadScene (lextLevel);
        }
 
    }


	void PlaySound(AudioClip clip)
	{
		SoundManager.instance.PlaySoundOnce(clip);
	}


}
