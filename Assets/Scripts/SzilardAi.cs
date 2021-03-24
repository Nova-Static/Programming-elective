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

    public SzilardAi()
    {
        name = "Szilard Ai";
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
        if (enemyInView)
        {
            if (Vector3.Distance(GetPosition(), targetLastPos) > 8f)
            {
                MoveForward();
            }
            RotateTo(targetLastPos - (GetPosition()));
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
            if (getCapturingState() && distanceToFlag >= 3f)
            {
                GoBackToFlag();
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
        //if (Vector3.Angle(GetForwardDirection(), GetFlagPos() - GetPosition()) > 5)
        //{
        //    RotateTo(GetFlagPos() - (GetPosition()));
        //}
        //Vector3 targetFlag = new Vector3(10.3f, -3.1f, 1.6f);
        //Debug.Log(GetFlagPos());
        //RotateTo(targetFlag);
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