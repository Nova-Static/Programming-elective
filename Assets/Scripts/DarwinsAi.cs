using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An example AI that is a bit more elaborate than <seealso cref="PondAI"/>.
/// </summary>
public class DarwinsAi : BaseAI
{
    Dictionary<string, RadarBlibInfo> radar = new Dictionary<string, RadarBlibInfo>();

    Vector3 MoveToPoint = Vector3.zero;

    Vector3 FlagPosition;

    bool searchingForEnemy = false;

    public DarwinsAi()
    {
        name = "Darwins Ai";
    }

    public override void Update()
    {


        if (!getCapturingState())
        {
            Debug.Log(getCapturingState());
            if (Vector3.Angle(GetForwardDirection(), FlagPosition - GetPosition()) > 5)
            {
                RotateTo(FlagPosition - (GetPosition()));
            }
            else
            {
                MoveForward();
            }


        } else
        {
            searchingForEnemy = true;
            Debug.Log(getCapturingState());
        }

        if ((searchingForEnemy && getCapturingState()))
        {
            RotateTo(new Vector3(0f, 0f, 45f));
        }

    }

    public override void OnRecordRadarBlib(RadarBlibInfo info)
    {
        searchingForEnemy = false;
        Fire(info.transform);

        if (radar.ContainsKey(info.name))
        {
            radar[info.name] = info;
        }
        else
        {
            radar.Add(info.name, info);
        }


    }

    public override void OnFlagBeingCaptured(FlagBeingCaptured e)
    {
    }

    public override void OnFlagInfo(FlagInfo info)
    {
        FlagPosition = info.position;
    }
}
