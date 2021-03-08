using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.other.gameObject.tag.Equals("Mech"))
        {
            if (collision.other.gameObject.GetComponent<MechController>() != null)
            {

            }
        }
    }
}
