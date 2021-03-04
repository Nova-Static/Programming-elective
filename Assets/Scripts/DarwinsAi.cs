using System.Collections;
using UnityEngine;

/// <summary>
/// An example AI that is a bit more elaborate than <seealso cref="PondAI"/>.
/// </summary>
public class DarwinsAi : BaseAI
{
    public override IEnumerator RunAI() {
        for (int i = 0; i < 10; i++)
        {
            yield return Ahead(10);
        }
    }

    /// <summary>
    /// <inheritdoc/>
    /// </summary>
    public override void OnScannedRobot(ScannedRobotEvent e)
    {
        Debug.Log("Ship detected: " + e.Name + " at distance: " + e.Distance);
    }

    
}
