using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowHealthBar : MonoBehaviour
{
    public Slider slider;
    public List<GameObject> MechsAlive = new List<GameObject>();
    public Text text;
    public Gradient gradient;
    public Image fill;
    
    void Start()
    {
        StartCoroutine("LateStart");
    }

    IEnumerator LateStart()
    {
        yield return new WaitForSeconds(0.1f);
        MechsAlive.AddRange(GameObject.FindGameObjectsWithTag("Mech"));
        this.GetComponent<RectTransform>().localPosition -= new Vector3(0f, 35f * MechsAlive.IndexOf(transform.root.gameObject), 0f);
        text.text = transform.root.name;
    }

    public void SetMaxHealth(int health)
    {
        slider.maxValue = health;
        slider.value = health;

        fill.color = gradient.Evaluate(1f);
    }
    public void SetHealth(int health)
    {
        slider.value = health;
        fill.color = gradient.Evaluate(slider.normalizedValue);
    }
}