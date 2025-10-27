using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZonaMuerte : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Aster"))
        {
            Debug.Log("Caiste al vacío");
            collision.GetComponent<PlayerController>().RecibeDanio(Vector2.zero, 99);
        }
    }
}
