using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowHealthBar : MonoBehaviour
{
    public Slider slider;
    public List<GameObject> MechsAlive = new List<GameObject>();

    void Start()
    {
        StartCoroutine("LateStart");
    }

    IEnumerator LateStart()
    {
        yield return new WaitForSeconds(0.1f);
        MechsAlive.AddRange(GameObject.FindGameObjectsWithTag("Mech"));
        this.GetComponent<RectTransform>().localPosition -= new Vector3(0f, 35f * MechsAlive.IndexOf(transform.root.gameObject), 0f);
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;
    }
    public void SetHealth(int health)
    {
        slider.value = health;
    }
}
