using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class detecting : MonoBehaviour
{

    public MechController MechController;
    public bool detected;
    public GameObject detectedGameObject;

    // Start is called before the first frame update


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer == 11 || other.gameObject.name == "terrain_new")
        {
            detectedGameObject = other.gameObject;

        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.layer == 11 || other.gameObject.name == "terrain_new")
        {
            detected = true;


        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.layer == 11 || other.gameObject.name == "terrain_new")
        {

            detected = false;
            detectedGameObject = null;

        }
    }


}