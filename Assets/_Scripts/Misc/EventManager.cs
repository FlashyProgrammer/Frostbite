using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    [Header("Lights")]
    [SerializeField] private List<Light> facilityLights;

    [Header("Audio")]
    [SerializeField] private List<AudioSource> enemySounds;
    [SerializeField] private AudioSource deathWolfSound;
    [SerializeField] private AudioSource deathTallSound;
    [SerializeField] private AudioSource alermSound;

   
    private readonly HashSet<GameObject> enemiesInside = new HashSet<GameObject>();
    private readonly List<GameObject> toRemove = new List<GameObject>();
    private readonly List<Coroutine> flickerRoutines = new List<Coroutine>();

    private void Update()
    {
        if (enemiesInside.Count == 0) return;

        toRemove.Clear();
        foreach (GameObject enemy in enemiesInside)
        {
            if (enemy == null || !enemy.activeInHierarchy)
            {
                toRemove.Add(enemy);
            }
        }

        foreach (GameObject enemy in toRemove)
        {
            RemoveEnemy(enemy);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy")) return;

       
        if (enemiesInside.Add(collision.gameObject) && enemiesInside.Count == 1)
        {
            StartEvent();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Enemy")) return;

        RemoveEnemy(collision.gameObject);
    }

    private void RemoveEnemy(GameObject enemy)
    {
        
        if (!enemiesInside.Remove(enemy)) return;

        if (enemy != null && enemy.TryGetComponent(out RadarEnemy radar))
        {
            if (radar.isWolf && deathWolfSound != null) deathTallSound.Play();
            if (radar.isHuman && deathTallSound != null) deathWolfSound.Play();
        }

        
        if (enemiesInside.Count == 0)
        {
            StopEvent();
        }
    }

    private void StartEvent()
    {
        bool lightsOff = false;
        foreach (Light light in facilityLights)
        {
            if (light != null && !light.gameObject.activeInHierarchy)
            {
                lightsOff = true;
            }
        }

        if (!lightsOff && alermSound != null)
        {
            alermSound.Play();
        }

        foreach (Light light in facilityLights)
        {
            if (light != null && light.TryGetComponent(out LightEffects effects))
            {
                flickerRoutines.Add(StartCoroutine(effects.Flicker()));
            }
        }

        foreach (AudioSource audio in enemySounds)
        {
            if (audio != null) audio.Play();
        }
    }

    private void StopEvent()
    {
      
        foreach (Coroutine routine in flickerRoutines)
        {
            if (routine != null) StopCoroutine(routine);
        }
        flickerRoutines.Clear();

        foreach (Light light in facilityLights)
        {
            if (light != null && light.TryGetComponent(out LightEffects effects))
            {
                light.color = effects.GetColor();
            }
        }

        foreach (AudioSource audio in enemySounds)
        {
            if (audio != null) audio.Stop();
        }

        if (alermSound != null) alermSound.Stop();
    }
}