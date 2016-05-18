using UnityEngine;
using System.Collections;

public class Spawner : MonoBehaviour {
    public GameObject longEnemy;
    public GameObject ShortEnemy;
    public GameObject meleeEnemy;

    public GameObject[] enemies;
    public GameObject playerPrefab;
    public GameObject[] players;

    public int totalEnemies;

   
    //public Transform enemyPrefab;
    //public GameObject enemySpawn;

    void OnServerInitialized()
    {
        //SpawnEnemy();
        Spawnplayer();
    }

    void OnConnectedToServer(){
		Spawnplayer();
	}
    void FixedUpdate()
    {
        players = GameObject.FindGameObjectsWithTag("Player");

        //foreach (GameObject g in players)
        //{
        //    if(!g.GetComponent<TempPlayerScript>().enabled)
        //    {
        //        g.GetComponent<TempPlayerScript>().enabled = !g.GetComponent<TempPlayerScript>().enabled;
        //    }

        //}
    }
	public void SpawnEnemy(Vector3 positionA,int numOfEnemies, int wave)
    {
        //This Function Spawns player Obj.
        /*Transform myNewTrans =*/
        //Spawns player where the Empty Game object called Spawner is
        // Network.Instantiate(enemyPrefab, enemySpawn.transform.position, enemySpawn.transform.rotation, 0);
        enemies = new GameObject[] { longEnemy, ShortEnemy, meleeEnemy };

        for (int i = 0; i < numOfEnemies; i++)
        {
            
             Network.Instantiate(enemies[i % 3], new Vector3(Random.Range(positionA.x - 5, positionA.x + 5), 0, Random.Range(positionA.z - 5, positionA.z + 5)), Quaternion.identity, 0) ;
            totalEnemies++;
            
     //      if ((handle.GetComponent("LongRangeEnemy") as LongRangeEnemy) != null)
     //       {
                /*Set Values for enemy
                health   
                Damage 
                etc
                handle.GetComponent<LongRangeEnemy>().health--;
                */
      //      }
      //      if ((handle.GetComponent("MeleeEnemy") as MeleeEnemy) != null)
       //     {
                /*Set Values for enemy
                health   
                Damage 
                etc
                handle.GetComponent<MeleeEnemy>().health--;
                */
       //     }
       //     if ((handle.GetComponent("MidRangeEnemy") as MidRangeEnemy) != null)
       //     {
                /*Set Values for enemy
                health   
                Damage 
                etc
                handle.GetComponent<MidRangeEnemy>().health--;
                */
        //    }
            
        }
        ////             Network.Instantiate          \\\\\\\\\\\\\\
    }
    public int GetNumEnemies()
    {
        GameObject[] longRange;
        GameObject[] midRange;
        GameObject[] melee;
        int numEn;


        longRange = GameObject.FindGameObjectsWithTag("LongRange");
        melee = GameObject.FindGameObjectsWithTag("Melee");
        midRange = GameObject.FindGameObjectsWithTag("MidRange");

        numEn = longRange.Length + melee.Length + midRange.Length;

        return numEn;

    }
    void Spawnplayer(){
		//This Function Spawns player Obj.
		/*Transform myNewTrans =*/ Network.Instantiate(playerPrefab, transform.position, transform.rotation, 0);
		//Spawns player where the Empty Game object called Spawner is

	}	
	void OnPlayerDisconnected(NetworkPlayer player) {
		Debug.Log("Clean up after player " + player);
		Network.RemoveRPCs(player);
		Network.DestroyPlayerObjects(player);
	}
	//This removes player when they disconnect
	void OnDisconnectedFromServer(NetworkDisconnection info) {
		Debug.Log("Clean up a bit after server quit");
		Network.RemoveRPCs(Network.player);
		Network.DestroyPlayerObjects(Network.player);
		Application.LoadLevel(Application.loadedLevel);
	}

}


