using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColorChanger : MonoBehaviour {

    void OnCollisionEnter(Collision target)
    {
        if (target.gameObject.tag != "Player")
        {
            if (target.gameObject.tag == "red")
            {
                base.gameObject.GetComponent<Collider>().enabled = false;
                target.gameObject.GetComponent<MeshRenderer>().enabled = true;
                target.gameObject.GetComponent<MeshRenderer>().material.color = Color.red;
                base.GetComponent<Rigidbody>().AddForce(Vector3.down * 50, ForceMode.Impulse);
                //HeartsFun(target.gameObject);
                Destroy(base.gameObject, .5f);
                print("Game Over");
            }
            else if(target.gameObject.tag == "floor")
            {
                //Player.instance.ChangePlayerState(Player.PlayerStateType.THROW);
                //GameObject.Find("hitSound").GetComponent<AudioSource>().Play();
                base.gameObject.GetComponent<Collider>().enabled = false;
                GameObject gameObject = Instantiate(Resources.Load("splash3"), target.gameObject.transform, false) as GameObject;
                //gameObject.transform.parent = target.gameObject.transform;
                Destroy(gameObject, 0.1f);
                target.gameObject.name = "color";
                target.gameObject.tag = "red";
                StartCoroutine(ChangeColor(target.gameObject));
                Debug.Log("Code reached ColorChanger else ================================");
            }
        }
    }

    IEnumerator ChangeColor(GameObject g)
    {
        // Wait for 0.1 second for our splash to disappear, then change the portion color
        yield return new WaitForSeconds(0.1f);
        Debug.Log("Change Color: " + g.gameObject.name);
        g.gameObject.GetComponent<MeshRenderer>().enabled = true;
        g.gameObject.GetComponent<MeshRenderer>().material.color = BallManager.oneColor;
        Destroy(base.gameObject);
    }

    void HeartsFun(GameObject g)
    {
        int @int = PlayerPrefs.GetInt("hearts");
        if(@int == 1)
        {
            FindObjectOfType<BallHandler>().FailGame();
            FindObjectOfType<BallHandler>().HeartsLow();
        }
    }

}
