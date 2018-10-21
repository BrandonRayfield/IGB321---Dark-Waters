using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {

    //Singleton Setup
    [Header("Main Variables")]
    public static GameManager instance = null;
    public Camera cameraObject;
    public GameObject submarineObject;

    public bool levelComplete = false;

    string thisLevel;
    public string nextLevel;

    // Awake Checks - Singleton setup
    void Awake() {

        //Check if instance already exists
        if (instance == null)

            //if not, set instance to this
            instance = this;

        //If instance already exists and it's not this:
        else if (instance != this)
            //Then destroy this. This enforces our singleton pattern, meaning there can only ever be one instance of a GameManager.
            Destroy(gameObject);
    }

    // Use this for initialization
    void Start() {
        thisLevel = SceneManager.GetActiveScene().name;

        try {
            cameraObject = GameObject.Find("Main Camera").GetComponent<Camera>();
        } catch {
            Debug.Log("Could not find camera object - Make sure there is a camera called 'Main Camera' in the scene.");
            cameraObject = null;
        }

	}

    // Update is called once per frame
    void Update () {
        if(Input.GetKey(KeyCode.F1)) {
            cameraObject.GetComponent<IsometricCamera>().SetCameraTarget(submarineObject);
        }
    }

    public IEnumerator LoadLevel(string level) {

        yield return new WaitForSeconds(5);

        SceneManager.LoadScene(level);
    }  
}
