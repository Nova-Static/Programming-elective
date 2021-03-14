using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundDissabler : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this);
        gameObject.SetActive(false);
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
