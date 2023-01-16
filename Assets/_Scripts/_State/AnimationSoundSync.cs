using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Animation2Sound
{
    public string anminationName;
    public string animationTag;
    public AudioClip animationSFX;
    public bool active;
    public bool loop;

}
public class AnimationSoundSync : MonoBehaviour {

    public Animation2Sound[] animation2Sound;

    [Header("State")]
    public string OldState;
    public string NewState;

    private Animator animator;
    private AudioSource _audio;

    public string GetAnimationCurrentTag()
    {
        return NewState;
    }

    void Awake()
    {
        animator = GetComponent<Animator>();

        _audio = GetComponent<AudioSource>();
    }

    void Update () {
        SyncSoundAndAnim2();
    }

    void RestState()
    {
        foreach (Animation2Sound t in animation2Sound)
        {
            t.active = false;
        }
    }


    void SyncSoundAndAnim2()
    {
        RestState();
        AnimatorStateInfo animStateInfo = animator.GetCurrentAnimatorStateInfo(0);
        foreach (Animation2Sound t in animation2Sound)
        {
            if (animStateInfo.IsTag(t.animationTag))
            {
                t.active = true;
                NewState = t.animationTag;
                if (OldState != NewState)
                {
                    OldState = t.animationTag;
                    _audio.Stop();
                    PlaySound(t.animationSFX);
                }
            }

        }
    }

    void PlaySound(AudioClip clip)
    {
        if (clip)
        {
            _audio.PlayOneShot(clip);
        }
    }
}
