using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An example AI that is a bit more elaborate than <seealso cref="PondAI"/>.
/// </summary>
public class StanimirAI : BaseAI
{
    [SerializeField]
    float Speed = 1;



    public Obstacle[] Obstacle;

    bool chose;
    bool left;
    private Dictionary<string, RadarBlibInfo> radar = new Dictionary<string, RadarBlibInfo>();
    bool patrolling = true;
    private Vector3 FlagPosition;

    private bool searchingForEnemy = false;
    private bool nearFlag = false;

    private Vector3 EnemyPosition;
    float delay = 2f;
    private float standardDelay = 1.0f;
    private float delaySearchEnemy = 1.0f;
    private bool delayStart = false;
    float prevHealth;
    bool proceed;
    private GameObject FlagTransform = new GameObject();
    private bool gotFlagTranform = false;
    public GameObject self;
    Rigidbody rb;
    bool DoOnce;
    bool underFire = false;

    public StanimirAI()
    {
        name = "Stanimir's Ai";

    }


    public override void Update()
    {
        if (DoOnce == false)
        {
            DoOnce = true;


            Obstacle = GameObject.FindObjectsOfType(typeof(Obstacle)) as Obstacle[];

            prevHealth = GetHealth();
        }
        if (!getCapturingState())
        {
            MoveForward();
        }

        if (GetHealth() != prevHealth)
        {
            prevHealth = GetHealth();
            underFire = true;

        }

        if (underFire && EnemyPosition != null)
        {
            Rotate(RotateDirection.Left);
        }

        // Get distance to Flag
        if (Vector3.Distance(GetPosition(), FlagPosition) < 3.0f)
        {
            nearFlag = true;
        }
        else
        {
            nearFlag = false;
        }


        if (!getCapturingState() && !underFire && EnemyPosition == null)
        {
            patrolling = true;
        }






    }




    public override void StartDetection(WallDetection detecting)
    {

        if (detecting.detecting == true)
        {
            proceed = false;




            Rotate(RotateDirection.Left);

        }
        else
        {



            if (getCapturingState())
            {
                if (Vector3.Distance(GetPosition(), FlagPosition) > 3f)
                {
                    MoveForward();
                }
                else
                {
                    Debug.Log("middle");
                    Rotate(RotateDirection.Right);
                }
                patrolling = false;
            }
            delay -= Time.deltaTime;

            if (delay <= 0.0f)
            {
                delay = 2;
                proceed = true;
            }

            if (patrolling)
            {

                if (proceed)
                {
                    chose = false;
                    if (Vector3.Distance(GetPosition(), FlagPosition) > 15.0f)
                    {
                        if (Vector3.Distance(GetPosition(), FlagPosition) > 25.0f)
                        {
                            if (Vector3.Angle(GetForwardDirection(), FlagPosition - GetPosition()) > 5f)
                            {
                                RotateTo(FlagPosition - (GetPosition()));

                            }
                        }
                        else
                        {

                            if (Vector3.Angle(GetForwardDirection(), FlagPosition - GetPosition()) <= 60)
                            {
                                Debug.Log("patollspot");
                                Rotate(RotateDirection.Right);

                            }
                        }
                    }

                }
            }
            else
            {
                if (Vector3.Distance(EnemyPosition, GetPosition()) > 10.0f)
                    RotateTo(EnemyPosition - GetPosition());
            }
        }
    }
    public override void OnRecordRadarBlib(RadarBlibInfo info)
    {

        if (radar.ContainsKey(info.name))
        {
            radar[info.name] = info;
        }
        else
        {
            radar.Add(info.name, info);
        }


        // set the latest enemy pos and stop searching for enemy
        EnemyPosition = info.transform.position;

        // fire at target
        Fire(info.transform);

        patrolling = false;
    }

    public override void OnFlagInfo(FlagInfo info)
    {
        FlagPosition = info.position;
    }




}