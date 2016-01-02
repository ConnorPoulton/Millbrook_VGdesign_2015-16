using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TSA;

public class PlayerStart : MonoBehaviour {

    Vector3 CameraStartPosition;
    Vector3 CameraTargetPosition;
    Quaternion CameraStartRotation;
    Quaternion CameraTargetRotation;

    public Transform player;
    public RectTransform StartCanvas;
    public RectTransform GameOverCanvas;
    RectTransform canvas; //store refrence to instantiated canvas
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
        canvas = Instantiate(StartCanvas, StartCanvas.position, StartCanvas.rotation) as RectTransform;
        title = canvas.GetChild(0).GetComponent<Text>();
        title.text = LevelName;
        paragraph = canvas.GetChild(1).GetComponent<Text>();
        paragraph.text = "";
        //subscribe to events
        ResourceManager.playerFound += GameOver;
        ResourceManager.playerClearedLevel += LevelClear;

        StartCoroutine(TextType()); //text type triggers LerpCamera, LerpCamera spawns player when complete
    }

    void OnDisable()
    {
        ResourceManager.playerFound -= GameOver;
        ResourceManager.playerClearedLevel -= LevelClear;         
    }

    public void GameOver()
    {
        Instantiate(GameOverCanvas, GameOverCanvas.position, GameOverCanvas.rotation);
        StartCoroutine(WaitForGameOver());
    }

    public void LevelClear()
    {

    }

    public IEnumerator TextType()
    {
        foreach (char letter in LevelDescription.ToCharArray())
        {
            paragraph.text += letter;
            if (Input.GetButton("Jump") == true)
            {

                paragraph.text = LevelDescription;
                break;
            }
            yield return new WaitForSeconds(letterPause);
        }

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


    public IEnumerator WaitForGameOver()
    {
        while (true)
        {
            if (Input.GetButtonDown("pause"))
                Application.LoadLevel(Application.loadedLevel);
            yield return null;
        }        
    }
}
