using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour {

    public GameObject thisPickup;

    public GameObject spawnLocation;

    private GameObject currentObject;

    private float respawnTimer;
    public float respawnTime = 5.0f;
	
	// Update is called once per frame
	void Update () {

        //Respawn if no object and respawnTimer is over
        if(!currentObject && Time.time > respawnTimer) {

            currentObject = Instantiate(thisPickup, spawnLocation.transform.position, spawnLocation.transform.rotation) as GameObject;

        }
	}

    //Item-Player Interaction on pickup - Override in child class for item specific behaviour
    public virtual void OnTriggerEnter(Collider other) {
        
        if(other.tag == "Player") {

            // Adjust player attributes here...
            Destroy(currentObject);
            respawnTimer = Time.time + respawnTime;
        }
    }
}
