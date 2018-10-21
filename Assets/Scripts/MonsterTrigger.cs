using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterTrigger : MonoBehaviour {

    public GameObject monsterShadow;
    private Animator monsterAnimator;
    public GameObject monsterSound;
    private bool hasTriggered;

	// Use this for initialization
	void Start () {
        monsterAnimator = monsterShadow.GetComponent<Animator>();
        monsterShadow.SetActive(false);

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player") {
            if (!hasTriggered) {
                monsterShadow.SetActive(true);
                //Instantiate(monsterSound, monsterShadow.transform.position, monsterShadow.transform.rotation);
                monsterAnimator.Play("Swim");
                StartCoroutine(HideMonster());
                hasTriggered = true;
            }
        }
    }

    private IEnumerator HideMonster() {
        yield return new WaitForSeconds(6);
        monsterShadow.SetActive(false);
    }

}
