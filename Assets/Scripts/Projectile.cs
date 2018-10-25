using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour {

    public float speed = 10.0f;
    public float damage = 5.0f;

    private float lifeTime = 1.5f;

    private void Start() {
        Destroy(this.gameObject, lifeTime);
    }

    // Update is called once per frame
    void Update () {
        Movement();
	}

    void Movement() {
        transform.position += Time.deltaTime * speed * transform.forward;
    }

    //When projectile hits enemy, tell enemy object to respawn and then destroy projectile
    void OnTriggerEnter(Collider otherObject) {

        if (otherObject.tag == "Enemy" || otherObject.tag == "SpawnedEnemy") { 
            otherObject.GetComponent<Enemy>().takeDamage(damage);
            Destroy(this.gameObject);
        }
        else if (otherObject.tag == "Environment") {
            Destroy(this.gameObject);
        }
    }
}
