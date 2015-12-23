using UnityEngine;
using System.Collections;
using TSA;

public class Goal : MonoBehaviour {

    public string NextLevel;
    Vector3 rotation = new Vector3(0,0,2);

    void Start()
    {
        ResourceManager.playerClearedLevel += Test;
    }

    void OnDisable()
    {
        ResourceManager.playerClearedLevel -= Test;
    }
	
	void Update ()
    {
        this.transform.Rotate(rotation, Space.Self); 
	}

    void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.tag == ("Player"))
            ResourceManager.CallplayerClearedLevel(col.gameObject);
    }

    void Test(GameObject player)
    {
        Application.LoadLevel(NextLevel);
    }
}
