using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An example AI that is a bit more elaborate than <seealso cref="PondAI"/>.
/// </summary>
public class DarwinsAi : BaseAI
{
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

    public DarwinsAi()
    {
        name = "Darwins Ai";
    }

    public override void Update()
    {
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
        } else if (!delayStart)
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

        } else
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
        FlagPosition = info.position;
    }

    private void delayEnded()
    {
        delaySearchEnemy = standardDelay;
        delayStart = false;
    }

}