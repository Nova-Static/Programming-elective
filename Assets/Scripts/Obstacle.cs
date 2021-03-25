using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    internal Vector3 position;
    [SerializeField]
    float force = 0;

    public float Force
    {
        get { return force; }
        set { force = value; }
    }
}
