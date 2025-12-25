using UnityEngine;
public class DoorTrigger : MonoBehaviour
{
    public Door linkedDoor; 
    private QuickSlotInventory playerInventory;
    public void Start()
    {
        playerInventory = FindObjectOfType<QuickSlotInventory>();
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            if (playerInventory != null)
            {
                playerInventory.SetNearbyDoors(linkedDoor); 
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) 
        {
            if (playerInventory != null)
            {
                playerInventory.SetNearbyDoors(null); 
            }
        }
    }
}