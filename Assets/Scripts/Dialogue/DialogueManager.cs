﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {

    private Queue<Dialogue> sentences;
    public Text nameText;
    public Text dialogueText;
    public Image characterPortrait;
    public Animator animator;
    public GameObject cameraObject;

    private Dialogue sentence;
    public bool isTalking;
    public bool finishedTalking;

    private int questID;
    private bool isQuestGiver;
    private bool acceptedQuest;
    private bool finishedQuest;

    private int currentNpcID;

    // Automatic Trigger Variables
    private bool isAutomatic;
    private bool autoComplete;

    //Trigger Variables
    private bool isLightSwitch;
    private GameObject doorTrigger;

    // GameManager Object for quests
    private GameManager gameManager;
    private LightManager lightManager;

    // Use this for initialization
    void Start() {

        gameManager = FindObjectOfType<GameManager>();
        lightManager = gameManager.gameObject.GetComponent<LightManager>();

        sentences = new Queue<Dialogue>();
        finishedTalking = true;
        try {
            cameraObject = GameObject.FindGameObjectWithTag("MainCamera");
        } catch {
            cameraObject = null;
        }
    }

    public void StartDialogue(Dialogue[] dialogue) {

        isTalking = true;
        if (isAutomatic) {
            autoComplete = false;
        }

        animator.SetBool("IsOpen", true);
        sentences.Clear();

        foreach (Dialogue sentence in dialogue) {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence() {
        if (finishedTalking && sentences.Count == 0) {

            if (isAutomatic) {
                autoComplete = true;
            }

            if (isLightSwitch) {
                lightManager.ActivateLights();
                lightManager.TurnOnLights();
                doorTrigger.SetActive(true);
                doorTrigger.GetComponent<BlastDoorTrigger>().SetHasPower(true);
            }

            EndDialogue();
            return;
        }
        StopAllCoroutines();
        if(finishedTalking) {
            sentence = sentences.Dequeue();
            characterPortrait.sprite = sentence.characterPortrait;
            nameText.text = sentence.name;

            StartCoroutine(TypeSentence(sentence.sentences));
        } else {
            skipTextAnimation(sentence.sentences);
        }
    }

    IEnumerator TypeSentence(string sentence) {
        dialogueText.text = "";

        finishedTalking = false;

        foreach (char letter in sentence.ToCharArray()) {
            dialogueText.text += letter;
            yield return new WaitForSeconds(.02f);
        }

        finishedTalking = true;

    }

    public void skipTextAnimation(string sentence) {
        dialogueText.text = sentence;
        finishedTalking = true;
    }

    public void EndDialogue() {
        StopAllCoroutines();
        isTalking = false;
        finishedTalking = true;
        animator.SetBool("IsOpen", false);
    }

    public bool getIsTalking() {
        return isTalking;
    }

    public bool getFinishedTalking() {
        return finishedTalking;
    }

    public void setIsQuestGiver(bool isQuest) {
        isQuestGiver = isQuest;
    }

    public void setQuestID(int newQuestID) {
        questID = newQuestID;
    }

    public void setCurrentNpcID(int newID) {
        currentNpcID = newID;
    }

    public int getCurrentNpcID() {
        return currentNpcID;
    }

    //--------------------------------------------------------------------------------------
    // Automatic Functions

    public void setIsAutomatic(bool automatic) {
        isAutomatic = automatic;
    }

    public bool getIsAutoComplete() {
        return autoComplete;
    }


    public void setIsPowerTrigger(bool lightSwitch) {
        isLightSwitch = lightSwitch;
    }

    public void setDoorObject(GameObject targetDoorObject) {
        doorTrigger = targetDoorObject;
    }

}
