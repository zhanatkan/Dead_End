using UnityEngine;
public class DoorTrigger : MonoBehaviour
{
    public Door linkedDoor; // Список дверей, которые связаны с этим триггером
    private QuickSlotInventory playerInventory; // Ссылка на инвентарь, который находится в Canvas
    public void Start()
    {
        playerInventory = FindObjectOfType<QuickSlotInventory>();
    }
    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) // Если игрок входит в триггер
        {
            if (playerInventory != null)
            {
                playerInventory.SetNearbyDoors(linkedDoor); // Сообщаем инвентарю о дверях
            }
        }
    }

    public void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Если игрок выходит из триггера
        {
            if (playerInventory != null)
            {
                playerInventory.SetNearbyDoors(null); // Убираем информацию о дверях, когда игрок уходит
            }
        }
    }
}