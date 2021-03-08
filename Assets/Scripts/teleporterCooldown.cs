using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleporterCooldown : MonoBehaviour
{

    public bool teleportGotUsed = false;
    public float teleportCooldown = 5f;

    private void Update()
    {
        if ((teleportGotUsed) && (teleportCooldown >= 0f))
        {
            teleportCooldown -= Time.deltaTime;
            Debug.Log(teleportCooldown);
        }

        if (teleportCooldown < 0f)
        {
            Debug.Log("Reset Cooldown");
            teleportGotUsed = false;
            teleportCooldown = 5f;
        }
    }
}
