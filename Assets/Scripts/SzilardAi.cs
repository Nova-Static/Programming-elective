using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SzilardAi : BaseAI
{
   
    Dictionary<string, RadarBlibInfo> radar = new Dictionary<string, RadarBlibInfo>();
    List<Vector3> telePos; 
    Vector3 MoveToPoint = Vector3.zero;


    Vector3 targetPos;
    bool obstacleInWay;

    [Header("In Combat")]
    float lastHealth;
    float lastHitTime;
    float timeSinceLastHit = 99f;
    bool underAttack;
    bool enemyInView;

    [Header("On Lookout")]
    bool enemyFound;
    float timeEnemyFound;
    float timeSinceEnemyFound = 99f;

    [Header("Target")]
    Vector3 targetLastPos;
    Quaternion angleToTarget;

    [Header("Flag")]
    float distanceToFlag;

    Obstacle[] Obstacles;
    private Vector3 velocity = Vector3.zero;
    State aiStates;
    public SzilardAi()
    {
        name = "Szilard Ai";
    }

    public override void Start()
    {
        lastHealth = GetHealth();
        Obstacles = GameObject.FindObjectsOfType<Obstacle>();
        aiStates = State.Roaming;
    }
    public override void Update()
    {
    
        if (lastHealth != GetHealth())
        {
            lastHealth = GetHealth();
            lastHitTime = Time.time;
            underAttack = true;
        }
        if (timeSinceLastHit > 4f)
        {
            underAttack = false;
        }
        if (timeSinceEnemyFound > 3f)
        {
            enemyInView = false;
        }

        switch (aiStates)
        {
            case State.Attacked:
                HandleMovement();
                break;
            case State.Pursuing:
                PurseHandler();
                break;
            case State.InCombat:
                inCombatHandler();
                break;
            case State.Roaming:
                roamHandler();
                break;
        }

        if (underAttack && !enemyInView)
        {
            aiStates = State.Attacked;

        }
        else if (enemyInView && !underAttack)
        {
            aiStates = State.Pursuing;
        }
        else if (enemyInView && underAttack)
        {
            aiStates = State.InCombat;

        }
        else if (!enemyInView && !underAttack)
        {
            aiStates = State.Roaming;
        }
        
        timeSinceLastHit = Time.time - lastHitTime;
        timeSinceEnemyFound = Time.time - timeEnemyFound;
        
    }
    private void PurseHandler()
    {
        if (Vector3.Distance(GetPosition(), targetLastPos) > 8f)
        {
            HandleMovement();

        }
        RotateTo(targetLastPos - GetPosition());
    }
    private void roamHandler()
    {
        distanceToFlag = Vector3.Distance(GetPosition(), GetFlagPos());

        if (distanceToFlag >= 10f)
        {
            GoBackToFlag();


        }
        else
        {
            if (distanceToFlag <= 40f && distanceToFlag >= 3f && !getCapturingState())
            {
                RoamFlag();
            }
        }
        if (getCapturingState() && distanceToFlag >= 5f)
        {
            GoBackToFlag();
        }
    }
    private void inCombatHandler()
    {
        HandleMovement();

        float angle = AngleDir(GetForwardPos(), targetLastPos, GetTransfrom().up);
        if (angle == -1)
        {
            Rotate(RotateDirection.Left);
        }
        else if (angle == 1)
        {
            Rotate(RotateDirection.Right);
        }
    }
    private void HandleMovement()
    {
        obstacleInWay = false;
        Vector3 fwd = GetTransfrom().TransformDirection(new Vector3(0f, 0f, 1f));
        Vector3 right = GetTransfrom().TransformDirection(new Vector3(0.45f, 0f, 0));
        Vector3 left = GetTransfrom().TransformDirection(new Vector3(-0.45f, 0f, 0));
        Vector3 playerpos = new Vector3(GetTransfrom().position.x, GetTransfrom().position.y + 1f, GetTransfrom().position.z);
        RaycastHit hit;
        if (Physics.Raycast(playerpos, fwd, out hit, 9))
        {

            if (hit.collider.gameObject.GetComponent<Obstacle>() != null)
            {
                obstacleInWay = true;
                Rotate(RotateDirection.Right);
            }
        }
        RaycastHit hit2;
        if (Physics.Raycast(playerpos, right, out hit2, 9))
        {
            if (hit2.collider.gameObject.GetComponent<Obstacle>() != null)
            {
                obstacleInWay = true;
                Rotate(RotateDirection.Left);
            }
        }
        RaycastHit hit3;
        if (Physics.Raycast(playerpos, left, out hit3, 9))
        {
            if (hit3.collider.gameObject.GetComponent<Obstacle>() != null)
            {
                obstacleInWay = true;
                Rotate(RotateDirection.Right);
            }
        }
        MoveForward();
    }
    public static float AngleDir(Vector3 fwd, Vector3 targetDir , Vector3 up ) {
        Vector3 perp  = Vector3.Cross(fwd, targetDir);
        float dir = Vector3.Dot(perp, up);
   
        if (dir > 0.0) {
            return 1.0f;
        } else if (dir< 0.0) {
            return -1.0f;
        } else
        {
            return 0.0f;
        }
    }
    private void RoamFlag()
    {
        MoveForward();
        if (!obstacleInWay)
        {
            Rotate(RotateDirection.Right);
            Rotate(RotateDirection.Right);
        }
    }
    private void GoBackToFlag()
    {
        if (!obstacleInWay)
        {
            RotateTo(GetFlagPos() - (GetPosition()));
        }
        //MoveForward();
        //
        HandleMovement();
    }
    public override void OnRecordRadarBlib(RadarBlibInfo info)
    {
        Fire(info.transform);
        if (underAttack)
        {
            enemyInView = true;

        }

        //Seek(radar[info.name].position);
       

        targetLastPos = info.position;
        timeEnemyFound = Time.time;
        enemyFound = true;
    }

    public override void OnFlagBeingCaptured(FlagBeingCaptured e)
    {
        
    }

    public override void OnTeleportersInfo(TeleportersInfo info)
    {
        if (!telePos.Contains(info.position))
        {
            telePos.Add(info.position);
        }
    }

    private Vector3 get_ClosestTeleporterPos()
    {
        float distance = 9999f;
        Vector3 locOfClosestPos = Vector3.zero;
           
        foreach (var tPos in telePos)
        {
            if (Vector3.Distance(GetPosition(), tPos) < distance){
                distance = Vector3.Distance(GetPosition(), tPos );
                locOfClosestPos = tPos;
            }
        }
        return locOfClosestPos;
    }
    enum State
    {
        
        Attacked,
        Pursuing,
        InCombat,
        Roaming

    }
}