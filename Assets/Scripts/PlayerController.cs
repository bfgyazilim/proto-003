using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
public class PlayerController : MonoBehaviour
{
    public GameObject projectilePrefab;
    public Transform projectileSpawnLocation;
    public float launchPower;
    public bool levelFailed;
    public static PlayerController instance;
    public bool playerWin;

    // Weapon grab
    [SerializeField]
    private GameObject weapon;
    protected bool shoot;
    float weaponMinX, weaponMaxX;
    float weaponTurnSpeed = 8f;
    float shootInterval = 0.2f;
    float joystickSensitivity = 0.2f;
    float shootTimePassed;
    bool shootTimeElapsed;
    [SerializeField]
    private float forceAmount = 20.0f;

    // Joystick controls
    Vector3 joystickVector;
    [SerializeField]
    protected VariableJoystick joystick;

    // button
    protected JoyButton joyButton1;

    public float mouseSensitivity = 20f;


    // Movement variables
    public Transform playerBody;
    Vector3 nextDir;
    [SerializeField]
    float playerSpeed = 3.0f;
    Animator animator;
    Rigidbody rb;


    float xRotation = 0f;

    // Cinemashine variables

    private CinemachineVirtualCamera cinemachineVirtualCamera;
    float shakeTimer;

    /// <summary>
    /// 
    /// </summary>
    private void Awake()
    {
        instance = this;
        cinemachineVirtualCamera = GetComponent<CinemachineVirtualCamera>();
    }
    // Start is called before the first frame update
    void Start()
    {
        //Cursor.lockState = CursorLockMode.Locked;

        // Initialize joystick
        //joystick = FindObjectOfType<VariableJoystick>();
        animator = GetComponentInChildren<Animator>();//need this...
        rb = GetComponent<Rigidbody>();
        //movementTargetPosition = transform.position;//initializing our movement target as our current position


        joystick.SetMode(JoystickType.Floating);
        joyButton1 = FindObjectOfType<JoyButton>();
    }

    // Update is called once per frame
    void Update()
    {
        if(shakeTimer > 0)
        {
            shakeTimer -= Time.deltaTime;
        }
        if(shakeTimer <= 0f)
        {
            if (cinemachineVirtualCamera != null)
            {


                CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
                    cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();

                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
            }
            else
            {
                Debug.Log("Cinemachine camera NULL");
            }
        }

        
        if (joystick != null)
        {
            //Debug.Log("Joystick not null");
            // get joystick values
            //float h = joystick.Horizontal * 1f + Input.GetAxis("Horizontal") * 1f;
            //float v = joystick.Vertical * 1f + Input.GetAxis("Vertical") * 1f;
            //weapon.transform.Rotate(new Vector3(joystick.Vertical * 2f + Input.GetAxis("Vertical") * 2f, joystick.Horizontal * 2f + +Input.GetAxis("Horizontal") * 2f, 0));

            // Camera control script
            //float mouseX = (joystick.Horizontal + Input.GetAxis("Mouse X")) * mouseSensitivity * Time.deltaTime;
            //float mouseY = (joystick.Vertical + Input.GetAxis("Mouse Y")) * mouseSensitivity * Time.deltaTime;

            float mouseX = (joystick.Horizontal) * mouseSensitivity * Time.deltaTime;
            float mouseY = (joystick.Vertical) * mouseSensitivity * Time.deltaTime;

            // Normailize moving of joystick vector to only forward or lef-right, disable cross movement.
            int inputX = Mathf.RoundToInt(joystick.Horizontal);
            int inputZ = Mathf.RoundToInt(joystick.Vertical);


            nextDir = new Vector3(Input.GetAxisRaw("Horizontal") + inputX, 0, Input.GetAxisRaw("Vertical") + inputZ);

            if (nextDir != Vector3.zero)
            {
                Vector3 vs = nextDir * Time.deltaTime * playerSpeed;
                if ((vs.x + transform.position.x > -2.5f) && (vs.x + transform.position.x < 1.4f))
                {
                    transform.position += nextDir * Time.deltaTime * playerSpeed;
                }
                animator.SetBool("Idling", false);
                animator.SetBool("isRunning", true);                
            }
            else
            {
                animator.SetBool("isRunning", false);
                animator.SetTrigger("DynIdle");
                //animator.SetBool("Idling", true);//stop moving
                // After touch released, there is a slide unwanted movement until she stops, this code is to prevent it!
                rb.velocity = Vector3.zero;

            }


            /*
            xRotation -= mouseY;
            xRotation = Mathf.Clamp(xRotation, -30f, 30f);


            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
            playerBody.Rotate(Vector3.up * mouseX);

            if(Input.GetKeyDown(KeyCode.F))
            {
                shoot = true;
            }
            // Get touch, so you can shoot, while using Joystick(aiming), independently from each other, native Unity Code...
            if (Input.touchCount > 0 || Input.GetMouseButtonDown(0))
            {
                // Code for editor, disable when building
#if UNITY_EDITOR
                Shoot();
#endif
                if (shootTimePassed < shootInterval)
                {
                    shootTimePassed += Time.deltaTime;
                }
                else
                {
                    shoot = true;
                    Shoot();
                    shootTimePassed = 0;                    
                }
            }
            */
        }
        else
        {
            Debug.Log("Joystick null");
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void Shoot()
    {
        //GameObject projectile = Instantiate(projectilePrefab, projectileSpawnLocation.position, Quaternion.identity);
        //Rigidbody rb = projectile.GetComponent<Rigidbody>();

        GameObject bullet = Instantiate(projectilePrefab, projectileSpawnLocation.position, Quaternion.identity);
        Rigidbody rb = bullet.GetComponent<Rigidbody>();
        rb.AddForce(weapon.transform.forward * forceAmount);
        Debug.DrawRay(weapon.transform.forward, weapon.transform.forward);

        ShakeCamera(5f, .1f);
        AudioManager.instance.PlaySFX(0);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="intensity"></param>
    /// <param name=""></param>
    public void ShakeCamera(float intensity, float time)
    {
        CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin =
                cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = intensity;
    }

    /// <summary>
    /// 
    /// </summary>
    public void ChangePlayerStateToDie()
    {
        // Death
        AudioManager.instance.PlaySFX(1);
        levelFailed = true;

    }


    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitAndLoad()
    {
        yield return new WaitForSeconds(2f);

        GameManager.instance.RestartGame();
    }

    /// <summary>
    /// 
    /// </summary>
    public void ChangePlayerStateToWin()
    {
        playerWin = true;

        // Wait 10 sec, then enable UI Manager WinView
        StartCoroutine(GameManager.instance.WaitForGivenTime(2f));
    }
}
