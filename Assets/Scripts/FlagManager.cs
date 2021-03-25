using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FlagManager : MonoBehaviour
{
    public string name;
    public bool robot = false;
    public float timeToWin = 5f;
    public Image image;
    public Canvas canvas;
    private float initialTimeToWin;
    private bool capturingFlag;
    List<GameObject> robots = new List<GameObject>();
    private GameObject[] fireworks;
    private bool playerWon = false;

    [SerializeField]
    public Canvas WinningScreen;

    [SerializeField]
    public TMPro.TextMeshProUGUI WinningText;

    private void Start()
    {
        fireworks = GameObject.FindGameObjectsWithTag("Firework");
        foreach (var pFireworks in fireworks)
        {
            pFireworks.GetComponent<ParticleSystem>().Stop();
        }
        initialTimeToWin = timeToWin;
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag == "Mech")       {
            
    //        robots.Add(other.gameObject);
    //    }
    //}
   

    private void OnTriggerStay(Collider other)
    {
        
        if (other.tag.Equals("Mech"))
        {
            if (!robots.Contains(other.gameObject))
            {
                robots.Add(other.gameObject);
            }
            
        }
    }

    private void FixedUpdate()
    {
        
        canvas.transform.LookAt(GameObject.Find("CM_Camera1").transform);
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
            image.fillAmount = timeToWin / initialTimeToWin;
            timeToWin -= Time.deltaTime;
            if (robots[0] != null)
            {
                name = robots[0].name;
            }
        }
        else
        {
            image.fillAmount = timeToWin / initialTimeToWin;
            timeToWin = initialTimeToWin;
            name = null;
        }
        if (timeToWin <= 0)
        {
            if (!playerWon)
            {
                foreach (var pFireworks in fireworks)
                {
                    pFireworks.GetComponent<ParticleSystem>().Play();
                }

                WinningScreen.gameObject.SetActive(true);
                WinningText.text = ("Winner is: " + robots[0].name);
                


            }
            playerWon = true;
        }
        robots.Clear();
    }
}
