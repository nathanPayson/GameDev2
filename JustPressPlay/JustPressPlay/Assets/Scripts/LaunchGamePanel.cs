using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class LaunchGamePanel : MonoBehaviour
{
    
    public TextMeshProUGUI LevelTitle;
    public TextMeshProUGUI LevelDescription;
    public GameObject panel;
    public static UnityEvent eGameStart = new UnityEvent();

    public void OnEnable()
    {
        SceneChanger.eChangeSceneRequested.AddListener(OnSceneChange);
    }
    public void OnDisable()
    {
        SceneChanger.eChangeSceneRequested.RemoveListener(OnSceneChange);
    }

    public void StartLevel()
    {
        eGameStart?.Invoke();
        panel.SetActive(false);
    }

    void OnSceneChange(string sceneName)
    {
        if(sceneName.Substring(0,5) == "Level")
        {
            panel.SetActive(true);
            LevelTitle.text = "Level " + sceneName.Substring(5);
            LevelDescription.text = "NYI: Description for Level " + sceneName.Substring(5);
        }
        else
        {
            panel.SetActive(false);
        }
    }

}
