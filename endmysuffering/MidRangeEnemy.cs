using UnityEngine;
using System.Collections;

public class MidRangeEnemy : MonoBehaviour {

    public GameObject nearestTarget;       //Handle to main target
    public GameObject[] players;           //Array holds every Player

    [Range(0.0f, 100f)]
    public float inaccuracy;               //Higher to decrease Accuracy

    Vector3 direction;                     //direction from this to player/target

    public float distToTarget;             //Handle to distance of nearwest target
    public float maxAttackRange;           //Enemy would stop and attack from this range
    public float health;                   //This enemy Health
    public float attackSpeed;              //Rate that enemy would perform attack
    public float followRange;              //starts following player if reaches this range

    private float attackTimer;             //Do Not Modify

    public int damage;                      //Needs to be negative

    NavMeshAgent agent;                    //Handle to NavMeshAgent

    GameObject enviroment;

    void Start()
    {
        enviroment = GameObject.FindGameObjectWithTag("Enviroment");

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

        attackTimer += Time.deltaTime;

        if (distToTarget < maxAttackRange && attackTimer > attackSpeed)
        {
            attackTimer = 0;
            ShootTarget();

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
    void ShootTarget()
    {
        if (damage > 0)
        {
            damage *= -1;
        }
        

        RaycastHit hit;
        Vector3 bulletVelocity = direction;

        if (inaccuracy != 0)
        {
            Vector2 rand = Random.insideUnitCircle;
            bulletVelocity += new Vector3(rand.x, 0, rand.y) * inaccuracy;
            bulletVelocity = bulletVelocity.normalized * maxAttackRange;
        }
        if (Physics.Raycast(transform.position, bulletVelocity, out hit))
        {

            nearestTarget.GetComponent<TempPlayerScript>().ManageHealth(damage);
        }
       


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
