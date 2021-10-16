using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructibleWall : MonoBehaviour
{
    [SerializeField]
    private Transform pfWallBroken;

    float damageAmount = 33;
    float totalHp;
    float remainingHp = 100.0f;
    bool zeroHp;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnCollisionStay(Collision target)
    {
        if (target.gameObject.tag == "PlayerSaw")
        {
            Debug.Log("PlayerSaw collided with wall");

            if (remainingHp > 0)
            {
                remainingHp -= damageAmount;
                AudioManager.instance.PlaySFX(3);
            }
            else
            {
                zeroHp = true;
                Instantiate(pfWallBroken, transform.position, transform.rotation);
                Destroy(gameObject);
                Debug.Log("Destroyed!");
            }
        }
    }
}
