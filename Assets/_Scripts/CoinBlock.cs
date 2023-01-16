#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;



public enum CoinColor {
	RED,GREEN,BLUE,YELLOW, RAND
}

public class CoinBlock : MonoBehaviour {

	public int rows = 2;
	public int cols = 2;
	public float spaceX = 2;
	public float spaceY = 2;
	public CoinColor color;

	public bool dirty = true;

	public GameObject coinPrefab ;

	Dictionary<Vector3,GameObject> mapCoins = new Dictionary<Vector3,GameObject>();

	GameObject CreateGameObject (Vector3 pos){

		GameObject aCoin = null;

		if( ! mapCoins.TryGetValue(pos, out aCoin) ){
			Assert.IsNotNull(coinPrefab);
			Assert.IsNotNull(transform);
			aCoin = GameObject.Instantiate(coinPrefab,transform);
			Assert.IsNotNull(aCoin);
			aCoin.transform.position = pos;
			mapCoins[pos] = aCoin;
		}
		return aCoin;
	}

	void ColorGameObject(GameObject aCoin){

		SpriteRenderer spriteRender = aCoin.GetComponent<SpriteRenderer>();
		if ( color == CoinColor.RAND){
			spriteRender.color = new Color(Random.Range(0f,1f),Random.Range(0f,1f),Random.Range(0f,1f));
		}
		if ( color == CoinColor.RED){
			spriteRender.color = Color.red;
		}
		if ( color == CoinColor.GREEN){
			spriteRender.color = Color.green;
		}
		if ( color == CoinColor.BLUE){
			spriteRender.color = Color.blue;
		}
	}

		

	void Awake(){

		for( int col = 0; col < cols ; col++){
			for( int row = 0; row < rows; row++){
				
				float dx = col * spaceX;
				float dy = row * spaceY;
				Vector3 pos = new Vector3(dx,dy,0) + transform.position;

				GameObject aCoin = CreateGameObject(pos);
				ColorGameObject(aCoin);


			
			}
		}
	}

	public int GetCount(){
		return cols * rows;
	}
    public int GetActiveCount()
    {
        int count = 0;
        foreach (var item in mapCoins)
        {
            GameObject go = item.Value;
            if ( go != null && go.activeInHierarchy)
            {
                count++;
            }
        }
        return count; ;
    }

    public void ClearCoins(){
		foreach(var item in mapCoins)
		{
			GameObject go = item.Value;
			GameObject.DestroyImmediate(go);
		}
		mapCoins.Clear();
	}


	//List<GameObject> coins = new List<GameObject>();
	void OnDrawGizmos() {
		if (dirty){
			for( int col = 0; col < cols ; col++){
				for( int row = 0; row < rows; row++){

					float dx = col * spaceX;
					float dy = row * spaceY;
					Vector3 pos = new Vector3(dx,dy,0) + transform.position;

					GameObject aCoin = CreateGameObject(pos);

					ColorGameObject(aCoin);
				}
			}
			dirty = false;
		}
	}
 
}

#if UNITY_EDITOR
[CustomEditor(typeof(CoinBlock))]
public class CiointBlockEditor : Editor {

	CoinBlock coinBlock ;
	SerializedProperty mapCoins;
	// called when Unity Editor Inspector is updated

	void OnEnable(){

		coinBlock = target as CoinBlock;
		mapCoins = serializedObject.FindProperty("mapCoins");
	}


	public override void OnInspectorGUI()
	{
		DrawDefaultInspector();
		// add a custom button to the Inspector component
		if(GUILayout.Button("clear coins"))
		{
			coinBlock.ClearCoins(); 
			coinBlock.dirty = true;
			Repaint();
		}
	}
}
#endif