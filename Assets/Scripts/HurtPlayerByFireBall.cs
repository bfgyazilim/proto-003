using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HurtPlayerByFireBall : MonoBehaviour
{
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
        if(other.tag == "Player")
        {
            Debug.Log("Player hurt..");
            //HealthManager.instance.Hurt();
            //AudioManager.instance.PlaySFX(8);
            Destroy(gameObject);
        }
    }
}
