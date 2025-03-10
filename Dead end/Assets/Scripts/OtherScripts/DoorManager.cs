using UnityEngine;

public class DoorManager : MonoBehaviour
{
    public GameObject door;
    public bool isInside;
    public void Start()
    {
        isInside = false;
    }
    public void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Door"))
        {
            isInside = true;
        }
    }
}