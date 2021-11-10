using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaintManager : MonoBehaviour
{
    public static float rotationSpeed =75;

    public static Color oneColor = Color.green;
    public GameObject ball;
    public Transform nozzle, rotMuzzle;
    [SerializeField]
    Vector3 offset;
    [SerializeField]
    float speed = 3f;

    // Start is called before the first frame update
    void Start()
    {
        //MakeANewCircle();
    }

    // Update is called once per frame
    void Update()
    {        
        //if(Input.GetMouseButtonDown(0) || Input.GetKey(KeyCode.Space))
        //{
            HitBall();
            Player.instance.ChangePlayerState(Player.PlayerStateType.JOGBOX);
        //}            
    }

    public void HitBall()
    {
        //GameObject gameObject = Instantiate<GameObject>(ball, new Vector3(0, 1f, 3f), Quaternion.identity);
        Debug.Log(offset);
        GameObject gameObject = Instantiate<GameObject>(ball, nozzle.transform.position, nozzle.transform.rotation);
        Destroy(gameObject, 1.3f);
        //GameObject gameObject = Instantiate(Resources.Load("splash1"), target.gameObject.transform, false) as GameObject;

        gameObject.GetComponent<MeshRenderer>().material.color = oneColor;
        gameObject.GetComponent<Rigidbody>().AddForce(Vector3.forward * speed, ForceMode.Impulse);
    }

    void MakeANewCircle()
    {
        GameObject gameObject2 = Instantiate(Resources.Load("round" + Random.Range(3, 6))) as GameObject;
        gameObject2.transform.position = new Vector3(0, 20, 23);
        gameObject2.name = "Circle";
    }
}
