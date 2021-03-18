using UnityEngine;
using System.Collections;

// Applies an explosion force to all nearby rigidbodies
public class ExplosionForce : MonoBehaviour
{
    public float radius;
    public float power;
    float distance;

    MechController MechController;

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
                    MechController=hit.gameObject.GetComponent<MechController>();
                    distance = Vector3.Distance(hit.transform.position, transform.position);
                    Debug.Log("distance: "+distance);
                    if (distance > 3.3f)
                    {
                        //deal damage according to distance*max damage
                        MechController.currentHealth -= ((Mathf.Clamp01(1 - distance / (radius/4))) * MechController.Damage);
                        Debug.Log("damage from distance: "+((Mathf.Clamp01(1 - distance / (radius/4))) * MechController.Damage));
                        MechController.healthbar.SetHealth(Mathf.RoundToInt(MechController.currentHealth));
                    }
                    //if the distance is more than 3.3
                    else
                    {

                        MechController.currentHealth -= MechController.Damage;
                        MechController.healthbar.SetHealth(Mathf.RoundToInt(MechController.currentHealth));
                        Debug.Log("maxDamage: " + MechController.Damage);
                     

                    }

                 
                }
                 rb.AddExplosionForce(power, explosionPos, radius,10f);
            }
           
        }
        GetComponent<Collider>().enabled = false;
        Destroy(gameObject,5f);
    }
}

