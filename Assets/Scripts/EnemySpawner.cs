using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public GameObject[] spawnedEnemies;

    public GameObject enemyObject;

    public bool isActive;
    private bool hasSpawned;

    private float spawnRate = 4.0f;
    private float currentSpawnTime;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {

        spawnedEnemies = GameObject.FindGameObjectsWithTag("SpawnedEnemy");

        if (isActive && spawnedEnemies.Length < 12) {
            currentSpawnTime += Time.deltaTime;
            if (currentSpawnTime >= spawnRate && !hasSpawned) {
                hasSpawned = true;
            }    
        }

        if(hasSpawned) {
            SpawnEnemy();
        }

	}

    private void SpawnEnemy() {
        Instantiate(enemyObject, transform.position, transform.rotation);
        currentSpawnTime = 0;
        hasSpawned = false;
    }
}
