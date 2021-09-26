using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;
using Cinemachine;

public class Player : MonoBehaviour
{
    public static event Action OnLevelFinished;
    public static Player instance;
    //[SerializeField] UnityEvent OnCompleteEvent, OnPickedUpFirstEvent;

    Vector3 moveVector;
    [SerializeField]
    float playerSpeed, horizontalSpeed;
    [SerializeField]
    GameObject[] crate;
    bool hit = false;
    float hitCount = 0;
    public bool levelFailed, levelFinished, levelStarted;
    bool registeredEvent;
    [SerializeField]
    float offset;
    GameObject go;

    // Joystick controls
    Vector3 joystickVector;
    [SerializeField]
    protected VariableJoystick joystick;

    // Cinemashine variables
    [SerializeField]
    private CinemachineVirtualCamera cinemachineVirtualCamera;
    CinemachineBasicMultiChannelPerlin cinemachineBasicMultiChannelPerlin;
    [SerializeField]
    float shakeTimer;

    // Text stacking variables
    string temp;
    bool textHit;
    string old;

    // State of the player
    bool died;

    public float invincibleLength = 2f;
    private float invincCounter;
    [SerializeField]
    GameObject playerModel;

    private void Awake()
    {
        instance = this;
        // Set Cinemachine Perlin Noise Amplitude Gain to Zero at start
        cinemachineBasicMultiChannelPerlin = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
    }

    private void Start()
    {
        joystick.SetMode(JoystickType.Floating);
        InitializeCrateText();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        // Shake Camera Logic, decrease timer, and shake'em up....
        if (shakeTimer > 0 && died == true)
        {
            // Set Cinemachine Perlin Noise Amplitude Gain to Zero at start
            cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 1f;
            // decrease timer to decide for how long It will shake
            shakeTimer -= Time.deltaTime;
        }
        if (shakeTimer <= 0f)
        {
            if (cinemachineVirtualCamera != null)
            {
                cinemachineBasicMultiChannelPerlin.m_AmplitudeGain = 0f;
            }
            else
            {
                Debug.Log("Cinemachine camera NULL");
            }
        }

        // If gets hit countdown for invincibile length so don't hurt again in that interval...
        if(invincCounter > 0)
        {
            invincCounter -= Time.deltaTime;

            //if(invincCounter < 1f)
            //{
            if(Mathf.Floor(invincCounter * 5f) % 2 == 0)
            {
                playerModel.SetActive(true);
            }
            else
            {
                playerModel.SetActive(false);
            }

            if(invincCounter <= 0)
            {
                playerModel.SetActive(true);
            }
            //}
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(levelStarted)
        {
            if (!levelFailed && !levelFinished)
            {
                // Normailize moving of joystick vector to only forward or lef-right, disable cross movement.
                int inputX = Mathf.RoundToInt(joystick.Horizontal);
                int inputZ = Mathf.RoundToInt(joystick.Vertical);

                // move in Z axis automatically
                transform.position += new Vector3(0, 0, playerSpeed * Time.deltaTime);

                // move horizontal with Joystick + Keyboard Input
                Vector3 nextDir = new Vector3(Input.GetAxisRaw("Horizontal") + inputX, 0, 0);

                if (nextDir != Vector3.zero)
                {
                    Vector3 vs = nextDir * Time.deltaTime * playerSpeed;
                    if ((vs.x + transform.position.x > -1.1f) && (vs.x + transform.position.x < 1.1f))
                    {
                        transform.position += nextDir * Time.deltaTime * playerSpeed;
                    }
                }
                else
                {
                    // Log here, there is no movement
                }
            }
        }

        /*
        if (joystick != null)
        {
            //Debug.Log("Joystick not null");
            // get joystick values
            float h = joystick.Horizontal * 1f + Input.GetAxis("Horizontal") * 1f;
            float v = joystick.Vertical * 1f + Input.GetAxis("Vertical") * 1f;


            // Normailize moving of joystick vector to only forward or lef-right, disable cross movement.
            int inputX = Mathf.RoundToInt(joystick.Horizontal);
            int inputZ = Mathf.RoundToInt(joystick.Vertical);

            nextDir = new Vector3(-Input.GetAxisRaw("Horizontal") - inputX, 0, Input.GetAxisRaw("Vertical") + inputZ);

            if (nextDir != Vector3.zero)
            {
                Vector3 vs = nextDir * Time.deltaTime * playerSpeed;
                if ((vs.x + transform.position.x > -2.5f) && (vs.x + transform.position.x < 1.4f))
                {
                    transform.position += nextDir * Time.deltaTime * playerSpeed;
                }
                animator.SetBool("Idling", false);
            }
            else
            {
                animator.SetBool("Idling", true);//stop moving
                                                 // After touch released, there is a slide unwanted movement until she stops, this code is to prevent it!
                rb.velocity = Vector3.zero;

            }
        }
        */
    }

    /// <summary>
    /// Level Finished, Invoke OnLevelFinished event (for listeners to register OnEnable, code this!)
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Finish")
        {
            Debug.Log("Level Finished");
            OnLevelFinished.Invoke();
        }
    }

    /// <summary>
    /// Register to the event on Collectible
    /// </summary>
    public void RegisterToSpawnedObjects()
    {

        //foreach (var collectible in _gatherables)
        //    collectible.OnPickup += HandleStacking; // Registering for the OnPickup event on Collectible
        // Registering for the OnPickup event on Collectible (getting it from the ObjectSpawner's List)
        if (ObjectSpawner.instance != null)
        {
            // Collectibles registering to Events
            Debug.Log("Gatherables count: " + ObjectSpawner.instance._gatherables.Count);
            foreach (var collectible in ObjectSpawner.instance._gatherables)
            {
                // If the collectible is a coin, HandleScore otherwise HandleStackin, add logic here for event registering accordingly Jewels...
                if(collectible.gameObject.name.StartsWith("Jew"))
                {
                    collectible.OnPickup += UIManager.instance.HandleCoinPickup;
                }
                else
                {
                    collectible.OnPickup += HandleStacking; // Registering for the OnPickup event on Collectible
                }
                collectible.OnPickup += FXManager.instance.HandleFeedbackParticles; // Register FXManager to the Onpickup event on Collectible
                //Debug.Log("Registered to collectible: OnPickup " + collectible.name);
            }
            Crate.OnHit += HandleHit;
            Crate.OnHit += FXManager.instance.HandleHitFeedbackParticles; // Register FXManager to the OnHit event on Crate(Player)
        }
        else
        {
            Debug.Log("ObjectSpawner NULL");
        }
        

    }

    /// <summary>
    /// Enables us to be aware of that the Crate was hit by an Enemy
    /// </summary>
    void HandleHit()
    {
        Debug.Log("Handle Hit");
        // Until invincibility counter is less then 0, then we can get hurt
        if(invincCounter <= 0)
        {
            invincCounter = invincibleLength;
            if(playerModel != null)
            {
                playerModel.SetActive(false);
            }            
        }

        hit = true;
    }

    /// <summary>
    /// Handle the pickup logic in player
    /// </summary>
    /// <param name="collectible"></param>
    void HandleStacking(Collectible collectible)
    {
        int childCount = transform.GetChild(0).childCount;
        int index = 0;

        if(collectible.gameObject.name.StartsWith("Tex")) 
        {
            index = 0;
            temp = collectible.capitalText.text;
            textHit = true;
        }
        else if (collectible.gameObject.name.StartsWith("Ca"))
        {
            index = 1; 
        }
        else if (collectible.gameObject.name.StartsWith("Ic"))
        {
            index = 2;
        }

        // Get First Child (Handler), and Instantiate the new Crate in it as sub-object
        go = Instantiate(crate[index], transform.GetChild(0), false);

        // Assign collected object's inside text property to the Crate's text property...
        if(textHit)
        {
            go.GetComponentInChildren<Crate>().capitalText.text = collectible.capitalText.text;
            textHit = false;
            ReorderCrateText();
        }
        
        float newY = childCount * offset;

        if (hit)
        {
            hitCount++;
            hit = false;
        }
        go.transform.localPosition = new Vector3(0, newY + (hitCount * offset), 0);
        //Debug.Log(go.transform.localPosition);        
    }

    /// <summary>
    /// Set the crates Capital Text to the spawnedCountry's letters in order
    /// </summary>
    void InitializeCrateText()
    {
        Debug.Log("InitializeCrateTex - Answer:" + ObjectSpawner.instance.spawnedAnswer + " Question: " + ObjectSpawner.instance.spawnedQuestion);

        string str1 = ObjectSpawner.instance.spawnedQuestion.Substring(0, 1).ToUpper();
        string str2 = ObjectSpawner.instance.spawnedQuestion.Substring(1, 1).ToUpper();
        string str3 = ObjectSpawner.instance.spawnedQuestion.Substring(2, 1).ToUpper();

        transform.GetChild(0).GetChild(0).gameObject.GetComponentInChildren<Crate>().capitalText.text = str3.ToString();
        transform.GetChild(0).GetChild(1).gameObject.GetComponentInChildren<Crate>().capitalText.text = str2.ToString();
        transform.GetChild(0).GetChild(2).gameObject.GetComponentInChildren<Crate>().capitalText.text = str1.ToString();

        Debug.Log("Player Crate letters: " + str1 + " " + str2 + " " + str3);        
    }

    /// <summary>
    /// Order the sub items (crates) text in Holder from  Top to bottom
    /// </summary>
    void ReorderCrateText()
    {
        int childCount = transform.GetChild(0).childCount;

        // Get Holder -> Container -> TMPText
        for(int i = childCount ; i > 0; i--)
        {
            //transform.GetChild(0).GetChild(childCount-1-i).gameObject.GetComponentInChildren<Crate>().capitalText.text =
            if (i == childCount)
            {
                old = transform.GetChild(0).GetChild(childCount-1).gameObject.GetComponentInChildren<Crate>().capitalText.text;
            }
            else
            {
                transform.GetChild(0).GetChild(i).gameObject.GetComponentInChildren<Crate>().capitalText.text =
                transform.GetChild(0).GetChild(i-1).gameObject.GetComponentInChildren<Crate>().capitalText.text;
            }
        }
        // Place the first
        transform.GetChild(0).GetChild(0).gameObject.GetComponentInChildren<Crate>().capitalText.text = old.ToString();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    public int GetChildCount()
    {
        return transform.GetChild(0).childCount;
    }
    /// <summary>
    /// Log Message for event
    /// </summary>
    void UpdateLog()
    {
        Debug.Log("HandleStacking Registered to the event (Collectible->OnPickup)");        
    }

    public void DieCameraAnimation()
    {
        // Set player state to die
        died = true;

        // Set Shake timer and start to decrement in Update in this interval
        shakeTimer = 5f;
        // Stop following we're gonna move em.
        cinemachineVirtualCamera.Follow = null;

        // Fall apart player we died!!!
       transform.Rotate(new Vector3(90, 0, 30));

    }
}
