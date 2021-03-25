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

    [SerializeField]
    Transform Target = null;

    [SerializeField]
    public Obstacle[] Obstacle;
    Vector3 velocity;
    Vector3 force;
    float distance;

    private Dictionary<string, RadarBlibInfo> radar = new Dictionary<string, RadarBlibInfo>();

    private Vector3 FlagPosition;

    private bool searchingForEnemy = false;
    private bool nearFlag = false;

    private Vector3 LatestEnemyPos;

    private float standardDelay = 1.0f;
    private float delaySearchEnemy = 1.0f;
    private bool delayStart = false;

    private GameObject FlagTransform = new GameObject();
    private bool gotFlagTranform = false;
    public GameObject self;

    bool DoOnce;
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
            self = GameObject.Find(name);
           
        }
        if (self != null)
        {

            foreach (var detector in Obstacle)
            {
                distance = 1 / Vector3.Distance(detector.transform.position, self.transform.position);
                if (distance > .1f&&self!=null)
                {
                    Debug.Log(detector.name);

                    force = (self.transform.position - detector.transform.position).normalized;



                    force *= distance;

                    force *= detector.Force;

                    velocity = (velocity + force).normalized;
                   // self.transform.Translate(velocity * Speed * Time.deltaTime);
                }
            }
        }
        if (!gotFlagTranform)
        {
            FlagTransform.transform.position = FlagPosition;
            gotFlagTranform = true;
        }


        // Get distance to Flag
        if (Vector3.Distance(GetPosition(), FlagPosition) < 5.0f) { nearFlag = true; }
        else { nearFlag = false; }

        // If not capturing and also not near the flag try getting to the flag
        if (!getCapturingState() || !nearFlag)
        {
            if (!delayStart)
            {
                if (Vector3.Angle(GetForwardDirection(), FlagPosition - GetPosition()) > 5)
                {
                    RotateTo(FlagPosition - (GetPosition()));
                }
                else
                {
                    MoveForward();
                }
            }
        }
        else if (!delayStart)
        {
            Rotate(RotateDirection.Left);
        }

        if (delayStart)
        {
            delaySearchEnemy -= Time.deltaTime;
            if (LatestEnemyPos != null)
            {
                RotateTo(LatestEnemyPos - GetPosition());
            }
        }

        if (delaySearchEnemy <= 0.0f)
        {
            delayEnded();

        }
        else
        {
            searchingForEnemy = false;
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
        LatestEnemyPos = info.transform.position;

        // fire at target
        Fire(info.transform);

        // start the delay so the mech stays in fight mode and doesnt walk or rotate aimlessly
        delayStart = true;

    }

    public override void OnFlagInfo(FlagInfo info)
    {
        Debug.Log("OnFlagInfo");
        FlagPosition = info.position;
    }

    private void delayEnded()
    {
        Debug.Log("delayEnded");

        delaySearchEnemy = standardDelay;
        delayStart = false;
    }

}