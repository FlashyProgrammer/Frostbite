using UnityEngine;

public class PlayerRadar : MonoBehaviour
{
    [SerializeField] private GameMaster gameMaster;

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
           gameMaster.GameOver();
           other.gameObject.SetActive(false);
        }
    }   
}
