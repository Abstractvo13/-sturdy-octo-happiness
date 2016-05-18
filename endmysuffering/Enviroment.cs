using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class Enviroment : MonoBehaviour {

   
    public  Spawner spawner;    //Lets us accesss the spawn method

    Vector3 spawnPoint1, spawnPoint2, spawnPoint3, spawnPoint4, spawnPoint5, spawnPoint6, spawnPoint7, spawnPoint8; // Spawn locations

    public Vector3[] spawnLocations;//Array to keep track of different spawn locations

    public bool StartWave;          //boolean to start spawning
    public int numOfEnOnWave;             //Number of enemies spawn in one location (Has to be greater than 3 or else will only spawn one type of enemy)
    public int waveNum = 0;         //Keeps track of current wave 

    public GameObject[] players;           //Array holds every Player

    public GameObject[] longRange;
    public GameObject[] midRange;
    public GameObject[] melee;


    public bool gameOver;
    public int liveEnemies;

   public Vector3 specCamPos;
   public  Vector3 specPlayerPos;
    public Vector3 playerSpawnPos;

    public int wavePoints;

    void OnServerInitialized() {

	}
    void Start()
    {
        gameOver = false;
        StartWave = false;

        playerSpawnPos = new Vector3(250,12,45);
        specPlayerPos = new Vector3(144,180,141);
        specCamPos = new Vector3(144,150,141);

        spawnPoint1 = new Vector3(32,12,33);
        spawnPoint2 = new Vector3(32, 12, 148);
        spawnPoint3 = new Vector3(32, 12, 277);
        spawnPoint4 = new Vector3(85, 12, 267);
        spawnPoint5 = new Vector3(253, 12, 266);
        spawnPoint6 = new Vector3(253, 12, 156);
        spawnPoint7 = new Vector3(157, 12, 156);
        spawnPoint8 = new Vector3(121, 12, 102);

        spawnLocations = new Vector3[] { spawnPoint1, spawnPoint2, spawnPoint3, spawnPoint4, spawnPoint5, spawnPoint6, spawnPoint7, spawnPoint8 };

    }
    void Update ()
    {

        players = spawner.GetComponent<Spawner>().players;

        longRange = GameObject.FindGameObjectsWithTag("LongRange");
        melee = GameObject.FindGameObjectsWithTag("Melee");
        midRange = GameObject.FindGameObjectsWithTag("MidRange");

        if (StartWave && longRange.Length == 0 && melee.Length == 0 && midRange.Length == 0)
        {

            SpawnWave();

        }

        if (CheckState())
        {
            StartWave = true;
        }


        DeadPlayers();
        CheckNumEnemies();

	}
    public void DeadPlayers()
    {
        foreach (GameObject g in players)
        {
            if (g.GetComponent<TempPlayerScript>().currHealth <= 0)
            {
                g.transform.position = specPlayerPos;
                g.GetComponent<TempPlayerScript>().PlayerCam.transform.position = specPlayerPos;
            }
        }
    }
    public bool CheckState()
    {
            
        if (players.Length == 0)
        {
            return false;
        }

        

         if (players[0].GetComponent<TempPlayerScript>().readyState)
         {
             return true;
         }
            
         else
             return false;


    }
    public void CheckNumEnemies()
    {
        liveEnemies = spawner.GetComponent<Spawner>().GetNumEnemies();
    }

    void SpawnWave()
    {

        foreach (GameObject g in players)
        {
            g.GetComponent<TempPlayerScript>().points += wavePoints;

            g.GetComponent<TempPlayerScript>().first = true;
            g.GetComponent<TempPlayerScript>().third = false;
            g.GetComponent<TempPlayerScript>().spec = false;

            g.transform.position = playerSpawnPos;

            g.GetComponent<TempPlayerScript>().currHealth = g.GetComponent<TempPlayerScript>().health;
            g.GetComponent<TempPlayerScript>().currClipAmmo = g.GetComponent<TempPlayerScript>().clipAmmo;
            g.GetComponent<TempPlayerScript>().currSpareAmmo = g.GetComponent<TempPlayerScript>().spareAmmo;

        }

        StartWave = false;
        waveNum++;
        numOfEnOnWave = waveNum * 5;
        wavePoints = 10 * waveNum;

        spawner.GetComponent<Spawner>().SpawnEnemy(spawnLocations[Random.Range(0, spawnLocations.Length)], numOfEnOnWave, waveNum);
    }
    
}
