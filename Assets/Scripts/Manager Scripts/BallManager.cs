using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallManager : MonoBehaviour
{
    public static float rotationSpeed =75;

    public static Color oneColor = Color.green;
    public GameObject ball;

    private float speed = 100f;

    // Start is called before the first frame update
    void Start()
    {
        MakeANewCircle();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
        {
            HitBall();
        }
    }

    public void HitBall()
    {
        GameObject gameObject = Instantiate<GameObject>(ball, new Vector3(0, 1f, 3f), Quaternion.identity);
        gameObject.GetComponent<MeshRenderer>().material.color = oneColor;
        gameObject.GetComponent<Rigidbody>().AddForce(Vector3.forward * speed, ForceMode.Impulse);
    }

    void MakeANewCircle()
    {
        GameObject gameObject2 = Instantiate(Resources.Load("round" + Random.Range(3, 6))) as GameObject;
        gameObject2.transform.position = new Vector3(0, 20, 13);
        gameObject2.name = "Circle";
    }
}
