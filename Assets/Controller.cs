using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {

	float speed = 2.0F;
    float rotationSpeed = 100.0F;
    Animator anim;
    float weight = 1f;

    public static GameObject controlledBy;
    public Transform phone;
    public Transform receiver;
    public Transform hand;

	// Use this for initialization
	void Start () {
		anim = this.GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {

        if(controlledBy != null) return;

		float translation = Input.GetAxis("Vertical") * speed;
        float rotation = Input.GetAxis("Horizontal") * rotationSpeed;
        rotation *= Time.deltaTime;
        
        transform.Rotate(0, rotation, 0);

        if(translation != 0)
        {
        	anim.SetBool("isWalking",true);
        	anim.SetFloat("speed", translation);
        }
        else
        {
        	anim.SetBool("isWalking",false);
        	anim.SetFloat("speed", 0);
        }	


    }

    void OnAnimatorIK (int layerIndex) 
    {
        weight = anim.GetFloat("IKPickup");
        if(weight > 0.7 && anim.GetBool("isAnswering"))
        {
            phone.parent = hand;
            phone.localPosition = new Vector3(-0.097f,0.054f,0.024f);
            phone.localRotation = Quaternion.Euler(37.261f,0,0);
        }
        else if(weight > 0.7 && !anim.GetBool("isAnswering"))
        {
            phone.parent = receiver;
            phone.localPosition = Vector3.zero;
            phone.localRotation = Quaternion.identity;
        }

        anim.SetIKPosition(AvatarIKGoal.RightHand, receiver.position);
        anim.SetIKPositionWeight(AvatarIKGoal.RightHand, weight);
    }
}
