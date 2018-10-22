using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapePodTrigger : MonoBehaviour {

    [Header("Main Objects")]
    public GameObject submarineObject;
    private Animator subAnimator;
    public GameObject doorObject;
    private Animator doorAnimator;
    public Camera cameraObject;

    private bool hasTriggered;
    private GameManager gm;
    private LightManager lm;

	// Use this for initialization
	void Start () {

        gm = GameManager.instance;
        lm = gm.GetComponent<LightManager>();

        subAnimator = submarineObject.GetComponent<Animator>();
        doorAnimator = doorObject.GetComponent<Animator>();

        try {
            cameraObject = GameObject.Find("Main Camera").GetComponent<Camera>();
        } catch {
            Debug.Log("Could not find camera object - Make sure there is a camera called 'Main Camera' in the scene.");
            cameraObject = null;
        }

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void triggerEvent() {
        if(submarineObject != null && !hasTriggered) {
            subAnimator.Play("Launch");
            doorAnimator.Play("Open");
            cameraObject.GetComponent<IsometricCamera>().SetCameraTarget(submarineObject);
            hasTriggered = true;
        }
    }



    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player") {
            triggerEvent();
        }
    }

    public void TriggerNextStage() {
        StartCoroutine(ResetCam());
        Debug.Log("Triggering Next Event");
    }

    private IEnumerator ResetCam() {
        yield return new WaitForSeconds(3);
        cameraObject.gameObject.GetComponent<IsometricCamera>().ResetCamera();
        StartCoroutine(PowerOut());
        Debug.Log("Camera Reset");
    }

    private IEnumerator PowerOut() {
        yield return new WaitForSeconds(1);
        lm.MakeDark();
        lm.DisableLights();
        StartCoroutine(NextLevel());
        Debug.Log("Power is Off");
    }

    private IEnumerator NextLevel() {
        yield return new WaitForSeconds(6);
        //gm.LoadLevel(gm.nextLevel);
        Debug.Log("Loaded Next Level");
    }

}
