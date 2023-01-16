using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public enum Type
{
    game, tutorial
}

[System.Serializable]
public class LevelDescriptor
{
    //public int    ordinal;
	[Tooltip("The Actual Scene Name need to load the Level")]
    public string sceneName;
	[Tooltip("The Display NameIn the HUD")]
    public string sceneDisplayName;
	[Tooltip("A description about the level")]
    public string levelDescription;
	[Tooltip("The Player Goals")]
    public string levelGoal;
	[Tooltip("Max score achievable in this level, used for star rating")]
    public int levelMaxScore;
	[Tooltip("Is there a time limit")]
    public bool hasTimeLimit;
	[Tooltip("Thetime limit if this level has time limit")]
    public int timeLimit;
	[Tooltip("BG image")]
    public Image btnImage;
	[Tooltip("Btn text in level select")]
    public string btnText;
	[Tooltip("Tutorial or Game")]
    public Type type;
	[Tooltip("BG music")]
    public string bgMusics;
	[Tooltip("Requiremnt keys")]
	public int keys;


    public LevelDescriptor(LevelDescriptor other)
    {
        sceneName = other.sceneName;
        sceneDisplayName = other.sceneDisplayName;
        levelDescription = other.levelDescription;
        levelGoal = other.levelGoal;
        levelMaxScore = other.levelMaxScore;
        hasTimeLimit = other.hasTimeLimit;
        timeLimit = other.timeLimit;
        btnImage = other.btnImage;
        btnText = other.btnText;
        type = other.type;
        bgMusics = other.bgMusics;
		keys = other.keys;
    }

}
