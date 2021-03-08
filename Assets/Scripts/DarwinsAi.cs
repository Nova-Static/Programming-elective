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

    string targetName = "Manno";

    public DarwinsAi()
    {
        name = "Darwins Ai";
    }

    public override void Update()
    {
        if (radar.ContainsKey(targetName))
        {
            //Seek(radar[targetName].position);
        }

        MoveForward();
    }

    public override void OnRecordRadarBlib(RadarBlibInfo info)
    {

        //Seek(radar[info.name].position);
        Fire();
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
        Debug.Log(e.Name);
    }
}
