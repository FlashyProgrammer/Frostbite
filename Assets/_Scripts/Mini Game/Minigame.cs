using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Minigame : MonoBehaviour
{
    [Header("Mini-Games")]
    [SerializeField] private GameObject miniGameOne;
    [SerializeField] private GameObject miniGameTwo;
    [SerializeField] private GameObject miniGameThree;


    [Header("General Parameters")]
    [SerializeField] private GameObject marker;
    [SerializeField] private GameObject hitArea;
    [SerializeField] private Slider meterBar;

    [Header("Mini-game One parameters")]
    [SerializeField] private float markerRotationSpeed;
    [SerializeField] private float hitRotationSpeed;

    [Header("Mini-game Two Parameters")]
    [SerializeField] private float minObstacleRotationSpeed;
    [SerializeField] private float maxObstacleRotationSpeed;
    [SerializeField] private List<RectTransform> obstacles;

    [Header("Mini-Game Three Parameters")]
    [SerializeField] private float barAmount;
    [SerializeField] private float increaseAmount;
    [SerializeField] private float decreaseAmount;

    [Header("Player Settings")]
    [SerializeField] private PlayerInteraction interaction;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private CameraMove mouseLook;


    public Action reward;
    private float randomSpeed;
    private bool hitStopped = false;
    private GameObject currentMinigame;
    private int buttonCounter;

    private List<float> randomSpeeds;
    private bool loopCompleted;

    private void Awake()
    {
        randomSpeeds = new List<float>();
        meterBar.maxValue = barAmount;
    }
    private void Update()
    {
        if (miniGameOne.activeInHierarchy)
        {
            currentMinigame = miniGameOne;
            Disable();

            if (!hitStopped)
            {
                marker.SetActive(false);
                hitArea.GetComponent<RectTransform>().Rotate(Vector3.forward * hitRotationSpeed * Time.deltaTime);
            }

            if (hitStopped)
            {
                marker.GetComponent<RectTransform>().Rotate(Vector3.forward * markerRotationSpeed * Time.deltaTime);
            }

        }

        if (miniGameTwo.activeInHierarchy)
        {
            currentMinigame = miniGameTwo;

            if (!loopCompleted)
            {
                for (int index = 0; index < obstacles.Count; index++)
                {

                    float randomChance = UnityEngine.Random.Range(0f, 1f);
                    if (randomChance < 0.5f)
                    {
                        randomSpeed = -maxObstacleRotationSpeed;
                    }

                    else
                    {
                        randomSpeed = maxObstacleRotationSpeed;
                    }

                    randomSpeeds.Add(randomSpeed);
                    loopCompleted = true;


                }
            }

            if (randomSpeeds != null)
            {
                for (int index = 0; index < obstacles.Count; index++)
                {
                    obstacles[index].GetComponent<RectTransform>().Rotate(Vector3.forward * randomSpeeds[index] * Time.deltaTime);
                }
            }
           
            Disable();
        }

        if (miniGameThree.activeInHierarchy)
        {
            currentMinigame = miniGameThree;
            meterBar.value -= decreaseAmount;
            Disable();
        }
    }

    public void Confirm (InputAction.CallbackContext context)
    {
        if(currentMinigame != null)
        {
            if (context.performed && buttonCounter == 0 && currentMinigame == miniGameOne)
            {
                if (currentMinigame.activeInHierarchy)
                {
                    if (!hitStopped)
                    {
                        hitStopped = true;
                        marker.SetActive(true);
                    }

                }

                buttonCounter++;
            }

            if (context.canceled && buttonCounter == 1 && currentMinigame == miniGameOne)
            {
                buttonCounter++;
            }

            if (context.performed && buttonCounter == 2 && currentMinigame == miniGameOne)
            {
                if (currentMinigame.activeInHierarchy)
                {
                    marker.GetComponent<Marker>().canCheck = true;
                    marker.GetComponent<Marker>().Check();
                }
            }

            if (context.performed && currentMinigame == miniGameThree)
            {
                meterBar.value += increaseAmount;
                if (meterBar.value >= barAmount) Completed();
            }
        }

       
        
    }
    private void Disable()
    {
        interaction.enabled = false;
        playerMovement.enabled = false;
        mouseLook.enabled = false;
    }
    public void Reset()
    {
        hitArea.GetComponent<RectTransform>().localRotation = Quaternion.identity;
        marker.GetComponent<RectTransform>().localRotation = Quaternion.identity;
        randomSpeeds.Clear();
        loopCompleted = false;
        hitStopped = false;
        buttonCounter = 0;
        interaction.enabled = true;
        playerMovement.enabled = true;
        mouseLook.enabled = true;
        marker.gameObject.GetComponent<Marker>().canCheck = false;
        currentMinigame.SetActive(false);
    }
    public void Completed()
    {
        Reset();
        if(reward != null) reward.Invoke();
    }

    public GameObject GetMiniGameOne()
    {
        return miniGameOne;
    }
    public GameObject GetMiniGameTwo()
    {
        return miniGameTwo;
    }
    public GameObject GetMiniGameThree()
    {
        return miniGameThree;
    }


}
