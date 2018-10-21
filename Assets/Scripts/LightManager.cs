using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class LightManager : MonoBehaviour {

    [Header("Lighting Controls")]
    public Light mainLight;
    public bool isDark;
    public bool stillDark;
    public float dimSpeed = 5;

    // Private Variables
    private GameObject[] roomLightObjects;
    private float maxIntensity = 1.0f;
    private float minIntensity = 0.0f;
    private float currentIntensity;

    [Header("Sound Variables")]
    public GameObject disableSound;
    public GameObject enableSound;

    // Use this for initialization
    void Start () {

        //Finds all the room light game objects in scene and orders them by name
        roomLightObjects = GameObject.FindGameObjectsWithTag("RoomLight").OrderBy(go => go.name).ToArray();

    }

    // Update is called once per frame
    void Update () {
        MainLightControl();

        if (Input.GetKey(KeyCode.Alpha1)) {
            MakeDark();
        }

        if (Input.GetKey(KeyCode.Alpha2)) {
            TurnOnLights();
        }

        if (Input.GetKey(KeyCode.Alpha3)) {
            DisableLights();
        }
    }

    private void MainLightControl() {
        if (stillDark) {
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

    public void MakeDark() {
        isDark = true;
        stillDark = true;
    }

    public void TurnOnLights() {
        isDark = false;
    }

    public void DisableLights() {
        StartCoroutine(DisableRoomLights());
    }

    public IEnumerator DisableRoomLights() {
        foreach (GameObject myLight in roomLightObjects) {
            myLight.SetActive(false);
            //Instantiate(disableSound, myLight.transform.position, myLight.transform.rotation);
            yield return new WaitForSeconds(1f);
        }
    }
}
