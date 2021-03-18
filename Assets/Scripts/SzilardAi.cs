using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SzilardAi : BaseAI
{
   
    Dictionary<string, RadarBlibInfo> radar = new Dictionary<string, RadarBlibInfo>();

    Vector3 MoveToPoint = Vector3.zero;

    string targetName = "Manno";
    bool enemyFound;



    public SzilardAi()
    {
        name = "Szilard Ai";
    }

    public override void Update()
    {
        if (radar.ContainsKey(targetName))
        {
            //Seek(radar[targetName].position);
        }
        if (!enemyFound)
        {
            MoveForward();
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
        enemyFound = true;
    }

    public override void OnFlagBeingCaptured(FlagBeingCaptured e)
    {

    }
    
}