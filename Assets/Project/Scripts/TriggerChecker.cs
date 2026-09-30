using System;
using UnityEngine;

public class TriggerChecker : MonoBehaviour
{
    public event Action<GameObject> TriggerEnter;
    public event Action<GameObject> TriggerStay;
    public event Action<GameObject> TriggerExit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TriggerEnter?.Invoke(collision.gameObject);
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        TriggerStay?.Invoke(collision.gameObject);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        TriggerExit?.Invoke(collision.gameObject);
    }
}
