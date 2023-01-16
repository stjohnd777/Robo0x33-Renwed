using UnityEngine;
using System.Collections;

public class ProjectLine : MonoBehaviour {


	public Color color;

	public float len = 50;

	public float span = 1;

 
	// Update is called once per frame
	void Update () {
		DrawLine(transform.position,transform.position+ (new Vector3( len, span,0)  ), color);
		DrawLine(transform.position,transform.position+ (new Vector3( len,-span,0) ), color);
	}

	void DrawLine(Vector3 start, Vector3 end, Color color, float duration = .1f)
	{
		GameObject myLine = new GameObject();
		myLine.transform.position = start;
		myLine.AddComponent<LineRenderer>();
		LineRenderer lr = myLine.GetComponent<LineRenderer>();
		lr.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
		lr.SetColors(color, color);
		lr.SetWidth(0.1f, 0.1f);
		lr.SetPosition(0, start);
		lr.SetPosition(1, end);
		GameObject.Destroy(myLine, duration);
	}


	public string[] prejudiceTagList;


	void OnTriggerEnter2D(Collider2D other) {
		if ( IsInPrejudiceTagList ( other.gameObject.tag) ){
			// do some thing;
			Debug.Log("see you");
		}
	}

 

	public bool IsInPrejudiceTagList(string tag){
		bool ret = false;
		foreach( string prejudiceTag in prejudiceTagList){
			if ( tag == prejudiceTag){
				ret = true;
				return ret;
			}
		}
		return ret;
	}
}
