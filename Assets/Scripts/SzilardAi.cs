using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SzilardAi : BaseAI
{
   
    Dictionary<string, RadarBlibInfo> radar = new Dictionary<string, RadarBlibInfo>();
    List<Vector3> telePos; 
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
            if (Vector3.Distance(tPos, GetPosition()) < distance){
                distance = Vector3.Distance(tPos, GetPosition());
                locOfClosestPos = tPos;
            }
        }
        return locOfClosestPos;
    }
}