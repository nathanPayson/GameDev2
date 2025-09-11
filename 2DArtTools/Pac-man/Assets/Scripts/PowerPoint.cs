using UnityEngine;
using System;

public class PowerPoint : point
{
    public static Action powerActivated;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("Point Triggered by: " + other.name);
        if (other.CompareTag("Player"))
        {
            powerActivated?.Invoke();
            pointScored?.Invoke();
            gameObject.SetActive(false);
        }
    }
}
