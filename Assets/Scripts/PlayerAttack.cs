using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public GameEnding gameEnding;

    bool hit;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if((other.tag == "Ghost") || (other.tag == "EnemyBall"))
        {
            Player.instance.anim.SetBool("isAttacking", true);
            Player.instance.anim.SetBool("isIdle", false);
            Player.instance.isAttacking = true;
            Debug.Log("Ghost IN Attack Range");

            hit = true;
            Player.instance.killedEnemy = true;
            DestroyObject(other.gameObject, .3f);
        }
        
        if ((other.tag == "Ragdoll"))
        {
            Player.instance.anim.SetBool("isAttacking", true);
            Player.instance.anim.SetBool("isIdle", false);
            Player.instance.isAttacking = true;
            Debug.Log("Ragdoll IN Attack Range");

            hit = true;
            Player.instance.killedEnemy = true;
            //DestroyObject(other.gameObject, .3f);
            other.gameObject.GetComponent<EnemyController>().Die();
        }
        
        /*
        if(other.tag == "EnemyBall")
        {
            gameEnding.CaughtPlayer();

            Debug.Log("Trigger EnemyBall");
        }
        */
    }

    private void OnTriggerExit(Collider other)
    {
        if ((other.tag == "Ghost") || (other.tag == "EnemyBall"))
        {
            Player.instance.anim.SetBool("isAttacking", false);
            Player.instance.anim.SetBool("isIdle", true);
            Debug.Log("Ghost OUT Attack Range");
            Player.instance.isAttacking = false;

            if (hit)
            {
                Player.instance.killedEnemy = true;
                DestroyObject(other.gameObject, .3f);
            }
        }
        if ((other.tag == "Ragdoll"))
        {
            Player.instance.anim.SetBool("isAttacking", true);
            Player.instance.anim.SetBool("isIdle", false);
            Player.instance.isAttacking = true;
            Debug.Log("Ragdoll OUT Attack Range");

            hit = true;
            Player.instance.killedEnemy = true;
            //DestroyObject(other.gameObject, .3f);
            other.gameObject.GetComponent<EnemyController>().Die();
        }
        /*
        if ((other.tag == "Ragdoll"))
        {
            Player.instance.anim.SetBool("isAttacking", false);
            Player.instance.anim.SetBool("isIdle", true);
            Debug.Log("Ragdoll NOT in Attack Range");
            Player.instance.isAttacking = false;

            if (hit)
            {
                Player.instance.killedEnemy = true;
                other.gameObject.GetComponent<EnemyController>().Die();
                
            }
        }
        */
    }
}
