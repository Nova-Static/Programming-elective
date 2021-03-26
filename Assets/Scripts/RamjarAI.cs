using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RamjarAI : BaseAI
{
    private Dictionary<string, RadarBlibInfo> radar = new Dictionary<string, RadarBlibInfo>();
    
    private Vector3 _flagPos;
    private bool _closeToFlag = false;
    
    private Vector3 _lastEnemyPos;
    
    private bool _inCombat = false;
    private float _combatTimer;
    private bool _enemyInVision = false;
    private float _enemySpottedTimer;
    private float _timeSinceEnemyFound = 99f;
    
    float _lastHealth;
    
    private bool _runOnStart = false;
    // Start is called before the first frame update
    public RamjarAI()
    {
        name = "Ramjars AI";
    }
    // Update is called once per frame
    public override void Update()
    {
        //check health (logic from SzilardAI)
        if (_lastHealth != GetHealth())
        {
            _lastHealth = GetHealth();
            _combatTimer = Time.time;
            _inCombat = true;
        }
        //reset timer for spotting enemy
        if (_timeSinceEnemyFound > 2) _enemyInVision = false;
        
        //reset combat state after 3 sec
        if (_combatTimer > 2) _inCombat = false;
        
        // rotate to enemy when spotted
        if (_enemyInVision) RotateTo(_lastEnemyPos - GetPosition());
        
        //rotate to enemy when getting attacked from blind spot
        if (!_enemyInVision && _inCombat) RotateTo(_lastEnemyPos - GetPosition());
        
        // Get distance to Flag
        if (Vector3.Distance(GetPosition(), _flagPos) < 5.0f) _closeToFlag = true;
        
        else _closeToFlag = false;
        
        //If the flag is not getting captured or not near it, go to flag pos.
        if (!getCapturingState() || !_closeToFlag)
        {
            //go to flag when no enemy spotted.
            if (!_enemyInVision) GoToFlag();
            //stop mech from moving to flag when enemy spotted.
            else RotateTo(_lastEnemyPos - GetPosition());
        }
        //(logic from SzilardAI)
        _timeSinceEnemyFound = Time.time - _enemySpottedTimer;
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
        _enemyInVision = true;
        //attack target
        Fire(info.transform);
        //get latest position of enemy
        _lastEnemyPos = info.transform.position;
        _enemySpottedTimer = Time.time;
    }
    public override void OnFlagInfo(FlagInfo info)
    {
        _flagPos = info.position;
    }
    private void GoToFlag()
    {
        RotateTo(GetFlagPos() - (GetPosition()));
        MoveForward();
    }
}
