using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStartPosition : MonoBehaviour
{
    public Vector3 vec;
    private PlayerHealth player;
    private void Start()
    {
        player = FindObjectOfType<PlayerHealth>();
        player.transform.position = vec;
    }
}
