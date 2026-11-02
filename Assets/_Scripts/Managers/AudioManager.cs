using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header("Audio Sources")]
    [SerializeField] private AudioSource playerSource;
    [Header("Audio Clips")]
    [SerializeField] private AudioClip walkClip;
    [SerializeField] private AudioClip runClip;
    [SerializeField] private AudioClip crouchClip;

    public void ActivateWalk()
    {
        playerSource.clip = walkClip;

    }
    public void ActivateRun()
    {
        playerSource.clip = runClip;

    }
    public void ActivateCrouch()
    {
        playerSource.clip = crouchClip;

    }

    public void SetClip(AudioClip clip)
    {
        playerSource.clip = clip;
    }

    public AudioSource GetPlayerSource()
    {
        return playerSource;
    }
}
