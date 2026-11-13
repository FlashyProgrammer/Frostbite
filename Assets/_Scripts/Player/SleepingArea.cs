using UnityEngine;
using UnityEngine.Playables;

public class SleepingArea : MonoBehaviour, IInteractable
{

     public bool canSleep = true;
     public bool isCompleted = false;

    [Header("Game Master")]
    [SerializeField] private GameMaster gameMaster;
    [Header("Cutscenes")]
    [SerializeField] private PlayableDirector sceneDirector;
    [SerializeField] private PlayableAsset nightTimeline;
    [SerializeField] private PlayableAsset DayTimeline;
    public string InteractPrompt => gameObject.name;
    public bool Interact()
    {
        if (canSleep && !isCompleted)
        {
            sceneDirector.playableAsset = nightTimeline;
            canSleep = false;
            isCompleted = false;
            sceneDirector.time = 0;
            sceneDirector.Play();
        }

        if (canSleep && isCompleted)
        {
            gameMaster.CompleteMenu();
        }

        return true;
    }

    public void DayTime()
    {
        sceneDirector.playableAsset = DayTimeline;
        canSleep = true;
        canSleep = false;
        isCompleted = false;
        sceneDirector.time = 0;
        sceneDirector.Play();
    }

}
