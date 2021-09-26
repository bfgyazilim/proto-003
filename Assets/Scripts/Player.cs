using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float turnSpeed = 20f;

    public Animator anim;
    Rigidbody m_Rigidbody;
    Vector3 m_Movement;
    Quaternion m_Rotation = Quaternion.identity;

    private CharacterController controller;
    private Vector3 playerVelocity;
    private bool groundedPlayer;
    bool isWalking, isIdle;
    public bool isAttacking;
    public bool hitEnemy, killedEnemy;
    public float playerSpeed = 3.0f;
    private float jumpHeight = 1.0f;
    private float gravityValue = -9.81f;

    public SphereCollider sphereCollider;
    public static Player instance;
    public bool playerWin;
    public bool levelFailed;

    // Sound variables
    AudioSource m_AudioSource;
    public AudioSource m_AudioSourceAttack, m_AudioSourceHit;

    // Came from old code /////////////////////////////////////////////////////
    public GameObject sceneManager;
    public GameObject helicopter;
    public AudioClip scoreUp;
    public AudioClip damage;
    public bool PlayerWin = false;

    public GameObject[] scoreParticles;
    public TextMeshProUGUI currentScoreUI, rescueText;
    public Image[] coins;

    public int RescuedPrisoners;
    private bool splatteredBlood;

    Vector3 move;

    // Joystick controls

    // User specific variables
    protected Joystick joystick;

    
    /// <summary>
    /// 
    /// </summary>
    private void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// 
    /// </summary>
    private void Start()
    {
        // initialize character controller
        controller = gameObject.GetComponent<CharacterController>();

        // Initialize joystick
        joystick = FindObjectOfType<Joystick>();

        anim = GetComponent<Animator>();
        anim.SetBool("isGrounded", true);
        m_AudioSource = GetComponent<AudioSource>();

        levelFailed = false;

        // disable the animation of the helicopter at start...
        helicopter.GetComponent<Animator>().enabled = false;
    }

    /// <summary>
    /// 
    /// </summary>
    private void Update()
    {
        
        if(joystick != null)
        {
            //Debug.Log("Joystick not null");
            // get joystick values
            float h = joystick.Horizontal * 1f + Input.GetAxis("Horizontal") * 1f;
            float v = joystick.Vertical * 1f + Input.GetAxis("Vertical") * 1f;
            move = new Vector3(h, 0, v);
            controller.Move(move * Time.deltaTime * playerSpeed);
        }
        else
        {
            Debug.Log("Joystick null");
        }

        // Check if is on ground
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        if (!isAttacking)
        {
            anim.SetBool("isIdle", true);
            isIdle = true;

        }
        else
        {
            anim.SetBool("isIdle", false);
            isIdle = false;
        }

        if (move != Vector3.zero)
        {
            gameObject.transform.forward = move;

            anim.SetBool("isWalking", true);
            isWalking = true;
            isIdle = false;
        }
        else
        {
            isWalking = false;
            anim.SetBool("isWalking", false);
            anim.SetBool("isIdle", true);
        }


        // Changes the height position of the player..
        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -3.0f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

    }

    /// <summary>
    /// 
    /// </summary>
    private void FixedUpdate()
    {
        if (isWalking)
        {
            if (!m_AudioSource.isPlaying)
            {
                m_AudioSource.Play();
            }
        }
        else
        {
            m_AudioSource.Stop();
        }

        if (isAttacking)
        {
            if (!m_AudioSourceAttack.isPlaying)
            {
                m_AudioSourceAttack.Play();
                hitEnemy = false;
                StartCoroutine(FinishAttack());
            }
        }
        else
        {
            m_AudioSourceAttack.Stop();
        }

        if (killedEnemy)
        {
            if (!m_AudioSourceHit.isPlaying)
            {
                m_AudioSourceHit.Play();
                killedEnemy = false;
            }
        }
        else
        {
            m_AudioSourceHit.Stop();
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator FinishAttack()
    {
        yield return new WaitForSeconds(0.3f);
        anim.SetBool("isAttacking", false);
        isAttacking = false;
    }

    /// <summary>
    /// 
    /// </summary>
    public void DisableAttacking()
    {
        anim.SetBool("isAttacking", false);
        anim.SetBool("isIdle", true);
        isAttacking = false;
        killedEnemy = true;
    }

    /// <summary>
    /// 
    /// </summary>
    public void ChangePlayerStateToRun()
    {
        // Play button pressed, start running...
        anim.SetBool("isRunning", true);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="other"></param>

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "scoreup")
        {
            AudioManager.instance.PlaySFX(0);
            Instantiate(scoreParticles[2], transform.position, Quaternion.identity);
        }
        else if (other.gameObject.tag == "box")
        {
            AudioManager.instance.PlaySFX(0);
            other.gameObject.SetActive(false);

            Instantiate(scoreParticles[0], transform.position, Quaternion.identity);
            Score.instance.ShowBonusText(other.gameObject.transform.position);

        }
        else if (other.gameObject.tag == "coin")
        {
            AudioManager.instance.PlaySFX(0);
            other.gameObject.SetActive(false);

            Instantiate(scoreParticles[0], transform.position, Quaternion.identity);
            //Score.instance.ShowBonusText(other.gameObject.transform.position);

        }
        else if ((other.gameObject.tag == "triangle") || (other.gameObject.tag == "Obstacle"))
        {
            // Change state to die, and play DIE animation...
            ChangePlayerStateToDie();
        }
        // Now player has finished jumping, come to level end
        else if (other.transform.tag == "Finish")
        {
            // Give extra coins for every rescued prisoner, multiply by prisoner count..
            if(RescuedPrisoners > 0)
            {
                rescueText.text = "Rescued " + RescuedPrisoners + " Hostages";

                Score.instance.MultiplyScore(RescuedPrisoners + 1);
            }
            // turn face to the user for dancing...
            transform.rotation = Quaternion.Euler(0, 180.0f, 0);
            anim.SetBool("isDancing", true);

            // call the helicopter...
            helicopter.GetComponent<Animator>().enabled = true;

            //GetComponent<Rigidbody>().velocity = Vector3.zero;

            playerWin = true;
            
            // Wait 10 sec
            StartCoroutine(WaitForGivenTime(2f));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public void ChangePlayerStateToDie()
    {
        if(!splatteredBlood)
        {
            Instantiate(scoreParticles[2], transform.position, Quaternion.identity);
            splatteredBlood = true;
        }

        // Death
        AudioManager.instance.PlaySFX(1);

        levelFailed = true;

        // Hit an obstacle move to Dieing...
        anim.SetBool("isDying", true);
        anim.SetBool("isIdle", false);

        sceneManager.GetComponent<App_Initialize>().GameOver();
    }

    /// <summary>
    /// 
    /// </summary>
    public void WinGame()
    {
        // If you want to track if the user was able to finish the level of the game

        // ENABLE BEFORE PUBLISHING
        //TinySauce.OnGameFinished(levelNumber: GameManager.instance.GetLevelNo().ToString(), true, Score.instance.score);

        // Animate all Images to up for giving collected to Global coins
        foreach (Image cImage in coins)
        {
            cImage.gameObject.GetComponent<Animator>().SetBool("isActive", true);
            AudioManager.instance.PlayLevelWin();
            // Call coin collect here
            //coin.gameObject.GetComponent<Animator>().SetBool("isActive", true);
        }

        // Save coins to the device after level end
        Score.instance.SaveScore();

        // If you want to track if the user was able to finish the level of the game
        //TinySauce.OnGameFinished(levelNumber: GameManager.instance.GetLevelNo().ToString(), false, Score.instance.score);

        // Increment level, and get new settings
        GameManager.instance.SetNewLevel();

        // Reload Game
        StartCoroutine("WaitAndLoad");
    }

    /// <summary>
    /// Wait for the given amount of seconds
    /// </summary>
    /// <param name="f"></param>
    /// <returns></returns>
    IEnumerator WaitForGivenTime(float f)
    {
        yield return new WaitForSeconds(f);

        // open rewardUI
        App_Initialize.instance.rewardMenuUI.SetActive(true);
        App_Initialize.instance.inGameUI.SetActive(false);
        // Show how many coins collected in this level +XXX
        currentScoreUI.text = "+" + Score.instance.score.ToString();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    IEnumerator WaitAndLoad()
    {
        yield return new WaitForSeconds(2f);

        App_Initialize.instance.RestartGame();
    }
}