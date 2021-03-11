using UnityEngine;
using System.Collections;

// Applies an explosion force to all nearby rigidbodies
public class ExplosionForce : MonoBehaviour
{
    public float radius;
    public float power;

    void Start()
    {
        Vector3 explosionPos = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPos, radius);
        foreach (Collider hit in colliders)
        {

            Rigidbody rb = hit.GetComponent<Rigidbody>();

            if (rb != null)
            {
                if (hit.tag == "Mech")
                {
                    power /= 2;
                }
                 rb.AddExplosionForce(power, explosionPos, radius,10f);
            }
        }
        GetComponent<Collider>().enabled = false;
        Destroy(gameObject,5f);
    }
}

