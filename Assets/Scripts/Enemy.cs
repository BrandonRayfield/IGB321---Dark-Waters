using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour {

    NavMeshAgent agent;

    public GameObject player;

    public float health = 10.0f;

    public float agroRange = 10.0f;
    public float damage = 5.0f;

    private float damageTimer;
    private float damageTime = 1.0f;

    // Use this for initialization
    void Start () {
        agent = GetComponent<NavMeshAgent>();
    }

    // Update is called once per frame
    void Update() {

        if (!player)
            player = GameObject.FindGameObjectWithTag("Player");
        else if (player) {


            //Raycast in direction of Player
            RaycastHit hit;
            if(Physics.Raycast(transform.position, -(transform.position - player.transform.position).normalized, out hit, agroRange)) {

                //If Raycast hits player
                if (hit.transform.tag == "Player") {

                    Debug.DrawLine(transform.position, player.transform.position, Color.red);

                    //Move towards player
                    agent.SetDestination(player.transform.position);
                }
            }
        }
    }

    private void OnCollisionStay(Collision collision) {

        if (collision.transform.tag == "Player" && Time.time > damageTimer) {
            collision.transform.GetComponent<Player>().takeDamage(damage);
            damageTimer = Time.time + damageTime;
        }
    }

    public void takeDamage(float thisDamage) {

        health -= thisDamage;

        if (health <= 0) {
            Destroy(this.gameObject);
        }
    }

}
