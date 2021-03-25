using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SzilardAi : BaseAI
{
   
    Dictionary<string, RadarBlibInfo> radar = new Dictionary<string, RadarBlibInfo>();
    List<Vector3> telePos; 
    Vector3 MoveToPoint = Vector3.zero;

    string targetName = "Manno";
    
    

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

    public SzilardAi()
    {
        name = "Szilard Ai";
    }

    public override void Start()
    {
        Obstacles = GameObject.FindObjectsOfType<Obstacle>();
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
        if (underAttack && !enemyInView)
        {
            MoveForward();
            Rotate(RotateDirection.Right);
        }
        if (enemyInView && !underAttack)
        {
            if (Vector3.Distance(GetPosition(), targetLastPos) > 8f)
            {
                MoveForward();
            }
            RotateTo(targetLastPos - (GetPosition()));
        }
        else if(enemyInView && underAttack)
        {
            MoveForward();
            Rotate(RotateDirection.Right);
        }

        if (!enemyInView && !underAttack)
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
        foreach (var obstacle in Obstacles)
        {
            float d = 1 / Vector3.Distance(obstacle.transform.position, GetPosition());
            if (d > .1f)
            {
                Vector3 force = (GetPosition() - obstacle.transform.position).normalized;
                force *= d;
                force *= obstacle.Force;
                velocity = (velocity + force).normalized;
                GetTransfrom().Translate(velocity * Time.deltaTime);
            }
        }

        timeSinceLastHit = Time.time - lastHitTime;
        timeSinceEnemyFound = Time.time - timeEnemyFound;
        
    }
   
    private void RoamFlag()
    {
        MoveForward();
        Rotate(RotateDirection.Right);
        Rotate(RotateDirection.Right);
    }
    private void GoBackToFlag()
    {
        RotateTo(GetFlagPos() - (GetPosition()));
        MoveForward();
    }
    public override void OnRecordRadarBlib(RadarBlibInfo info)
    {
        if (underAttack)
        {
            enemyInView = true;

        }

        //Seek(radar[info.name].position);
        Fire(info.transform);

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
}