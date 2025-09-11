using System;
using UnityEngine;

public class point : MonoBehaviour
{
    public static Action pointScored;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Point Triggered by: " + other.name);
        if (other.CompareTag("Player"))
        {
            pointScored?.Invoke();
            gameObject.SetActive(false);
        }
    }
}
