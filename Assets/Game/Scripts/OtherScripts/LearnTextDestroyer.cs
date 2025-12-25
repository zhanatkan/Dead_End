using UnityEngine;

public class LearnTextDestroyer : MonoBehaviour
{
    public GameObject ltext;
    private bool i;
    private void Start()
    {
        i = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.CompareTag("Player"))
        {
            ltext.SetActive(true);
            if(i == false) ltext.SetActive(false);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            i = false;
            ltext.SetActive(false);
        }
    }
}
