using UnityEngine;
using System.Collections;
using UnityEngine.Assertions;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class FadingTextFactory : MonoBehaviour {

	private static string uriPrefabPopupTextDamage 	 = "Prefabs/PopupTextDamage";
	private static string uriPrefabPopupAddToScore 	 = "Prefabs/PopupTextAddToScore";
	private static string uriPrefabPopupAddInventory = "Prefabs/PopupTextAddInventory";
	private static string uriPrefabPopupTextInfo 	 = "Prefabs/PopupTextInfo";

	private static string canvasName = "Canvas-HUD";
    
	private static GameObject canvas;

	private static Dictionary<string, FadingLabel> mapPrefabFadingLabels = new Dictionary<string, FadingLabel>();

	private static void getCanvas(){
		if ( canvas == null){
			canvas = GameObject.Find(canvasName);
		}

		if ( canvas == null){
			GameObject[] gos = SceneManager.GetActiveScene().GetRootGameObjects();
			foreach ( GameObject go in  gos){
				if (  go is Canvas){

					//canvas = (Canvas)go;

					Debug.Log("Found");
					break;
				}
					
			}
			//canvas =  GameObject.FindObjectOfType<Canvas>();
		}
		Assert.IsNotNull(canvas);
	}


	private static void createFadeLabel(string prefabUri,Transform location,string text){

		FadingLabel prefab = null;

		if ( mapPrefabFadingLabels.TryGetValue(prefabUri, out prefab) ){

		}else{
			prefab = Resources.Load<FadingLabel>(prefabUri);
			Assert.IsNotNull(prefab);
			mapPrefabFadingLabels[prefabUri] = prefab;
		}
		Assert.IsNotNull(prefab);

		FadingLabel instance = Instantiate(prefab);
		instance.transform.SetParent(canvas.transform, false);
		Vector2 screenPos = Camera.main.WorldToScreenPoint(location.position);
		instance.transform.position = screenPos;
		instance.SetText(text);
	}

	public static void CreateFloatingTextDamage(string text,Transform location)
	{
	    getCanvas();
		createFadeLabel(uriPrefabPopupTextDamage,location,text);

	}

	/*
	 * used for adding to the score
	 */ 
    public static void CreateFloatingTextScore(string text, Transform location)
    {
		getCanvas();
		createFadeLabel(uriPrefabPopupAddToScore,location,text);

    }

	/*
	 *  Used for adding inventory
	 */ 
    public static void CreateFloatingTextInventory(string text, Transform location)
    {
		getCanvas();
		createFadeLabel(uriPrefabPopupAddInventory,location,text);

    }

	/*
	 *  Used for a general message 
	 */
    public static void CreateFloatingTextInfo(string text, Transform location)
    {
		getCanvas();
		createFadeLabel(uriPrefabPopupTextInfo,location,text);

    }
}
