using UnityEngine;
using System.Collections;

public class TimedFlipX : MonoBehaviour {

	public GameObject agent;

	public float delay = 2.5f;

	public bool IsUseSoundManager = true;

	public AudioClip sfx;

	AudioSource _audioSource ;

	void Start () {
	
		_audioSource = GetComponent<AudioSource> ();

		StartCoroutine(Flip(delay));
	}
	
 
	IEnumerator Flip(float wait){

		yield return new WaitForSeconds (wait);

		agent.transform.localScale = new Vector3 ( - agent.transform.localScale.x,agent.transform.localScale.y,agent.transform.localScale.z);

		PlaySound (sfx);

		StartCoroutine(Flip(delay));
	}


	void PlaySound(AudioClip sfx){

		if ( sfx!= null){
			if ( IsUseSoundManager ){
				SoundManager.instance.PlaySoundOnce(sfx);
			} else {
				if (_audioSource != null){
					_audioSource.PlayOneShot(sfx);
				}
			}
		}
	}

}
