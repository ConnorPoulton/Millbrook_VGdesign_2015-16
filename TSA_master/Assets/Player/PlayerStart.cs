using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class PlayerStart : MonoBehaviour {

    Vector3 CameraStartPosition;
    Vector3 CameraTargetPosition;
    Quaternion CameraStartRotation;
    Quaternion CameraTargetRotation;

    public Transform player;
    public RectTransform UIcanvas;
    RectTransform canvas;
    public float letterPause;
    public float TimeToLerp;
    Text title;
    Text paragraph;

    public int CameraStartZoom;
    public string LevelName;
    public string LevelDescription;

    void Start()
    {
        Vector3 trans = this.transform.position;
        CameraStartPosition = Camera.main.transform.position;
        CameraStartRotation = Camera.main.transform.rotation;
        CameraTargetPosition = new Vector3(trans.x, trans.y + CameraStartZoom, trans.z);
        CameraTargetRotation = Quaternion.Euler(new Vector3(90,0,0));    
        canvas = Instantiate(UIcanvas, UIcanvas.position, UIcanvas.rotation) as RectTransform;
        //RectTransform rectTrans = canvas.GetComponent<RectTransform>();
        title = canvas.GetChild(0).GetComponent<Text>();
        title.text = LevelName;
        paragraph = canvas.GetChild(1).GetComponent<Text>();
        paragraph.text = "";
        
       
        StartCoroutine(TextType()); //text type triggers LerpCamera, LerpCamera spawns player when complete
    }



    public IEnumerator TextType()
    {
        foreach (char letter in LevelDescription.ToCharArray())
        {
            paragraph.text += letter;
            if (Input.GetButton("Jump") == true)
            {
                Debug.Log("break");
                paragraph.text = LevelDescription;
                break;
            }
            yield return new WaitForSeconds(letterPause);
        }

        Debug.Log("end");
        yield return new WaitForSeconds(.3f);

        while (!Input.GetButton("Jump"))
            yield return null;

        canvas.gameObject.SetActive(false);
        StartCoroutine(LerpCamera()); 
        yield break;
    }



    public IEnumerator LerpCamera()
    {
        float ElapsedTime = 0f;

        while (ElapsedTime <= TimeToLerp)
        {
            ElapsedTime += Time.deltaTime;
            float perc = (ElapsedTime / TimeToLerp);
            Camera.main.transform.position = Vector3.Lerp(CameraStartPosition, CameraTargetPosition, perc);
            Camera.main.transform.rotation = Quaternion.Lerp(CameraStartRotation, CameraTargetRotation, perc);
            yield return new WaitForEndOfFrame();
        }

    yield return new WaitForSeconds(1);
    Instantiate(player, this.transform.position, this.transform.rotation);
    yield break;
    }
}
