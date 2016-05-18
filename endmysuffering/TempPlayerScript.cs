using UnityEngine;
using UnityEngine.Networking;
using System.Collections;

public class TempPlayerScript : NetworkBehaviour
{

    public Camera PlayerCam;        //Set any Camera for this player

    public GameObject firstPerson;  //GameObjects that sets rotation && position of cam (Could be made into a Transform)
    public GameObject spectator;    //GameObjects that sets rotation && position of cam (Could be made into a Transform)

    Vector3 direction;              //Vector3 sets direction

    CharacterController controller; //Handle of Character Controller

    public float gravity;           //Sets Gravity for Character Controller
    public float jump;              //Jump Height for Player (Sets Character controller height)



    public bool third;              //Booleans to keep track
    public bool first;              //of which position the
    public bool spec;               //Camera would be

    private float time;             //handle of time (If needed)

    public float minY;
    public float maxY;

    public float sensX;
    public float sensY;

    private float mouseY;
    private float mouseX;

    public Vector2 center;
    public Vector2 target;
    public float angle;

    private Vector3 moveDirection;
    private float smoothDirection;
    private Vector3 inAirVelocity;
    private float speedInAir;
    [SyncVar]
    public bool readyState;
    GameObject enviroment;
    //class reliant variables and weapon variables: initialized based on class, modified through saves
    public int clipAmmo;
    public int currClipAmmo;
    public int spareAmmo;
    public int currSpareAmmo;

    public bool canFire;
    public int rof;
    public bool automatic;
    public int cdUlt;
    public int currcdUlt;
    public bool infAmmo;
    public int ultActiveTime;
    public int currUltActiveTime;
    public int priDmg;
    public int secDmg;
    public enum charClass { assault, healer, melee, sniper };
    public charClass currClass;
    public bool primaryWeap;
    public int health;            
    public int currHealth;
    public float speed;             //Speed for player
    public int ultValue; //adjusted based on class for both function and value
    public float attackDist;

    public int points;


    void Awake()
    {
        PlayerCam = GameObject.Find("Main Camera").GetComponent<Camera>();
        if (!GetComponent<NetworkView>().isMine)
        {

            enabled = false;
        }


    }
    void Start()
    {

        SetStuff();

    }
    void SetStuff()
    {
        readyState = false;
        minY = -45.0f;
        maxY = 45.0f;
        canFire = true;

        sensY = 100.0f;
        sensX = 100.0f;

        jump = 15;

        currcdUlt = 0;

        infAmmo = false;
        primaryWeap = true;
        gravity = 20.0F;


        moveDirection = Vector3.forward;
        direction = Vector3.zero;
        controller = GetComponent<CharacterController>();

        first = true;
        third = false;
        spec = false;

        enviroment = GameObject.FindGameObjectWithTag("Enviroment");

        center = new Vector2(Screen.width / 2, Screen.height / 2);

        moveDirection = Vector3.forward;
        smoothDirection = 10.0f;
        inAirVelocity = Vector3.zero;
        speedInAir = 1.0f;
        //used for testing
        PlayerPrefs.SetInt("Class", 1);

        if (PlayerPrefs.GetInt("Class") == 1)
        {
            currClass = charClass.assault;
            attackDist = 1000;
            clipAmmo = 30 + PlayerPrefs.GetInt("clipMod");
            currClipAmmo = clipAmmo;
            spareAmmo = 150 + PlayerPrefs.GetInt("spareMod");
            currSpareAmmo = spareAmmo;
            rof = 10 + PlayerPrefs.GetInt("rofMod");
            automatic = true;
            cdUlt = 30 + PlayerPrefs.GetInt("ultcdMod");
            ultActiveTime = 5 + PlayerPrefs.GetInt("ultTimeMod");
            currUltActiveTime = ultActiveTime;
            priDmg = 25 + PlayerPrefs.GetInt("dmgMod");
            secDmg = 5 + PlayerPrefs.GetInt("dmgMod2");
            health = 250 + PlayerPrefs.GetInt("healthMod");
            currHealth = health;
            speed = 5 + PlayerPrefs.GetInt("speedMod");
            ultValue = 5 + PlayerPrefs.GetInt("ultMod");
        }
        else if (PlayerPrefs.GetInt("Class") == 2)
        {
            currClass = charClass.healer;
            attackDist = 1000;
            clipAmmo = 20 + PlayerPrefs.GetInt("clipMod");
            currClipAmmo = clipAmmo;
            spareAmmo = 100 + PlayerPrefs.GetInt("spareMod");
            currSpareAmmo = spareAmmo;
            rof = 5 + PlayerPrefs.GetInt("rofMod");
            automatic = true;
            cdUlt = 60 + PlayerPrefs.GetInt("ultcdMod");
            ultActiveTime = 10 + PlayerPrefs.GetInt("ultTimeMod");
            currUltActiveTime = ultActiveTime;
            priDmg = 20 + PlayerPrefs.GetInt("dmgMod");
            secDmg = 10 + PlayerPrefs.GetInt("dmgMod2");
            health = 100 + PlayerPrefs.GetInt("healthMod");
            currHealth = health;
            speed = 8 + PlayerPrefs.GetInt("speedMod");
            ultValue = 12 + PlayerPrefs.GetInt("ultMod");
        }
        else if (PlayerPrefs.GetInt("Class") == 3)
        {
            //special case for primary
            currClass = charClass.melee;
            attackDist = 5;
            clipAmmo = 0;
            currClipAmmo = clipAmmo;
            spareAmmo = 0;
            currSpareAmmo = spareAmmo;
            rof = 0;
            automatic = false;
            cdUlt = 10 + PlayerPrefs.GetInt("rofMod");
            ultActiveTime = 0;
            currUltActiveTime = ultActiveTime;
            priDmg = 50 + PlayerPrefs.GetInt("dmgMod");
            secDmg = 10 + PlayerPrefs.GetInt("dmgMod2");
            health = 500 + PlayerPrefs.GetInt("healthMod");
            currHealth = health;
            speed = 10 + PlayerPrefs.GetInt("speedMod");
            ultValue = 250 + PlayerPrefs.GetInt("ultMod");
        }
        else if (PlayerPrefs.GetInt("Class") == 4)
        {
            currClass = charClass.sniper;
            attackDist = 1000;
            clipAmmo = 5 + PlayerPrefs.GetInt("clipMod");
            currClipAmmo = clipAmmo;
            spareAmmo = 15 + PlayerPrefs.GetInt("spareMod");
            currSpareAmmo = spareAmmo;
            rof = 0;
            automatic = false;
            cdUlt = 30 + PlayerPrefs.GetInt("rofMod");
            ultActiveTime = 3 + PlayerPrefs.GetInt("ultTimeMod");
            currUltActiveTime = ultActiveTime;
            priDmg = 100 + PlayerPrefs.GetInt("dmgMod");
            secDmg = 15 + PlayerPrefs.GetInt("dmgMod2");
            health = 100 + PlayerPrefs.GetInt("healthMod");
            currHealth = health;
            speed = 8 + PlayerPrefs.GetInt("speedMod");
            ultValue = 1000 + PlayerPrefs.GetInt("ultMod");
        }


    }
    void updateStats()
    {
        clipAmmo += PlayerPrefs.GetInt("clipMod");
        spareAmmo += PlayerPrefs.GetInt("spareMod");
        rof += PlayerPrefs.GetInt("rofMod");
        cdUlt += PlayerPrefs.GetInt("ultcdMod");
        ultActiveTime += PlayerPrefs.GetInt("ultTimeMod");
        priDmg += PlayerPrefs.GetInt("dmgMod");
        secDmg += PlayerPrefs.GetInt("dmgMod2");
        health += PlayerPrefs.GetInt("healthMod");
        speed += PlayerPrefs.GetInt("speedMod");
        ultValue += PlayerPrefs.GetInt("ultMod");
    }
    void FixedUpdate()
    {

        if (GetComponent<NetworkView>().isMine)
        {
            InputMovement();
        }
        weaponManager();

    }
    public void SetState(bool state)
    {
        readyState = state;
    }
    void InputMovement()
    {
        if (enviroment.GetComponent<Enviroment>().StartWave)
        {
            readyState = false;
        }
        if (health <= 0)
        {
            spec = true;
            third = false;
            first = false;
        }
        if (!spec) { 
        if (Input.GetKeyDown(KeyCode.Alpha4))
            readyState = true;

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            spec = true;
            print("In Spector Mode");
            third = false;
            first = false;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            third = true;
            print("In Third Person Mode");
            first = false;
            spec = false;
        }
        else if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            first = true;
            print("In First Person Mode");
            third = false;
            spec = false;
        }

    }
        if (spec)
        {
            Screen.lockCursor = false;
            DeadView(PlayerCam);


        }
        else if (first)
        {
            MoveFirstPerson();
            Screen.lockCursor = true;
            FirstPersonView(PlayerCam);
        }
        else if (third)
        {
            MoveThirdPerson(PlayerCam);
            Screen.lockCursor = false;
            ThirdPersonView(PlayerCam);
        }
    }
    void DeadView(Camera cam)
    {
        direction = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
        cam.transform.position += direction;
    }
        void MoveThirdPerson(Camera cam)
    {




        if (controller.isGrounded)
        {
            direction = new Vector3(Input.GetAxis("Horizontal")*speed, 0, Input.GetAxis("Vertical")*speed);
            cam.transform.TransformDirection(direction);
           
            direction.y = 0.0f;

        }


        direction.y -= gravity * Time.deltaTime;

        controller.Move(direction * Time.deltaTime);
    }
    void MoveFirstPerson()
    {

        if (controller.isGrounded)
        {
            direction = new Vector3(Input.GetAxis("Horizontal")* speed, 0, Input.GetAxis("Vertical")* speed);
            direction = transform.TransformDirection(direction);

           

            if (Input.GetButton("Jump"))
            {
                direction.y = jump;
            }

        }

        direction.y -= gravity * Time.deltaTime;

        controller.Move(direction * Time.deltaTime);

    }
    public void ManageHealth(int amount)
    {
        currHealth += amount;
    }
    public void SpecMode()
    {
        spec = true;
        first = false;
        third = false;


    }

    void FirstPersonView(Camera cam)
    {
        cam.transform.rotation = firstPerson.transform.rotation;

        cam.transform.position = firstPerson.transform.position;

        mouseX += Input.GetAxis("Mouse X") * sensX * Time.deltaTime;
        mouseY += Input.GetAxis("Mouse Y") * sensY * Time.deltaTime;
        mouseY = Mathf.Clamp(mouseY, minY, maxY);

        cam.transform.localEulerAngles = new Vector3(-mouseY, mouseX, 0);
        transform.localEulerAngles = new Vector3(0, mouseX, 0);
    }
    void ThirdPersonView(Camera cam)
    {
        transform.localEulerAngles = new Vector3(0, 0, 0);


        cam.transform.eulerAngles = new Vector3(60, 0, 0);
        cam.transform.position = new Vector3(transform.position.x, transform.position.y + 12, transform.position.z - 6);

        target = (Vector2)Input.mousePosition - center;
        angle = Vector2.Angle(Vector2.up, target);

        if (Input.mousePosition.x > center.x)
            angle = 360 - angle;

        transform.localEulerAngles = new Vector3(0, -angle, 0);





    }

    void weaponManager()
    {
        if (Input.GetKeyDown(KeyCode.E))
            ult();
        if (Input.GetKeyDown(KeyCode.Q))
            primaryWeap = !primaryWeap;
        if (Input.GetKeyDown(KeyCode.R))
            reload();
        if (!canFire)
            return;
        else
        {
            if (primaryWeap)
            {
                if (currClipAmmo != 0 || infAmmo)
                {
                    if (automatic)
                    {
                        if (Input.GetButtonDown("Fire1") && !IsInvoking("Attack"))
                        {
                            
                                InvokeRepeating("Attack", 0.0f, 1.0f / rof);
                            
                           
                        
                        }
                        else if (Input.GetButtonUp("Fire1"))
                            CancelInvoke("Attack");
                    }
                    else
                    {
                        if (Input.GetButtonDown("Fire1"))
                        {

                            Attack();
                           
                           
                        }
                    }
                }
                else
                {
                    CancelInvoke("Attack");
                    if (currSpareAmmo != 0)
                        reload();
                }
            }
            else
            {
                if (Input.GetButtonDown("Fire1"))
                {

                    Attack();
                    
                   
               
                }

            }
        }
    }
    void ult()
    {
        if (currcdUlt == 0)
        {
            currcdUlt = cdUlt;
            if (ultActiveTime != 0)
            {
                if (currClass == charClass.assault)
                {
                    rof += ultValue;
                    infAmmo = true;
                }
                if (currClass == charClass.healer)
                {
                    speed += ultValue;
                    //make a healy field
                }
                if (currClass == charClass.sniper)
                {
                    priDmg += ultValue;
                }
                InvokeRepeating("ultCounter", 1f, 1f);
            }
            else
            {
                //this is the melee guy
                currHealth += ultValue;
                if (currHealth > health)
                    currHealth = health;
                InvokeRepeating("ultCounter", 0f, 1f);
            }
        }
    }
    void ultCounter()
    {
        currcdUlt--;
        if (currcdUlt == 0)
        {
            CancelInvoke("ultCounter");
            currUltActiveTime = ultActiveTime;
            return;
        }
        if (currUltActiveTime >= 0)
            currUltActiveTime--;
        if (currUltActiveTime == 0)
        {
            if (currClass == charClass.assault)
            {
                rof -= ultValue;
                infAmmo = false;
            }
            if (currClass == charClass.healer)
            {
                speed -= ultValue;
                //turn off the field
            }
            if (currClass == charClass.sniper)
            {
                priDmg -= ultValue;
            }
        }

    }
    void Attack()
    {
        if (primaryWeap && !infAmmo)
        {
            if (currClipAmmo <= 0)
            {
                CancelInvoke("Attack");
                reload();
                return;
            }
            else
            {
                currClipAmmo--;
                print(currClipAmmo);
                //implement dmg through pridmg
            }
        }
        else if (!primaryWeap)
        {
            //implement dmg through secdmg
        }
        RaycastHit hit;
        Ray ray;
        if (first)
        {
             ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        }
        
        else if(third)
        {
             ray = new Ray(gameObject.transform.position,gameObject.transform.forward);
        }
        else
        {
            ray = new Ray(transform.position, transform.up);
        }
            if (Physics.Raycast(ray, out hit,attackDist))
            {
                if (hit.collider != null)
                {
                    if (hit.collider.tag == "Melee")
                    {
                        if (primaryWeap)
                        {
                            hit.collider.GetComponent<MeleeEnemy>().ManageHealth(priDmg);
                        }
                        else
                        {
                            hit.collider.GetComponent<MeleeEnemy>().ManageHealth(secDmg);
                        }
                    }
                    else if (hit.collider.tag == "LongRange")
                    {
                        if (primaryWeap)
                        {
                            hit.collider.GetComponent<LongRangeEnemy>().ManageHealth(priDmg);
                        }
                        else
                        {
                            hit.collider.GetComponent<LongRangeEnemy>().ManageHealth(secDmg);
                        }
                    }
                    else if (hit.collider.tag == "MidRange")
                    {
                        if (primaryWeap)
                        {
                            Debug.DrawRay(ray.origin, ray.direction * 10, Color.yellow);
                            hit.collider.GetComponent<MidRangeEnemy>().ManageHealth(priDmg);
                        }
                        else
                        {
                            hit.collider.GetComponent<MidRangeEnemy>().ManageHealth(secDmg);
                        }
                    }
                    else if (hit.collider.tag == "Player" && currClass == charClass.healer)
                    {
                        if (primaryWeap)
                        {
                            Debug.DrawRay(ray.origin, ray.direction * 10, Color.green);
                            hit.collider.GetComponent<TempPlayerScript>().ManageHealth(priDmg);
                        }
                    }
                }
                //DrawRay(ray.origin, ray.direction * 10, Color.yellow);
            }
        
    }
    void reload()
    {
        int diff = clipAmmo - currClipAmmo;
        if (currSpareAmmo >= diff)
        {
            currSpareAmmo = currSpareAmmo - diff;
            currClipAmmo = clipAmmo;
        }
        else
        {
            currClipAmmo += currSpareAmmo;
            currSpareAmmo = 0;
        }
        print("reloaded @ " + currClipAmmo + ":" + currSpareAmmo);
    }

    //[RPC]
    //public bool ReadyStateSync()
    //{
    //    return (readyState);
    //}
    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
    {
        if (stream.isWriting)
        {


            Vector3 pos = transform.position;
            stream.Serialize(ref pos);

        }
        else {

            Vector3 posReceive = Vector3.zero;
            stream.Serialize(ref posReceive);

            transform.position = Vector3.Lerp(transform.position, posReceive, 1);

        }
    }
}