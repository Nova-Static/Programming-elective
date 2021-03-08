using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleporterScript : MonoBehaviour
{
    private Transform CounterTeleporter;

    private teleporterCooldown parentCooldownManager;

    [SerializeField]
    [Range(0, 1)]
    public int childNumber;

    private void Start()
    {
        parentCooldownManager = transform.parent.GetComponent<teleporterCooldown>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Mech"))
        {
            if (childNumber == 0)
            {
                CounterTeleporter = this.gameObject.transform.parent.GetChild(1).gameObject.transform;
            }
            else if (childNumber == 1)
            {
                CounterTeleporter = this.gameObject.transform.parent.GetChild(0).gameObject.transform;
            }
        }
    }

    // Update is called once per frame
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Mech"))
        {
            if (parentCooldownManager)
            {
                parentCooldownManager.teleportGotUsed = true;
                if (parentCooldownManager.teleportCooldown == 5)
                {
                    other.gameObject.transform.position = CounterTeleporter.position;
                }
            }
        }
    }
}
