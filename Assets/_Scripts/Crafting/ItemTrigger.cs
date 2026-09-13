using UnityEngine;

public class ItemTrigger : MonoBehaviour
{
    [SerializeField] private Item item;
    public int quantity = 1;

    public Item ItemProperties()
    {
        return item;
    }

}
