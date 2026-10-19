using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class EventManager : MonoBehaviour
{
    [Header("Lights")]
    [SerializeField] private List<Light> facilityLights;

    [Header("Audio")]
    [SerializeField] private List<AudioSource> enemySounds;


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            foreach (Light light in facilityLights) 
            {
                StartCoroutine(light.GetComponent<LightEffects>().Flicker());
            
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            foreach (Light light in facilityLights)
            {
                StartCoroutine(light.GetComponent<LightEffects>().Flicker());

            }
            foreach (AudioSource audio in enemySounds)
            {
                audio.Play();
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            foreach (Light light in facilityLights)
            {
                if (light != null)
                {
                    light.GetComponent<Light>().color = light.GetComponent<LightEffects>().GetColor();
                    StopAllCoroutines();
                }
                foreach (AudioSource audio in enemySounds)
                {
                    audio.Stop();
                }


            }
        }
    }


}
