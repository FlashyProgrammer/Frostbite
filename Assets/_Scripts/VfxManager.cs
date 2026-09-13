using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

public class VFXManager : MonoBehaviour
{
    [SerializeField] private Transform playerTransform;
    [Header("Breathe VFX")]
    [SerializeField] private VisualEffect breatheVFX;
    [SerializeField] private float minTime;
    [SerializeField] private float maxTime;

    private float timeCounter;


    private void Awake()
    {
        timeCounter = Random.Range(minTime, maxTime);
    }
    private void Update()
    {
        if (timeCounter > 0) timeCounter -= Time.deltaTime;
        
        else
        {
            breatheVFX.Play();
            timeCounter = Random.Range(minTime, maxTime);
        }
    }

}
