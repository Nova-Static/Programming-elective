using System.Collections;
using UnityEngine;
/// <summary>
/// An example AI.
/// You can modify this template to make your own AI
/// </summary>
public class PondAI : BaseAI
{
    bool detectedRobot;
    public override IEnumerator RunAI() {
        while (true)
        {
            
            yield return Ahead(30);
            yield return TurnRight(180);
            
        }
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public override void OnScannedRobot(ScannedRobotEvent e)
    {
        FireFront(10f);
        detectedRobot = true;
       // Debug.Log("Ship detected: " + e.Name + " at distance: " + e.Distance);
    }
    public override void OnFlagBeingCaptured(FlagBeingCaptured e)
    {
        //FireFront(10f);
        //detectedRobot = true;
    }

    //public override void OnSlopeDetected(SlopeDetectedEvent e)
    //{
    //    base.OnSlopeDetected(e);
    //    Debug.Log("Stuck on slope");
    //}
}