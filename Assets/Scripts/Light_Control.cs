﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Light_Control : MonoBehaviour {

    public Light mainLight;
    public bool isDark;
    public bool stillDark;
    public int dimSpeed = 5;

    private float maxIntensity = 1.0f;
    private float minIntensity = 0.0f;
    public float currentIntensity;

    // Use this for initialization
    void Start () {
        currentIntensity = maxIntensity;
    }
	
	// Update is called once per frame
	void Update () {

        if(stillDark) {
            mainLight.intensity = currentIntensity;
        }

        if (isDark) {
            if (currentIntensity > minIntensity) {
                currentIntensity -= (Time.deltaTime / dimSpeed);
            }
        } else {
            if (currentIntensity < maxIntensity) {
                currentIntensity += (Time.deltaTime / dimSpeed);
            } else {
                stillDark = false;
            }
        }
	}

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player") {
            isDark = true;
            stillDark = true;
        }
    }

    private void OnTriggerExit(Collider other) {
        if (other.gameObject.tag == "Player") {
            if(stillDark) {
                isDark = false;
            }

        }
    }

}
