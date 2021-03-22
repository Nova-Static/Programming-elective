using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class SoundSaver : MonoBehaviour
{
    Slider slider;
    AudioSource[] sources;
    public enum SelSound
    {

        SFX,
        Music

    }
    public SelSound selectedSound;
    private void Awake()
    { 

        sources = FindObjectsOfType<AudioSource>();
        
         slider = GetComponent<Slider>();
        switch (selectedSound)
        {

            case SelSound.SFX:
                slider.value = PlayerPrefs.GetFloat("SFX", 0);
                break;
            case SelSound.Music:
                slider.value = PlayerPrefs.GetFloat("Music", 0);
                break;
        }
    }
    private void Start()
    {
        soundChanged();
    }
    // Start is called before the first frame update
    public void soundChanged()
    {
        sources = FindObjectsOfType<AudioSource>();

        switch (selectedSound)
        {
        
            case SelSound.SFX:
                PlayerPrefs.SetFloat("SFX", slider.value);

                break;
            case SelSound.Music:
                PlayerPrefs.SetFloat("Music", slider.value);
                

                foreach (AudioSource audioSource in sources)
                {
                    if (audioSource.clip.name == "music")
                    {
                        audioSource.volume = slider.value/10;
                    }
                }
                break;
        }
    }
}
