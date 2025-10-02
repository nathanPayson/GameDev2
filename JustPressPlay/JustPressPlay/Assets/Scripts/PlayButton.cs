using UnityEngine;
using UnityEngine.Events;

public class PlayButton : MonoBehaviour
{
    public GameObject button;
    public static UnityEvent ePlayButtonPressed = new UnityEvent();
    bool allowed = true;
    public void clicked()
    {
        if (allowed)
        {
            Debug.Log("Play Button Clicked");
            ePlayButtonPressed?.Invoke();
            button.gameObject.SetActive(false);
        }
    }

    void OnEnable()
    {
        Debug.Log("Play Button Enabled");
        GameManager.eTasksComplete?.AddListener(OnTasksComplete);
        GameManager.eTasksIncomplete?.AddListener(OnTasksIncomplete);
        GameManager.eNewLevel?.AddListener(OnNewLevel);
    }

    private void OnDisable()
    {
        Debug.Log("Play Button Disabled");
        GameManager.eTasksComplete?.RemoveListener(OnTasksComplete);
        GameManager.eTasksIncomplete?.RemoveListener(OnTasksIncomplete);
        GameManager.eNewLevel?.RemoveListener(OnNewLevel);
    }

    void OnTasksComplete()
    {
        allowed = true;
    }

    void OnTasksIncomplete()
    {
        allowed = false;
    }

    void OnNewLevel()
    {
        allowed = false;
    }
}
