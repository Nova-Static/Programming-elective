using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RamjarAI : BaseAI
{
    private Dictionary<string, RadarBlibInfo> radar = new Dictionary<string, RadarBlibInfo>();
    
    private Vector3 LatestEnemyPos;
    bool enemyInView;
    bool inCombat;
    
    private Vector3 FlagPosition;

    private bool searchingForEnemy = false;
    private bool nearFlag = false;
    
    
    
    // Start is called before the first frame update
    public RamjarAI()
    {
        name = "Ramjars AI";
    }
    // Update is called once per frame
    public override void Update()
    {
        if (!enemyInView && !getCapturingState())
        {
            GoToFlag();
        }
    }
    public override void OnRecordRadarBlib(RadarBlibInfo info)
    {
        Fire(info.transform);
        LatestEnemyPos = info.transform.position;
        
        if (radar.ContainsKey(info.name))
        {
            radar[info.name] = info;
        }
        else
        {
            radar.Add(info.name, info);
        }
    }
    public override void OnFlagInfo(FlagInfo info)
    {
        FlagPosition = info.position;
    }
    private void GoToFlag()
    {
        RotateTo(GetFlagPos() - (GetPosition()));
        MoveForward();
    }
}
