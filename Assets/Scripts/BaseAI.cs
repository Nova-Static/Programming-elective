using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// The event data that is generated when another participant in the arena was 'seen'
/// </summary>
public class ScannedRobotEvent {
    public string Name;
    public float Distance; 
}

public class BaseAI
{
    public MechController Mech = null;

    /// <summary>
    /// Another participant was 'seen'. Do something with the info stored in the even data
    /// </summary>
    /// <param name="e">The event data</param>
    public virtual void OnScannedRobot(ScannedRobotEvent e)
    {
        // 
    }

    /// <summary>
    /// Move this Mech ahead by the given distance
    /// </summary>
    /// <param name="distance">The distance to move</param>
    /// <returns></returns>
    public IEnumerator Ahead(float distance) {
        yield return Mech.__Ahead(distance);
    }

    /// <summary>
    /// Move the Mech backwards by the given distance
    /// </summary>
    /// <param name="distance">The distance to move</param>
    /// <returns></returns>
    public IEnumerator Back(float distance) {
        yield return Mech.__Back(distance);
    }

    /// <summary>
    /// Turns the Mech left by the given angle
    /// </summary>
    /// <param name="angle">The angle to rotate</param>
    /// <returns></returns>
    public IEnumerator TurnLeft(float angle) {
        yield return Mech.__TurnLeft(angle);
    }

    /// <summary>
    /// Turns the Mech right by the given angle
    /// </summary>
    /// <param name="angle">The angle to rotate</param>
    /// <returns></returns>
    public IEnumerator TurnRight(float angle) {
        yield return Mech.__TurnRight(angle);
    }

    /// <summary>
    /// Fire from the forward pointing cannon
    /// </summary>
    /// <param name="power">???</param>
    /// <returns></returns>
    public IEnumerator FireFront(float power) {
        yield return Mech.__FireFront(power);
    }

    public virtual IEnumerator RunAI() {
        yield return null;
    }
}
