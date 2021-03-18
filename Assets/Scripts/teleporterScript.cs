using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class teleporterScript : MonoBehaviour
{
    private teleporterCooldown parentCooldownManager;

    public ParticleSystem PS_1;
    public ParticleSystemRenderer PS_1_rend;
    public ParticleSystem PS_2;
    public ParticleSystemRenderer PS_2_rend;

    [SerializeField]
    [Range(0, 1)]
    public int childNumber;

    [SerializeField]
    public Transform child0Pos;

    [SerializeField]
    public Transform child1Pos;

    private AudioSource audio;

    public AudioClip teleportAudio;

    private void Start()
    {

        parentCooldownManager = transform.parent.GetComponent<teleporterCooldown>();

        PS_1 = this.gameObject.transform.parent.GetChild(0).GetComponentInChildren<ParticleSystem>();
        PS_1_rend = this.gameObject.transform.parent.GetChild(0).GetComponentInChildren<ParticleSystemRenderer>();
        PS_1_rend.material.SetColor("_EmissionColor", this.GetComponent<MeshRenderer>().material.GetColor("_EmissionColor"));

        PS_1.Stop();

        PS_2 = this.gameObject.transform.parent.GetChild(1).GetComponentInChildren<ParticleSystem>();
        PS_2_rend = this.gameObject.transform.parent.GetChild(1).GetComponentInChildren<ParticleSystemRenderer>();
        PS_2_rend.material.SetColor("_EmissionColor", this.GetComponent<MeshRenderer>().material.GetColor("_EmissionColor"));

        PS_2.Stop();
    }

    // Update is called once per frame
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Mech"))
        {

            audio = other.gameObject.GetComponent<AudioSource>();

            parentCooldownManager.teleportGotUsed = true;
            if (parentCooldownManager.teleportCooldown == 5)
            {

                if (childNumber != 1)
                {
                    other.gameObject.transform.SetPositionAndRotation(new Vector3(child1Pos.position.x, child1Pos.position.y + 2f, child1Pos.position.z), other.gameObject.transform.rotation);

                    Debug.Log("Teleport to child1");

                }
                else if (childNumber == 1)
                {
                    other.gameObject.transform.SetPositionAndRotation(new Vector3(child0Pos.position.x, child0Pos.position.y + 2f, child0Pos.position.z), other.gameObject.transform.rotation);

                    Debug.Log("Teleport to child0");

                }

                PS_1.Play();
                PS_2.Play();

                if (audio != null)
                {
                    audio.PlayOneShot(teleportAudio, PlayerPrefs.GetFloat("SFX", 0));
                }
            }
        }           
        
    }
}

