using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DebugPanel : MonoBehaviour
{
    public static UnityEvent<int,string> eDebugAlert = new UnityEvent<int, string>();
    [SerializeField] TextMeshProUGUI gameStateText;

    private void Awake()
    {
    }
    private void OnEnable()
    {
        eDebugAlert.AddListener(OnDebugAlert);
    }

    private void OnDisable()
    {
        eDebugAlert.RemoveListener(OnDebugAlert);
    }

    private void OnDebugAlert(int alertLevel, string message)
    {
        if(alertLevel == 0)
        {
            gameStateText.text = "Game State: " + message;
        }
    }
}
