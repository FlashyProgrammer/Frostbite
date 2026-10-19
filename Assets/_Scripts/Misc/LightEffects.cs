using UnityEngine;
using System.Collections;

public class LightEffects : MonoBehaviour
{

    [Header("Flicker Settings")]
    [SerializeField] private float minIntensity = 0.2f;
    [SerializeField] private float maxIntensity = 1.5f;

    [Header("Flicker Speed Settings")]
    [SerializeField] private float minDelay = 0.05f;
    [SerializeField] private float maxDelay = 0.2f;

    [Header("Light Property")]
    [SerializeField] private Color emergencyColor;
    private Light facilityLight;

    private Color defaultLight;
    private void Awake()
    {
        facilityLight = GetComponent<Light>();
        defaultLight = facilityLight.color;
    }

    public Color GetColor() 
    { 
        return defaultLight; 
    }

    public IEnumerator Flicker()
    {
        while (true && facilityLight != null)
        {
            facilityLight.color = emergencyColor;
            facilityLight.intensity = Random.Range(minIntensity, maxIntensity);
            float randomDelay = Random.Range(minDelay, maxDelay);
            yield return new WaitForSeconds(randomDelay);
        }
    }

}
