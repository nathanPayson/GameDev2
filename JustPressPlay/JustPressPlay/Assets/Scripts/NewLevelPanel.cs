using UnityEngine;
using UnityEngine.Events;

public class NewLevelPanel : MonoBehaviour
{
    public GameObject panel;
    public static UnityEvent<int> eNextLevelSelected = new UnityEvent<int>();
 

    int level = 0;

    private void OnEnable()
    {
        SceneChanger.eChangeSceneRequested?.AddListener(onSceneChange);
        animationTimelineHolder.animationComplete.AddListener(() => { panel.SetActive(true); });

    }
    private void OnDisable()
    {
        SceneChanger.eChangeSceneRequested?.RemoveListener(onSceneChange);
        animationTimelineHolder.animationComplete.RemoveListener(() => { panel.SetActive(true); });
    }
    public void OnNextLevelButton()
    {
        eNextLevelSelected?.Invoke(level+1);
        panel.SetActive(false);
    }

    public void onSceneChange(string sceneName)
    {
        if (sceneName.Substring(0, 5) == "Level")
        {
            level = int.Parse(sceneName.Substring(5));
        }
    }



}
