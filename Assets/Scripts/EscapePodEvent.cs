using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EscapePodEvent : MonoBehaviour {

    [Header("Main Objects")]
    public Camera cameraObject;
    public GameObject explosionObject;
    public GameObject triggerObject;

    private LightManager lm;
    private GameManager gm;


    // Use this for initialization
    void Start () {

        gm = GameManager.instance;
        lm = gm.GetComponent<LightManager>();

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

    public void DestroyPod() {
        //Instantiate(explosionObject, transform.position, transform.rotation);
        Debug.Log("Boom!");
        triggerObject.GetComponent<EscapePodTrigger>().TriggerNextStage();
        gameObject.SetActive(false);
        Destroy(gameObject);
    }





}
