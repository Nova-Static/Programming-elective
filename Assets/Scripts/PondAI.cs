﻿using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// An example AI that is a bit more elaborate than <seealso cref="PondAI"/>.
/// </summary>
public class PondAI : BaseAI
{
    Dictionary<string, RadarBlibInfo> radar = new Dictionary<string, RadarBlibInfo>();

    Vector3 MoveToPoint = Vector3.zero;

    string targetName = "Manno";

    public PondAI()
    {
        name = "Pond Ai";
    }

    public override void Update()
    {
        if (radar.ContainsKey(targetName))
        {
            //Seek(radar[targetName].position);
        }
    }

    public override void OnRecordRadarBlib(RadarBlibInfo info)
    {

        //Seek(radar[info.name].position);
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
}
