using UnityEngine;
using System.Collections;
using TSA;

public class PlayerMovment : MonoBehaviour {

    public float baseSpeed; //default walk speed
    public float cameraZoomIn; //camera zoomed y value
    public float cameraDefaultZoom; //camera zoomed out value
    public RectTransform UICanvas;
    float cameraZoom; //current  camera y value
    float speed; //difference in speed from base
    float currentSpeed; //current speed value
    float moveDown = 0f; //velocity caused by gravity
    public float gravity = 9.8f;
    Vector3 trans; //store shorthand refrence to this objects position
    Transform SoundBounds; //store refrence to child object
    CharacterController cc;
    
    
    /*TODO
    expand functionality with statemachine
    */


    // Use this for initialization
    void Start ()
    {
        trans = this.transform.position;
        SoundBounds = this.transform.GetChild(0);
        
        Camera.main.transform.position = new Vector3(trans.x, trans.y + cameraDefaultZoom, trans.z);
        Camera.main.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));

        cameraZoomIn = Camera.main.transform.position.y + cameraZoomIn;
        cameraDefaultZoom = Camera.main.transform.position.y + cameraDefaultZoom;

        cc = this.GetComponent<CharacterController>();
    }
	
	
 
    void Update()
    {
        Vector3 camPos = Camera.main.transform.position;
        trans = this.transform.position;
        currentSpeed = speed + baseSpeed;

        ApplyMovement();
        AdjustSpeed();

        if (Input.GetButton("sprint"))
        {
            cameraZoom = cameraZoomIn;
            if (speed < 1)
            {
                speed += .1f;
            }
        }else
        { cameraZoom = cameraDefaultZoom; }       

        
        if (Input.GetButtonDown("Jump"))
        {
            SoundBounds.position = trans;
        }
        else { SoundBounds.position = ResourceManager.OutOfBounds; }

        Camera.main.transform.position = new Vector3(trans.x, AdjustCamera(camPos), trans.z);
    }


    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == "Lethal")
        {
            ResourceManager.CallplayerFound();
            Instantiate(UICanvas, this.transform.position, UICanvas.rotation);
            this.GetComponent<PlayerMovment>().enabled = false;
        }            
    }

    void ApplyMovement()
    {
        float moveHorizontal = 0f;
        float moveVertical = 0f;
        moveHorizontal = Input.GetAxis("Horizontal");
        moveVertical = Input.GetAxis("Vertical");

        if (cc.isGrounded)
        {          
            moveDown = 0;
        }
        else
        {
            moveDown -= gravity * Time.deltaTime;
            //moveHorizontal = 0;
            //moveVertical = 0;
        }
        
        Vector3 movement = new Vector3(moveHorizontal, moveDown, moveVertical);
        cc.Move((movement * speed));
    }

    float AdjustCamera(Vector3 camPos)
    {
        if (cameraZoom == cameraZoomIn && camPos.y > cameraZoomIn)
        { camPos.y -= .5f; }
        if (cameraZoom == cameraDefaultZoom && camPos.y < cameraDefaultZoom)
        { camPos.y += .5f; }
        if (cameraZoom == cameraDefaultZoom && camPos.y > cameraDefaultZoom)
        { camPos.y -= .5f; }

        return camPos.y;
        
    }

    void AdjustSpeed()
    {
        if (speed > baseSpeed)
        { speed -= .1f; }
        else if (speed < baseSpeed)
        { speed = baseSpeed; }
    }
}
