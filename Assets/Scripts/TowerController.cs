using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        /* Do something else with wall, maybe jump or wait here to kill the Blue Bots
        if (other.gameObject.tag == "Wall")
        {
            //gameObject.GetComponentInChildren<BotController>().EnableBot();
            //GameObject go = Instantiate(bot, transform.position, Quaternion.identity);
            other.GetComponent<Destructible>().DestructWall();

            Debug.Log("Triggered Floor");
        }
        */
        if (other.gameObject.tag == "Runner")
        {
            //gameObject.GetComponentInChildren<BotController>().EnableBot();
            //GameObject go = Instantiate(bot, transform.position, Quaternion.identity);
            Debug.Log("Triggered Tower/Runner");

            // Game is over
            GameManager.instance.GameOver();

        }
    }
}
