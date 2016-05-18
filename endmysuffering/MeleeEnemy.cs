using UnityEngine;
using System.Collections;

public class MeleeEnemy : MonoBehaviour {

    public GameObject nearestTarget;        //Handle to main target
    public GameObject[] players;            //Array holds every Player

   

    Vector3 direction;                      //direction from this to player/target

    public float distToTarget;              //Handle to distance
    public float maxAttackRange;            //Enemy would stop and attack from this range
    public float attackSpeed;               //Rate that enemy would perform attack
    public float health;
    public float followRange;              //starts following player if reaches this range

    private float attackTimer;              //Do Not Modify

    NavMeshAgent agent;                     //Handle to NavMeshAgent

    GameObject enviroment;

    public int damage;                      //Needs to be negative

    void Start()
    {
        enviroment = GameObject.FindGameObjectWithTag("Enviroment");

        attackTimer = 0;

        players = GameObject.FindGameObjectsWithTag("Player");

        agent = GetComponent<NavMeshAgent>();



        if (maxAttackRange == 0)
        {
            maxAttackRange = 10;
        }
        
    }


    void FixedUpdate()
    {

        if (enviroment.GetComponent<Enviroment>().gameOver)
        {
            Destroy(this.gameObject);
        }

        nearestTarget = FindNearestPlayer(players);
        if (nearestTarget != null)
        {

            if (nearestTarget.GetComponent<TempPlayerScript>().spec != true)
            {
                direction = nearestTarget.transform.position - transform.position;
                distToTarget = Vector3.Distance(nearestTarget.transform.position, transform.position);

                if (distToTarget < followRange)
                {
                    agent.destination = nearestTarget.transform.position;
                }

                agent.stoppingDistance = maxAttackRange;
            }
            else
            {
                FindNearestPlayer(players);
            }
        }
    }
    void Update()
    {
    
        nearestTarget = FindNearestPlayer(players);
        attackTimer += Time.deltaTime;

        if (distToTarget < maxAttackRange && attackTimer > attackSpeed)
        {
            attackTimer = 0;
            Attack();

        }

    }
   public void ManageHealth(int amount)
    {
        
        health -= amount;

        if (health <= 0)
        {
            Destroy(this.gameObject);
        }
    }
    void Attack()
    {
        if (damage > 0)
        {
            damage *= -1;
        }
        nearestTarget = FindNearestPlayer(players);
        RaycastHit hit;

        if (Physics.Raycast(transform.position, direction, out hit)&&nearestTarget.GetComponent<TempPlayerScript>().spec != true)
        {
            

            nearestTarget.GetComponent<TempPlayerScript>().ManageHealth(damage);
        }
        FindNearestPlayer(players);
    }

    GameObject FindNearestPlayer(GameObject[] targets)
    {
        GameObject gMin = null;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = transform.position;


        foreach (GameObject g in targets)
        {
            if (g != null)
            {
                float dist = Vector3.Distance(g.transform.position, currentPos);

                if (dist < minDist && g != null)
                {
                    gMin = g;
                    minDist = dist;
                }
            }

        }


        return gMin;
    }
    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
    {
        if (stream.isWriting)
        {


            Vector3 pos = transform.position;
            stream.Serialize(ref pos);
            stream.Serialize(ref health);

        }
        else {

            Vector3 posReceive = Vector3.zero;
            stream.Serialize(ref posReceive);
            transform.position = Vector3.Lerp(transform.position, posReceive, 1);
            stream.Serialize(ref health);
        }
    }
}
