using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    
    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist GameManager across scenes
        } else
        {
            Destroy(gameObject); // Only one instance is allowed
        }
    } 
}
