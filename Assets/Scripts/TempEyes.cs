using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempEyes : MonoBehaviour
{
    public GameObject Eyes;
    GameObject eyes2;
    LayerMask mask;

    void Start()
    {
        mask = LayerMask.GetMask("Mech");
        mask = ~mask;
    }

    // Update is called once per frame
    void OnTriggerStay(Collider other)
    {

        if (other.tag == "Mech")
        {
            eyes2 = other.gameObject.transform.GetChild(2).gameObject;

           
            if (Physics.Linecast(Eyes.transform.position, eyes2.transform.position, mask))
            {
                Debug.Log("blocked");
            }
            else
            {
                Debug.Log("Enemy");
            }

        }

    }
}