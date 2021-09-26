using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class BulletController : MonoBehaviour
{
    [SerializeField]
    GameObject bot;

    // Explosion variables
    public float radius = 5.0F;
    public float power = 10.0F;
    public Material explosiveMaterial;
    public GameObject explosionParticle;
    

    //Used to shake the camera providing a screenshake effect
    CinemachineImpulseSource impulseSource; 

    // Start is called before the first frame update
    void Start()
    {
        impulseSource = GetComponent<CinemachineImpulseSource>(); 
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        /*
        if(other.gameObject.tag == "Floor")
        {
            //gameObject.GetComponentInChildren<BotController>().EnableBot();
            GameObject go = Instantiate(bot, transform.position, Quaternion.identity);            
            go.GetComponent<BotController>().ActivateWaypoint();

            Debug.Log("Triggered Floor");
        }
        */
        //Detonate();

        if (other.gameObject.tag == "Wall")
        {
            // Enable rigidbodies            
            Detonate();
            //Destroy(gameObject);
            Debug.Log("Explosion");
        }
        else if(other.gameObject.tag == "Ragdoll")
        {

            // Enable rigidbodies            
            Detonate();            
            //Destroy(gameObject);
            Debug.Log("Explosion hit Ragdoll");

            // If enemy have not been killed before, If it is the first time!
            if(other.gameObject.GetComponent<EnemyController>().GetState())
            {
                // Jump of the Ragdoll , with special setting
                other.gameObject.GetComponent<EnemyController>().Die();

                // It will decrease the enemy count by 1, if remaining enemy is 0, then change player state to WIN!
                // fly copter to the landing zone to take the prisoners!!!!
                GameManager.instance.DecreaseEnemy();
            }
        }
        else if(other.gameObject.tag == "Prisoner")
        {
            Detonate();
            // You lose the game bro, don't kill the hostile guys, you gonna save them.
            //PlayerController.instance.ChangePlayerStateToDie();
            other.gameObject.GetComponent<Unit>().Die();
            // Turn back to last Timeline
            GameManager.instance.GameOver();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {

        if (collision.gameObject.tag == "Wall")
        {
            Detonate();
            Destroy(gameObject);
            Debug.Log("Explosion");
        }
    }

    /// <summary>
    /// Apply an explosice force to nearby rigidbodies
    /// </summary>
    void Detonate()   
    {
        Vector3 explosionPos = transform.position;

        // Cast a sphere around this to find nearby colliders
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);

        // Check if each one found has a Rigidbody
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                // Make the objects reactable to Physics forces...
                rb.isKinematic = false;
                rb.AddExplosionForce(power, explosionPos, radius, 130.0F);
            }
        }

        //If the explosion particle is assigned in the Inspector...
        if (explosionParticle != null)
        {
            //Instantiate a particle system
            var particle = Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
            //And then destroy it after 3 seconds
            Destroy(particle, .3f);
        }

        if (impulseSource != null)
        {
            //If there is a Cinemachine impulse source assigned, generate an impulse to create a screenshake effect
            impulseSource.GenerateImpulse();
            Debug.Log("Impulse generated");
        }

        //Destroy this gameObject
        Destroy(gameObject);
    }

    /*
    /// <summary>
    /// Apply an explosive force to nearby rigidbodies
    /// </summary>
    void Detonate()
    {
        Vector3 explosionPos = transform.position;

        //Cast a sphere around this to find nearby colliders
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);

        //Check if each one found has a Rigidbody
        foreach (Collider hit in colliders)
        {
            Rigidbody rb = hit.GetComponent<Rigidbody>();
            if (rb != null)
            {
                //If it does, add explosion force and send it flying
                var realPower = power / Time.timeScale;
                rb.AddExplosionForce(realPower, explosionPos, radius, 1);
            }
        }

        //If the explosion particle is assigned in the Inspector...
        if (explosionParticle != null)
        {
            //Instantiate a particle system
            var particle = Instantiate(explosionParticle, transform.position, explosionParticle.transform.rotation);
            //And then destroy it after 3 seconds
            Destroy(particle, 3);
        }

        if (impulseSource != null)
        {
            //If there is a Cinemachine impulse source assigned, generate an impulse to create a screenshake effect
            impulseSource.GenerateImpulse();
        }

        //Destroy this gameObject
        Destroy(gameObject);
    }
    */
}
