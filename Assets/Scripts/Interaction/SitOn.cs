using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SitOn : MonoBehaviour {

    public GameObject character;
    public GameObject anchor;
    bool isWalkingTowards = false;
    bool sittingOn = false;
    Animator anim;

    void Start() {

        anim = character.GetComponent<Animator>();
    }

    private void Update() {

        if (isWalkingTowards) {

            AutoWalkTowards();
        }
    }

    private void FixedUpdate() {

        AnimLerp();
    }

    private void OnMouseDown() {

        if (!sittingOn) {

            anim.SetFloat("speed", 1.0f);
            anim.SetBool("isWalking", true);
            isWalkingTowards = true;
            Controller.controlledBy = this.gameObject;
        } else {

            anim.SetBool("isSitting", false);
            isWalkingTowards = false;
            Controller.controlledBy = null;
            sittingOn = false;
        }
    }

    private void AutoWalkTowards() {

        Vector3 targetDir;
        targetDir = new Vector3(anchor.transform.position.x - character.transform.position.x, 0.0f, anchor.transform.position.z - character.transform.position.z);
        Quaternion rot = Quaternion.LookRotation(targetDir);
        character.transform.rotation = Quaternion.Slerp(character.transform.rotation, rot, 0.05f);
        // character.transform.Translate(Vector3.forward * 0.01f);

        if(Vector3.Distance(character.transform.position, anchor.transform.position) < 0.8f) {

            print("Less than 0.6f");
            anim.SetBool("isSitting", true);
            anim.SetBool("isWalking", false);

            character.transform.rotation = anchor.transform.rotation;

            isWalkingTowards = false;
            sittingOn = true;
        }
    }

    void AnimLerp() {

        if (!sittingOn) return;

        if(Vector3.Distance(character.transform.position, anchor.transform.position) > 0.1f) {

            character.transform.rotation = Quaternion.Lerp(character.transform.rotation, anchor.transform.rotation, Time.deltaTime * 0.5f);
            character.transform.position = Vector3.Lerp(character.transform.position, anchor.transform.position, Time.deltaTime * 0.5f);
        } else {

            character.transform.rotation = anchor.transform.rotation;
            character.transform.position = anchor.transform.position;
        }
    }
}
