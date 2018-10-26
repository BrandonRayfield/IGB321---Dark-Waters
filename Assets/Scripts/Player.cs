﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using System.Linq;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour {

    NavMeshAgent agent;
    public Camera camera;
    public Vector3 moveTo;
    public bool canMove;
    public bool canShoot;

    public float health = 100.0f;
    private bool dead;

    public GameObject projectile;
    private float fireTimer;
    private float fireTime = 0.5f;
    public GameObject fireLocation;

    [Header("Torch Variables")]
    public GameObject torchObject;
    public bool torchEnabled;
    public float maxTorchPower = 3.0f;
    public float minTorchPower = 0.7f;
    public float currentTorchPower;
    private float torchDimRate = 0.1f;
    public Slider powerBar;

    [Header("Explosion Prefab")]
    public GameObject explosionObject;

    private GameObject[] gunObjects;

	// Use this for initialization
	void Start () {
        agent = GetComponent<NavMeshAgent>();
        moveTo = transform.position;

        canMove = true;

        currentTorchPower = maxTorchPower;
    }
	
	// Update is called once per frame
	void Update () {

        if(dead) {
            StartCoroutine(Restart());
        }

        powerBar.value = currentTorchPower - minTorchPower;
        torchObject.GetComponent<Light>().intensity = currentTorchPower;

        if (Input.GetKeyDown(KeyCode.F)) {
            if (torchEnabled) {
                ActivateTorch(false);
            } else {
                ActivateTorch(true);
                if(currentTorchPower >= minTorchPower) {
                    currentTorchPower -= 0.1f;
                }
            }

        }

        if(torchEnabled) {
            if(currentTorchPower >= minTorchPower) {
                currentTorchPower -= Time.deltaTime * torchDimRate;
            }
        } else {
            if (currentTorchPower <= maxTorchPower) {
                currentTorchPower += Time.deltaTime * torchDimRate;
            }
        }

        if (torchEnabled) {
            torchObject.SetActive(true);
        } else {
            torchObject.SetActive(false);
        }

        //Basic Player Movement - Left Mouse Button on Environment
        if (Input.GetMouseButton(0))
            PlayerMovement();

        //Basic Player Weapon - Right Mouse Button
        if (Input.GetMouseButton(1) && canShoot)
            FireProjectile();

        agent.SetDestination(moveTo);
    }

    //Basic Movement Controls - Move to cursor position
    void PlayerMovement() {
        if (canMove) {
            RaycastHit hit;
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out hit)) {
                if (hit.transform.tag == "Ground") {
                    moveTo = hit.point;
                }
            }
        }
    }

    //Basic Weapon Controls - Track mouse location and spawn projectile at intersecting plane
    void FireProjectile() {

        Vector3 targetPoint;

        // Generate a plane that intersects the transform's position with an upwards normal.
        Plane playerPlane = new Plane(Vector3.up, transform.position);

        // Generate a ray from the cursor position
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        // Determine the point where the cursor ray intersects the plane.
        float hitdist = 0.0f;
        // If the ray is parallel to the plane, Raycast will return false.
        if (playerPlane.Raycast(ray, out hitdist)) {
            // Get the point along the ray that hits the calculated distance.
            targetPoint = ray.GetPoint(hitdist);

            fireLocation.transform.LookAt(targetPoint);

            if (Time.time > fireTimer) {

                Instantiate(projectile, fireLocation.transform.position, fireLocation.transform.rotation);

                fireTimer = Time.time + fireTime;
            }
        }
    }

    public void takeDamage(float damage) {

        if(canMove) {
            health -= damage;

            if (health <= 0) {
                gameObject.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = false;
                canMove = false;
                //Instantiate(explosionObject, transform.position, transform.rotation);
                dead = true;
            }
        }
    }

    private IEnumerator Restart() {
        yield return new WaitForSeconds(3);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void OnTriggerEnter(Collider other) {
        
        if(other.tag == "Goal") {
            GameManager.instance.levelComplete = true;
            StartCoroutine(GameManager.instance.LoadLevel(GameManager.instance.nextLevel));
        }

        if(other.tag == "Gun") {
            canShoot = true;
            gunObjects = GameObject.FindGameObjectsWithTag("Gun").OrderBy(go => go.name).ToArray();

            foreach (GameObject gun in gunObjects) {
                Destroy(gun);
            }
        }
    }

    public void SetCanMove(bool newMovement) {
        canMove = newMovement;
    }

    public void ActivateTorch(bool isOn) {
        torchEnabled = isOn;
    }

}
