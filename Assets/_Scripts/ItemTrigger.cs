using UnityEngine;

public class ItemTrigger : MonoBehaviour
{
    [SerializeField] private Item item;

    public Item ItemProperties()
    {
        return item;
    }

}
