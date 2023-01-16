using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.Assertions;
using System;

[System.Serializable]
public enum ENUM_BG {
	INTRO,
	MENU,
	SUMMARY,
	FAILED,
}

[System.Serializable]
public class BackgroundMusic {
	public string name;
	public AudioSource audio;
}
	
/*
* SoundManager script is spawned on the first scene of the 
* game and provides the background audio in an uniteruptable 
* service since the script is marked DontDestroyOnLoad. 
* 
* The SoundManager script is hosted on a GameObject with an audio source
* designed to occomodate one shot audio clips from anywhere since the instance 
* is exposed as public static varable.
* 
* In addition the hosting game object also Hosts object with audio sources with 
* looping audio clips designed to be a backgound music for a scene. The SoundManager 
* Subscribes to the SceneManager sceneLoaded event wich enable this script to pair 
* background music particular music with a particular scene
*/
public class SoundManager : MonoBehaviour {

	public static SoundManager instance;

	public static SoundManager getInstance(){
        if (instance == null)
        {
            instance = new SoundManager();
        }
		return instance;
	}

	void OnEnable(){
		SceneManager.sceneLoaded += OnSceneLoaded;
	}

	void OnDisable(){
		SceneManager.sceneLoaded -= OnSceneLoaded;
	}

	// callback
	private void  OnSceneLoaded(Scene aScene, LoadSceneMode aMode){
 		
		string scemeName = aScene.name;

		if (scemeName.StartsWith("Loading") )
		{
			if (playingNow == null || !playingNow.Equals ("playfull")) {
				PlayBackgroundMusic ("playfull");
			}
		} else if (scemeName.StartsWith("MainMenu") )
		{
			if (playingNow == null || !playingNow.Equals ("playfull")) {
				PlayBackgroundMusic ("playfull");
			}
		}else if (scemeName.StartsWith("Puzzle") )
		{
			DiasableAll ();
		}else if (scemeName.StartsWith("GameLose") )
		{
			if (playingNow == null || !playingNow.Equals ("funeral")) {
				PlayBackgroundMusic ("funeral");
			}
		}else
        {
            DiasableAll();
            //PlayBackgroundMusic("playfull");
        }
	
	}


	public string playingNow;

	public BackgroundMusic[] audioBackGrounds;

	private AudioSource _audioSource;

	void Awake(){

		if (instance != null) { 
			// this gameobject must already have been setup in a 
			// previous scene, so just destroy this game object
			Destroy (this.gameObject);
		} else {
			instance = this.GetComponent<SoundManager>();

			_audioSource	= GetComponent<AudioSource>();

			PlayBackgroundMusic (playingNow);

			// keep this gameobject around as new scenes load
			DontDestroyOnLoad(gameObject);
		}

    }


	public void PlayBackgroundMusic(string key){

		DiasableAll ();
		for (int index = 0; index < audioBackGrounds.Length; index++) {
			if ( key.Equals(audioBackGrounds[index].name )){
				StopClip();
				playingNow = key;
				audioBackGrounds[index].audio.mute = false;
				audioBackGrounds[index].audio.Play ();
			}
		}
	}

	public float fadeRate = .2f;
	IEnumerator FadeOut(AudioSource audio) 
	{
		while( audio.volume > 0.1 )
		{
			audio.volume =   audio.volume  - fadeRate * Time.deltaTime ;
			yield return  new WaitForSeconds(.1f);
		}
		audio.volume = 0.0f;
	}

	IEnumerator FadeIn(AudioSource audio)  
	{
		audio.volume = 0;
		while( audio.volume < 0.9 )
		{
			audio.volume =   audio.volume  + fadeRate * Time.deltaTime ;
			yield return  new WaitForSeconds(.1f);
		}
		audio.volume = 1.0f;
	}



	private void DiasableAll(){
		playingNow = null;
		for (int index = 0; index < audioBackGrounds.Length; index++) {
			audioBackGrounds [index].audio.mute = true;
		}
	}
		
	public void PlaySoundOnce(AudioClip sfx, float volumeScale = .25F)
    {
        Assert.IsNotNull(sfx);
        Assert.IsNotNull(_audioSource);
		//try {
            //if (_audioSource == null && getInstance() != null)
            //{
            //    _audioSource = getInstance().gameObject.AddComponent<AudioSource>();
            //}
            _audioSource.PlayOneShot (sfx, volumeScale);
		//} catch(Exception x){
		//}
	}

	public void PlaySoundOnce(AudioClip sfx,Vector3 pos){
		AudioSource.PlayClipAtPoint (sfx, pos);
	}

    public void StopClip()
    {
        _audioSource.Stop();
    }

    public AudioClip buzzer;
    internal void PlayBuzer()
    {
        if(buzzer)  PlaySoundOnce(buzzer);
    }

    public AudioClip bell;
    internal void Bell()
    {
        if (bell) PlaySoundOnce(buzzer);
    }

    public AudioClip betwixed;
    internal void BBetwixed()
    {
        if (betwixed)  PlaySoundOnce(betwixed);
    }
}
