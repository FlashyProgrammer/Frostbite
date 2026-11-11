using System.Collections;
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
    [SerializeField] private float stunTime;

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
    private Animator animator;
    private Color defaultColor;
    private Coroutine stunRoutine;
    private int currentPoint;
    private float timerCounter;
    private bool enemyOverlap;
    private bool overlapThisStep;
    private bool isStunned;

    private void Awake()
    {
        enemyImage = GetComponent<Image>();
        rectTransform = GetComponent<RectTransform>();
        animator = GetComponent<Animator>();
        defaultColor = enemyImage.color;
        currentPoint = 0;
    }

    private void OnEnable()
    {
        currentPoint = 0;
        timerCounter = timerCheck;
        enemyOverlap = false;
        overlapThisStep = false;
        ResetStun();
    }

    private void OnDisable()
    {
        currentPoint = 0;
        inRadarOne = false;
        inRadarTwo = false;
        inRadarThree = false;
        inRadarFour = false;
        ResetStun();
    }

    private void Update()
    {
        if (isStunned) return;
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
        
        if (!gameObject.activeInHierarchy) return;

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
        if (trap.CompareTag("Talisman"))
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
            return;
        }

        if (trap.CompareTag("Flare"))
        {
            trap.gameObject.SetActive(false);
            Stun();
        }
    }

   
    private void Stun()
    {
        if (stunRoutine != null) StopCoroutine(stunRoutine);
        stunRoutine = StartCoroutine(StunEnemy());
    }

    private IEnumerator StunEnemy()
    {
        isStunned = true; 
        PlayAnimation("Freeze");
        enemyImage.color = Color.blue;

        currentPoint = Mathf.Max(0, currentPoint - 1);

        if (movePath != null && movePath.Count > 0)
        {
            MoveToCurrentPoint();
        }

        timerCounter = timerCheck;
        enemyOverlap = false;
        overlapThisStep = false;

        yield return new WaitForSeconds(stunTime);

        ResetStun();
        PlayAnimation("Pulse");
    }

    
    private void ResetStun()
    {
        isStunned = false;
        stunRoutine = null;
     
        if (enemyImage != null)
        {
            enemyImage.color = defaultColor;
        }
    }

    private void PlayAnimation(string state)
    {
        if (animator != null)
        {
            animator.Play(state);
        }
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

    public void ConfigureRadars(bool radarOne, bool radarTwo, bool radarThree, bool radarFour)
    {
        inRadarOne = radarOne;
        inRadarTwo = radarTwo;
        inRadarThree = radarThree;
        inRadarFour = radarFour;

        currentPoint = 0;
        timerCounter = timerCheck;
        enemyOverlap = false;
        overlapThisStep = false;
        ResetStun();
    }
}