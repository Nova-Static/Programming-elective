using UnityEngine;
using System.Collections;

// Applies an explosion force to all nearby rigidbodies
public class ExplosionForce : MonoBehaviour
{
    public float radius;
    public float power;
    float distance;
  
    bool takingDamage;
    MechController MechController;

    void Awake()
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
                    MechController = hit.gameObject.GetComponent<MechController>();
                    distance = Vector3.Distance(hit.transform.position, transform.position);


                    if (distance > 3.3f)
                    {
                        MechController.healthHealth = MechController.currentHealth;
                        //deal damage according to distance*max damage
                        MechController.healthHealth -= ((Mathf.Clamp01(1 - distance / (radius / 4))) * MechController.Damage);
                        MechController.damaging = true;

                    }
                    //if the distance is more than 3.3
                    else
                    {
                        MechController.healthHealth = MechController.currentHealth;

                        MechController.healthHealth -= MechController.Damage;

                        MechController.damaging = true;

                    }



                }
                rb.AddExplosionForce(power, explosionPos, radius, 10f);
            }

        }
        GetComponent<Collider>().enabled = false;
        Destroy(gameObject, 5f);
    }
   
   
}

