using UnityEngine;
using System.Collections;
using TSA;

public class PlayerMovment : MonoBehaviour {

    
    public float p_CameraZoomInY;
    Vector3 CameraZoomIn;
    public float p_CameraDefaultZoomY;
    Vector3 CameraDefaultZoom;
    public float p_CameraDemoSpeed;
    public float p_CameraZoomSpeed;
    Vector3 CamTarget;

    public float p_SprintSpeed;
    public float p_BaseSpeed; 
    float Speed; //difference in player Speed from base
    float CurrentSpeed; 
    float MoveDown = 0f; //velocity caused by p_Gravity
    public float p_Gravity = 9.8f;
    bool IsSprinting = false;
    bool PlayerControlsCamera = true;

    public RectTransform UICanvas;
    Transform SoundBounds; 
    CharacterController cc;
    
    
    /*TODO
    expand functionality with statemachine
    */


    // Use this for initialization
    void Start ()
    {
        Vector3 trans = this.transform.position;
        SoundBounds = this.transform.GetChild(0);
        
        Camera.main.transform.position = new Vector3(trans.x, trans.y + p_CameraDefaultZoomY, trans.z);
        Camera.main.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));

        p_CameraZoomInY = Camera.main.transform.position.y + p_CameraZoomInY;
        p_CameraDefaultZoomY = Camera.main.transform.position.y + p_CameraDefaultZoomY;

        cc = this.GetComponent<CharacterController>();
    }

    void OnEnable()
    {
        ResourceManager.playerFound += OnPlayerFound;
    }

    void OnDisable()
    {
        ResourceManager.playerFound -= OnPlayerFound;
    }
	
	
 
    void Update()
    {
        Vector3 camPos = Camera.main.transform.position;
        Vector3 trans = this.transform.position;
        CurrentSpeed = Speed + p_BaseSpeed;  
              
        IsSprinting = Input.GetButton("sprint") ? true : false;

        if (IsSprinting == true)
        { SprintSpeedAdjust(trans);}
        else { WalkSpeedAdjust(); }

        if (PlayerControlsCamera == true && IsSprinting == false)
        {
            SetDefaultZoom(trans);
            CamTarget = CameraDefaultZoom;
        }
                   
        ApplyMovement();
        AdjustCamera(camPos, trans);

        if (Input.GetButtonDown("Jump"))
        {SoundBounds.position = trans;}
        else { SoundBounds.position = ResourceManager.OutOfBounds; }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }


    }


    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "CameraTrigger")
        {
            PlayerControlsCamera = false;
            Vector3 target = col.transform.parent.GetChild(1).transform.position;
            CamTarget = target;
        }
        else {

            PlayerControlsCamera = true;
        }
    }

    //---event functions-------------

    void OnPlayerFound()
    {
        this.GetComponent<PlayerMovment>().enabled = false;
    }


    //-----functions-----------------
    void ApplyMovement()
    {
        float moveHorizontal = 0f;
        float moveVertical = 0f;
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");

        if (cc.isGrounded)
        {          
            MoveDown = 0;
        }
        else
        {
            MoveDown -= p_Gravity * Time.deltaTime;
        }
        
        Vector3 movement = new Vector3(moveHorizontal, MoveDown, moveVertical);
        cc.Move((movement * Speed));
    }


    void AdjustCamera(Vector3 camPos, Vector3 trans)
    {
        float camSpeed = 0;
        float zoomSpeed = 1;
        if (IsSprinting == false)
            camSpeed = p_BaseSpeed;
        if (IsSprinting == true)
            camSpeed = p_SprintSpeed;
        if (Mathf.RoundToInt(camPos.x) == Mathf.RoundToInt(trans.x))          
            zoomSpeed = p_CameraZoomSpeed;

        Vector3 velocity = (CamTarget - camPos) * (camSpeed);
        
        Camera.main.transform.position += new Vector3(velocity.x, (velocity.y) * zoomSpeed, velocity.z);
        return;        
    }


    void WalkSpeedAdjust()
    {
        if (Speed > p_BaseSpeed)
        { Speed -= .1f; }
        else if (Speed < p_BaseSpeed)
        { Speed = p_BaseSpeed; }
    }

    void SprintSpeedAdjust(Vector3 trans)
    {
        if (Speed < p_SprintSpeed)
        {
            Speed += .1f;
        }

        if (PlayerControlsCamera == true)
            SetCameraZoomIn(trans);
    }

    void SetDefaultZoom(Vector3 trans)
    {
        CameraDefaultZoom.x = trans.x;
        CameraDefaultZoom.y = p_CameraDefaultZoomY; 
        CameraDefaultZoom.z = trans.z;

        CamTarget = CameraDefaultZoom;
    }

    void SetCameraZoomIn(Vector3 trans)
    {
        CameraZoomIn.x = trans.x;
        CameraZoomIn.y = p_CameraZoomInY; 
        CameraZoomIn.z = trans.z;

        CamTarget = CameraZoomIn;
    }
}
