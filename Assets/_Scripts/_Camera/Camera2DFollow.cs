using System;
using UnityEngine;


public class Camera2DFollow : MonoBehaviour
{
	[Header("The Camera Follow Control")]
	[Tooltip("What the camera is following")]
    public Transform target;
    public float damping = 1;
    public float lookAheadFactor = 3;
    public float lookAheadReturnSpeed = 0.5f;
    public float lookAheadMoveThreshold = 0.1f;

    [Header("Boundry")]
    public bool UseBoundry = false;
    [Space(10)]

//    [Header("Info Camera Bounds")]
//	public Transform llc;
//	public Transform ulc;
//	public Transform urc;
//	public Transform lrc;
//	public Vector3 offset;
//
//        public float xMin;
//        public float xMax;
//        public float yMin;
//        public float yMax;
    [Space(10)]
    public float _cameraMinX;
    public float _cameraMaxX;
    public float _cameraMinY;
    public float _cameraMaxY;
    [Space(10)]

    [Header("Info Camera Dimensions")]
	public float orthoSize;
	public int screenWidth;
	public int screenHieght;
	public int pixelHieght;
	public int pixelWidth;
	public float aspect;


	// private variables
    float m_OffsetZ;
    Vector3 m_LastTargetPosition;
    Vector3 m_CurrentVelocity;
    Vector3 m_LookAheadPos;
	BoxCollider2D boxCollider;


	Camera camera ;


	public float boundryMinX;
	public float boundryMaxX;
	public float boundryMinY;
	public float boundryMaxY;

    // Use this for initialization
    private void Start()
    {
//		// boundry box 
//		boundryMinX = llc.position.x;
//		boundryMinY = llc.position.y;
//		boundryMaxX = urc.position.x;
//		boundryMaxY = ulc.position.y;


		camera = GetComponent<Camera> ();

	    // compute rectangle size of view port/ 2d camera
		screenHieght  = Screen.height;
		screenWidth = Screen.width;
		orthoSize = camera.orthographicSize;
		pixelHieght = camera.pixelHeight;
		pixelWidth = camera.pixelWidth;
		aspect = camera.aspect;
		Vector2 sizeView =  new Vector2 ( 2*orthoSize * aspect , 2*orthoSize);
		     

        m_LastTargetPosition = target.position;

        m_OffsetZ = (transform.position - target.position).z;

        transform.parent = null;

		// if target not set, then set it to the player
		if (target==null) {
			target = GameObject.FindGameObjectWithTag("Player").transform;
		}

		if (target==null)
			Debug.LogError("Target not set on Camera2DFollow.");

	    // set box collider to size view
	    boxCollider = GetComponent<BoxCollider2D> ();
		if (boxCollider == null) {
			gameObject.AddComponent<BoxCollider2D> ();
			boxCollider = GetComponent<BoxCollider2D> ();
			boxCollider.isTrigger = true;
			orthoSize = camera.orthographicSize;
			boxCollider.size = sizeView ;//new Vector2 ( 2*orthoSize * aspect , 2*orthoSize);
		}

        _cameraMinX = boundryMinX + orthoSize;
        _cameraMaxX = boundryMaxX - orthoSize;
        _cameraMinY = boundryMinY + orthoSize * aspect;
        _cameraMaxY = boundryMaxY - orthoSize * aspect;



    }

	public bool disable = false;
	public bool disableX = false;
	public bool disableY = false;

    // Update is called once per frame
	private void Update() {
    
	    // This section computes a rectangle that represents the 
		// camera view port
		screenHieght  = Screen.height;
		screenWidth = Screen.width;
		pixelHieght = camera.pixelHeight;
		pixelWidth = camera.pixelWidth;
		orthoSize = camera.orthographicSize;
		aspect = camera.aspect;
		boxCollider.size = new Vector2 ( 2*orthoSize * aspect , 2*orthoSize);
	

	   // middle mouse wheel to adjust the camera size 2D
		if (camera.orthographic) {
			float factor = 10;
			float delta = Input.GetAxis ("Mouse ScrollWheel");
			camera.orthographicSize =Mathf.Min(120, Mathf.Max(camera.orthographicSize + factor * delta,10));
		}


		// camera x,y,(z) position is in the center of the rectangle(cube), so this camera.x can not
		// be less the the width/2=orthoSize and thus do let camera move to an x pos less
		// the _xMin
        _cameraMinX = boundryMinX + orthoSize;
        _cameraMaxX = boundryMaxX - orthoSize;
        _cameraMinY = boundryMinY + orthoSize * aspect;
        _cameraMaxY = boundryMaxY - orthoSize * aspect;
 

        // only update lookahead pos if accelerating or changed direction,
		// how much has the target we are following moved in one frame in the X axis
        float xMoveDelta = (target.position - m_LastTargetPosition).x;

		// has the target oved enough to validate a compute 
        bool updateLookAheadTarget = Mathf.Abs(xMoveDelta) > lookAheadMoveThreshold;

        if (updateLookAheadTarget)
        {
            m_LookAheadPos = lookAheadFactor * Vector3.right * Mathf.Sign(xMoveDelta);
        }
        else
        {
			m_LookAheadPos = Vector3.MoveTowards(m_LookAheadPos, Vector3.zero, Time.deltaTime*lookAheadReturnSpeed);
        }
			
        Vector3 aheadTargetPos = target.position + m_LookAheadPos + Vector3.forward*m_OffsetZ;

		if (UseBoundry) {
        if (disableX == true)
	        {
				aheadTargetPos.x = camera.transform.position.x;
	        }
			if (disableY == true)
			{
				aheadTargetPos.y = camera.transform.position.y;
			}
		}
			
		Vector3 newPos = Vector3.SmoothDamp(transform.position, aheadTargetPos, ref m_CurrentVelocity, damping) ;//+ offset;

        transform.position = newPos;

        m_LastTargetPosition = target.position;
    }

	void OnTriggerEnter2D(Collider2D collider){

		if (collider.gameObject.tag == "Boundry") {
			disable = true;
		}
		if (collider.gameObject.tag == "BoundryX") {
			disableX = true;
		}
		if (collider.gameObject.tag == "BoundryY") {
			disableY = true;
		}
	}

	void OnTriggerExit2D(Collider2D collider){
		if (collider.gameObject.tag == "Boundry") {
			disable = false;
		}
		if (collider.gameObject.tag == "BoundryX") {
			disableX = false;
		}
		if (collider.gameObject.tag == "BoundryY") {
			disableY = false;
		}
	}

	void OnDrawGizmos() {
		Gizmos.color = new Color(1f,0f,0f,.5f);
		if (camera){
			Gizmos.DrawCube(camera.transform.position, new Vector3( 2*orthoSize * aspect ,2*orthoSize, 1));
		}
	}
}

