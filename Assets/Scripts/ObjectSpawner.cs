using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectSpawner : MonoBehaviour {

    public GameObject player;

    // Assign rotation and offset to the road from inspector...
    public GameObject[] trianglePrefabs;
    public GameObject[] enemyPrefabs;

    // Other Objects, for extra environment setups
    public GameObject[] otherPrefabs;
    public Vector3[] otherOffset;
    public Vector3[] otherRotation;
    public bool[] isOtherActive; 

    public GameObject[] dressPrefabs;
    public GameObject[] collectiblePrefabs;

    // offset position calibrating on level, left-right etc.
    public Vector3[] triangleOffset;
    public Vector3[] enemyOffset;
    public Vector3[] dressOffset;
    public Vector3[] collectibleOffset;

    // Buildings and such extra BIG objectcs
    public GameObject[] sidePrefabs;
    public Vector3[] sideOffset;
    public Vector3[] sideRotation;
    [SerializeField] bool fixedSideObjects;
    [SerializeField]
    int sideObjectMax, spawnedObjectCount;


    // Small decoration objects like umbrella and such...
    public GameObject[] decorationPrefabs;
    public Vector3[] decorationOffset;

    private Vector3 spawnObstaclePosition;
    private Vector3 spawnObstaclePosition2;
    private Vector3 spawnObstaclePosition3;
    private Vector3 spawnObstaclePosition4;


    public float spawnRange = 150f;
    private float currentPos;
    public float collectibleSpawnRange;

    // Spacing between spawnable objects
    public float triangleSpacing, enemySpacing, dressSpacing, otherSpacing;
    public float objectSpacing, objectSpacingLeft;
    public float collectibleSpacing;
    public float decorationSpacing;
    public float minSpawnToPlayer;
    public float platformHalfLength;
    // Offseter for the spawning, currently linear
    private float counter;
    // Determines how many times the counter will iterate, for the spawn loop
    [SerializeField]
    float counterIncrement;

    // Spacing Increments
    [SerializeField]
    private float triangleSpacingIncrement, enemySpacingIncrement, dressSpacingIncrement,
            collectibleSpacingIncrement, objectSpacingIncrement, objectSpacingLeftIncrement,
            decorationSpacingIncrement, otherSpacingIncrement;

    [SerializeField]
    private bool spawnLeft;

    private bool firstPass;

    // Invoke(Trigger) this event on Spawn 
    // Plauer and other classes listen to this FX, and UI
    // for the response in their ways!!!
    public List<Collectible> _gatherables;
    public List<SpeechBubble> _sideObjects;
    public static ObjectSpawner instance;

    // Text Spawn list variables
    string[] capitals = new string[] { "Athens", "Bangkok", "Beijing", "Berlin", "Amsterdam", "Ankara" };
    string[] countrynames = new string[] { "Greece", "Thailand", "China", "Germany", "Netherlands", "Turkey" };
    public string spawnedQuestion, spawnedAnswer;
    int randTextIndex;

    /// <summary>
    /// 
    /// </summary>
    private void Awake()
    {
        instance = this;
        currentPos = transform.position.x;
    }

    /// <summary>
    /// 
    /// </summary>
    private void Start()
    {        
        //currentPos = transform.parent.position.z - platformHalfLength;
        //currentPos = Player.instance.transform.position.z;

        // Get the distance between the player and the platform, if it is closer than 120 then spawn objects
        float distanceToPlayer = Vector3.Distance(player.gameObject.transform.position, transform.parent.position);

        // get the current position again, because it scrolled in Z direction
        //currentPos = transform.parent.position.z - platformHalfLength;


        // First Spawn other objects regardless of the player position
        SpawnOther();

        // Spawn Text Capitals of the spawnquestion(Country) in order to make sure all getc chance to fill the answer!!!
        //SpawnCollectiblesAllText();

        //if (distanceToPlayer < minSpawnToPlayer && currentPos + counter < currentPos + spawnRange)
        while (counter < spawnRange)
        {
            //Debug.Log("Spawner ------------ " + currentPos + " " + objectSpacing + " < " + currentPos + " " + spawnRange + "Counter:" + counter);
            
            //SpawnTriangles();
            SpawnCollectibles();

            SpawnCollectiblesInFormation();
            //SpawnEnemies();
            //SpawnDresses();

            // Spawn rotated and offseted objects , or fixed objects...
            // Can be Customer in this game...
            if (fixedSideObjects)
            {
                SpawnSideObjectsFixed();
            }
            else
            {
                SpawnSideObjects();
            }            
            //SpawnDecorationObjects();

            // general increment on the map
            counter += counterIncrement;            
            // increments relative to the counter
            triangleSpacing += triangleSpacingIncrement;
            enemySpacing += enemySpacingIncrement;
            dressSpacing += dressSpacingIncrement;
            collectibleSpacing += collectibleSpacingIncrement;
            objectSpacing += objectSpacingIncrement;
            objectSpacingLeft += objectSpacingLeftIncrement;
            decorationSpacing += decorationSpacingIncrement;
            otherSpacing += otherSpacingIncrement;
        }

        // Player registers to the collectible events at start
        Player.instance.RegisterToSpawnedObjects();
    }

    /// <summary>
    /// 
    /// </summary>
    void SpawnTriangles()
    {
        int index = Random.Range(0, trianglePrefabs.Length);
        if (triangleOffset.Length != 0)
        {
            spawnObstaclePosition = new Vector3(triangleOffset[index].x, triangleOffset[index].y, currentPos + triangleSpacing);
            Instantiate(trianglePrefabs[index], spawnObstaclePosition, Quaternion.identity, transform.parent);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void SpawnEnemies()
    {
        int index = Random.Range(0, enemyPrefabs.Length);
        if (enemyOffset.Length != 0)
        {
            spawnObstaclePosition = new Vector3(enemyOffset[index].x, enemyOffset[index].y, currentPos + enemySpacing);
            Instantiate(enemyPrefabs[index], spawnObstaclePosition, Quaternion.identity, transform.parent);
        }
    }

    void InitializeRandomText()
    {
        randTextIndex = Random.Range(0, 5); // 5 is a constant - ensure to say capitals.length
        string randomCapital = capitals[randTextIndex];
        var correspondingRandomCountry = countrynames[randTextIndex]; // assuming your country array is in the righ order

        //Console.WriteLine("Which country has the capital city {0}? ", randomCapital);
        //Console.WriteLine("Enter up to 3 names, comma-seperated: ");
        //string userinput = Console.ReadLine();
        //string[] temp = userinput.Split(',');
        //Console.WriteLine(temp.Any(t => t.ToLower().Equals(correspondingRandomCountry.ToLower())) == true ? "right answer" : "wrong answer");

        //1 index accessor
        //int strLength = longString.Length;

        // save the question and answer, later for access in the game
        spawnedAnswer = randomCapital;
        spawnedQuestion = correspondingRandomCountry;

        Debug.Log("Answer:" + spawnedAnswer + " Question: " + spawnedQuestion);

      
    }

    string SpawnRandomText()
    {        
        string str = spawnedQuestion.Substring(Random.Range(0, spawnedAnswer.Length - 2), 1).ToUpper();
        Debug.Log("Spawned letter: " + str);
        return str;
    }

    /// <summary>
    /// 
    /// </summary>
    void SpawnCollectibles()
    {
        int index = Random.Range(0, collectiblePrefabs.Length);
        if (collectibleOffset.Length != 0)
        {
            //spawnObstaclePosition2 = new Vector3(collectibleOffset[index].x, collectibleOffset[index].y, currentPos + collectibleSpacing);

            // Place the object relative to the Spawn Manager's position and increment, later randomly...
            spawnObstaclePosition2 = new Vector3(transform.position.x, transform.position.y, transform.position.z + collectibleSpacing);

            // Instantiate a new collectible, and get the returning gameobject, and then use it for adding to the List of gatherables
            //GameObject go = Instantiate(collectiblePrefabs[index], spawnObstaclePosition2, Quaternion.identity, transform.parent);
            GameObject go = Instantiate(collectiblePrefabs[index], spawnObstaclePosition2, Quaternion.identity, transform);

            // Invoke OnCollectibleSpawn
            _gatherables.Add(go.GetComponent<Collectible>());
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void SpawnCollectiblesInFormation()
    {
        // bridge prefab
        int index = 4;
        if (collectibleOffset.Length != 0)
        {
            //spawnObstaclePosition2 = new Vector3(collectibleOffset[index].x, collectibleOffset[index].y, currentPos + collectibleSpacing);

            // Place the object relative to the Spawn Manager's position and increment, later randomly...
            spawnObstaclePosition2 = new Vector3(transform.position.x, transform.position.y, transform.position.z + collectibleSpacing);

            // Instantiate a new collectible, and get the returning gameobject, and then use it for adding to the List of gatherables
            //GameObject go = Instantiate(collectiblePrefabs[index], spawnObstaclePosition2, Quaternion.identity, transform.parent);
            GameObject go = Instantiate(collectiblePrefabs[index], spawnObstaclePosition2, Quaternion.identity, null);

            // Invoke OnCollectibleSpawn
            _gatherables.Add(go.GetComponent<Collectible>());
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void SpawnSideObjects()
    {
        // Spawn right of the road
        int index = Random.Range(0, sidePrefabs.Length);
        // randomize spawning also to give excitement
        int randomizer = Random.Range(0, 3);
        if(randomizer != 0 && spawnedObjectCount++ < sideObjectMax)
        {
            if (sideOffset.Length != 0)
            {
                spawnObstaclePosition3 = new Vector3(sideOffset[index].x, sideOffset[index].y, currentPos + objectSpacing - randomizer);
                GameObject go = Instantiate(sidePrefabs[index], spawnObstaclePosition3, Quaternion.Euler(sideRotation[index].x, sideRotation[index].y, sideRotation[index].z), transform.parent);

                // Invoke OnCollectibleSpawn
                _sideObjects.Add(go.GetComponent<SpeechBubble>());

                if (spawnLeft)
                {
                    // Spawn left of the road
                    index = Random.Range(0, sidePrefabs.Length);
                    spawnObstaclePosition3 = new Vector3(-sideOffset[index].x, sideOffset[index].y, currentPos + objectSpacingLeft);
                    Instantiate(sidePrefabs[index], spawnObstaclePosition3, Quaternion.Euler(-sideRotation[index].x, -sideRotation[index].y, -sideRotation[index].z), transform.parent);
                }
            }
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void SpawnSideObjectsFixed()
    {
        // Spawn Buildings on RIGHT side of the platform...
        spawnObstaclePosition3 = new Vector3(8.4f, -1.0f, currentPos + objectSpacing);
        Instantiate(sidePrefabs[(Random.Range(0, sidePrefabs.Length))], spawnObstaclePosition3, Quaternion.identity, transform.parent);

        // Spawn Buildings on LEFT side of the platform...
        spawnObstaclePosition3 = new Vector3(-8.4f, -1.0f, currentPos + objectSpacing);
        Instantiate(sidePrefabs[(Random.Range(0, sidePrefabs.Length))], spawnObstaclePosition3, Quaternion.identity, transform.parent);
    }

    void SpawnDecorationObjects()
    {
        int index = Random.Range(0, decorationPrefabs.Length);
        if (decorationOffset.Length != 0)
        {
            spawnObstaclePosition4 = new Vector3(decorationOffset[index].x, decorationOffset[index].y, currentPos + decorationSpacing);
            Instantiate(decorationPrefabs[index], spawnObstaclePosition4, Quaternion.identity, transform.parent);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void SpawnDresses()
    {
        int index = Random.Range(0, dressPrefabs.Length);
        if (dressOffset.Length != 0)
        {
            spawnObstaclePosition = new Vector3(dressOffset[index].x, dressOffset[index].y, currentPos + dressSpacing);
            Instantiate(dressPrefabs[index], spawnObstaclePosition, Quaternion.identity, transform.parent);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    void SpawnOther()
    {
        int index = Random.Range(0, 2);

        //index = 0;

        if(index == 0)
        {
            for (int i = 0; i < 4; i++)
            {
                if(isOtherActive[i])
                {
                    spawnObstaclePosition = new Vector3(otherOffset[i].x, otherOffset[i].y, otherOffset[i].z);
                    Instantiate(otherPrefabs[i], spawnObstaclePosition, Quaternion.Euler(otherRotation[i].x, otherRotation[i].y, otherRotation[i].z), transform.parent);
                }
            }
        }
        else if (index == 1)
        {
            for (int i = 4; i < 8; i++)
            {
                if (isOtherActive[i])
                {
                    spawnObstaclePosition = new Vector3(otherOffset[i].x, otherOffset[i].y, otherOffset[i].z);
                    Instantiate(otherPrefabs[i], spawnObstaclePosition, Quaternion.Euler(otherRotation[i].x, otherRotation[i].y, otherRotation[i].z), transform.parent);
                }
            }
        }
        else if (index == 2)
        {
            for (int i = 8; i < 12 ; i++)
            {
                spawnObstaclePosition = new Vector3(otherOffset[i].x, otherOffset[i].y, currentPos + otherSpacing);
                Instantiate(otherPrefabs[i], spawnObstaclePosition, Quaternion.Euler(otherRotation[i].x, otherRotation[i].y, otherRotation[i].z), transform.parent);
            }
        }
        else if (index == 3)
        {
            for (int i = 12; i < 16; i++)
            {
                spawnObstaclePosition = new Vector3(otherOffset[i].x, otherOffset[i].y, currentPos + otherSpacing);
                Instantiate(otherPrefabs[i], spawnObstaclePosition, Quaternion.Euler(otherRotation[i].x, otherRotation[i].y, otherRotation[i].z), transform.parent);
            }
        }
    }
}
