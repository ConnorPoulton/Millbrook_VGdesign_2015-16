using UnityEngine;
using System.Collections;
using TSA;
[RequireComponent(typeof(Rigidbody))]


public class PlayerMovment : MonoBehaviour {

    public Rigidbody rb;
    public float baseSpeed; //default walk speed
    public float cameraZoomIn; //camera zoomed y value
    public float cameraDefaultZoom; //camera zoomed out value
    float cameraZoom; //current  camera y value
    float speed; //difference in speed from base
    float currentSpeed; //current speed value
    Vector3 trans; //store shorthand refrence to this objects position
    Transform SoundBounds; //store refrence to child object
    
    // Use this for initialization
    void Start ()
    {
        trans = this.transform.position;
        SoundBounds = this.transform.GetChild(0);
        rb = gameObject.GetComponent<Rigidbody>();

        Camera.main.transform.position = new Vector3(trans.x, trans.y + cameraDefaultZoom, trans.z);
        Camera.main.transform.rotation = Quaternion.Euler(new Vector3(90, 0, 0));

        cameraZoomIn = Camera.main.transform.position.y + cameraZoomIn;
        cameraDefaultZoom = Camera.main.transform.position.y + cameraDefaultZoom;
    }
	
	
	void FixedUpdate ()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, 0, moveVertical);
        rb.velocity = movement * currentSpeed;
    }
 
    void Update()
    {
        //TODO clean up with lerp and functions
        Vector3 camPos = Camera.main.transform.position;
        trans = this.transform.position;
        currentSpeed = speed + baseSpeed;

        if (Input.GetButton("sprint"))
        {
            cameraZoom = cameraZoomIn;
            if (speed < 8)
            {
                speed += .4f;
            }
        }else
        { cameraZoom = cameraDefaultZoom; }



        if (cameraZoom == cameraZoomIn && camPos.y > cameraZoomIn)
        { camPos.y -= .5f; }
        if (cameraZoom == cameraDefaultZoom && camPos.y < cameraDefaultZoom)
        { camPos.y += .5f; }
        if (cameraZoom == cameraDefaultZoom && camPos.y > cameraDefaultZoom)
        { camPos.y -= .5f; }

        if (speed > baseSpeed)
        { speed -= .2f; }
        else if (speed < baseSpeed)
        { speed = baseSpeed; }

        if (Input.GetButtonDown("Jump"))
        {
            SoundBounds.position = trans;
        }
        else { SoundBounds.position = ResourceManager.OutOfBounds; }

        Camera.main.transform.position = new Vector3(trans.x, camPos.y, trans.z);
    }


    void OnTriggerEnter(Collider col)
    {
        if(col.gameObject.tag == "Lethal")
            ResourceManager.CallplayerFound(this.gameObject);
    }
}
