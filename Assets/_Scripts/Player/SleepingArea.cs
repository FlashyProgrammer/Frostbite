using UnityEngine;
using UnityEngine.Playables;

public class SleepingArea : MonoBehaviour, IInteractable
{
    private bool canSleep = true;
    public bool isCompleted = false;
    [SerializeField] private PlayableDirector sceneDirector;
    [SerializeField] private PlayableAsset nightTimeline;
    [SerializeField] private PlayableAsset DayTimeline;
    public string InteractPrompt => gameObject.name;
    public bool Interact()
    {
        if (canSleep)
        {
            sceneDirector.playableAsset = nightTimeline;
            canSleep = false;
            isCompleted = false;
            sceneDirector.Play();
        }

        if (canSleep && isCompleted)
        {
            sceneDirector.playableAsset = DayTimeline;
            canSleep = true;
            canSleep = false;
            isCompleted = false;
            sceneDirector.Play();
        }


        return true;
    }

}
