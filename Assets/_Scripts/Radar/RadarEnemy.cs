using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RadarEnemy : MonoBehaviour
{
    [Header("Enemy Type")]
    public bool isWolf;
    public bool isHuman;

    [Header("Enemy Movement Settings")]
    [SerializeField] private float timerCheck;
    [Range(1, 100)]
    [SerializeField] private float moveBackChance;

    [Header("Movement Ring Points")]
    public List<RectTransform> movePath;

    [Header("Radars")]
    public bool inRadarOne;
    public bool inRadarTwo;
    public bool inRadarThree;
    public bool inRadarFour;

    private EnemySpawner spawner;
    private RectTransform rectTransform;
    private Image enemyImage;
    private int currentPoint;
    private float timerCounter;
    private bool enemyOverlap;
    private bool overlapThisStep;

    private void Awake()
    {
        enemyImage = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        currentPoint = 0;
    }

    private void OnEnable()
    {
        currentPoint = 0;
        timerCounter = timerCheck;
        enemyOverlap = false;
        overlapThisStep = false;
    }

    private void Update()
    {
        if (movePath == null || movePath.Count == 0) return;

        timerCounter -= Time.deltaTime;
        if (timerCounter > 0) return;
        timerCounter = timerCheck;

      
        bool moveBack = Random.value <= moveBackChance / 100f;
        currentPoint = Mathf.Clamp(currentPoint + (moveBack ? -1 : 1), 0, movePath.Count - 1);
        MoveToCurrentPoint();
    }

    private void FixedUpdate()
    {
        enemyOverlap = overlapThisStep;
        overlapThisStep = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HandleTrigger(collision);
    }


    private void OnTriggerStay2D(Collider2D collision)
    {
        HandleTrigger(collision);
    }

    private void HandleTrigger(Collider2D collision)
    {
        if (collision.CompareTag("Enemy"))
        {
            overlapThisStep = true;
            return;
        }

        if (!collision.TryGetComponent(out TalismanTrap trap)) return;
        if (!IsInRadar(trap.radarNumber)) return;

        TriggerTrap(collision);
    }

    private bool IsInRadar(int radarNumber)
    {
        switch (radarNumber)
        {
            case 1: return inRadarOne;
            case 2: return inRadarTwo;
            case 3: return inRadarThree;
            case 4: return inRadarFour;
            default: return false;
        }
    }

    private void TriggerTrap(Collider2D trap)
    {
        if (spawner != null)
        {
            spawner.SetSpawnNumber(spawner.GetSpawnNumber() - 1);
        }

        trap.gameObject.SetActive(false);

        currentPoint = 0;
        if (movePath != null && movePath.Count > 0)
        {
            MoveToCurrentPoint();
        }

        enemyImage.enabled = false;
        gameObject.SetActive(false);
    }

    private void MoveToCurrentPoint()
    {
        rectTransform.position = movePath[currentPoint].position;
        rectTransform.SetParent(movePath[currentPoint]);
    }

    public bool IsOverlapped()
    {
        return enemyOverlap;
    }

    public EnemySpawner SetSpawner(EnemySpawner enemySpawner)
    {
        spawner = enemySpawner;
        return spawner;
    }
}