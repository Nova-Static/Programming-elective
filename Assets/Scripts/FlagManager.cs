using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlagManager : MonoBehaviour
{
    public string name;
    public bool robot = false;

    private bool capturingFlag;
    List<GameObject> robots = new List<GameObject>();

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Mech")
        {
            
            robots.Add(other.gameObject);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.tag.Equals("Mech"))
        {
            robots.Remove(other.gameObject);
        }
    }

    private void FixedUpdate()
    {
        if (robots== null)
        {
            return;
        }
        if (robots.Count > 0)
        {

            robot = true;
        }
        if (robots.Count == 1)
        {
            
            name = robots[0].name;
        }
        else
        {
            name = null;
        }
    }
}
