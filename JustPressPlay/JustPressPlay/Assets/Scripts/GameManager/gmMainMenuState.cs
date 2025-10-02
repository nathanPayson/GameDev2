using Unity.VisualScripting;
using UnityEngine;

public class gmMainMenuState : MonoBehaviour, gmState
{
    public string stateName => "MainMenuState";
    bool stateStarted = false;
    string nextState = "";
    void OnEnable()
    {
        SceneChanger.eChangeSceneRequested?.AddListener(OnSceneChanged);
    }

    void OnDisable()
    {
        SceneChanger.eChangeSceneRequested?.RemoveListener(OnSceneChanged);
    }
    #region gmState implementation
    gmState gmState.DoState(GameManager gm)
    {
        gmState state = this;
        if(!stateStarted)
        {
            state.onEntrance();
            stateStarted = true;
        }
        DebugPanel.eDebugAlert?.Invoke(0,"InMainMenuState");

        return gm.currentState.endConditions(gm);
    }
    void gmState.onEntrance()
    {
        Debug.Log("Entering MainMenuState");
    }
    gmState gmState.endConditions(GameManager gm)
    {
        if (nextState == "LaunchLevelState")
        {
            nextState = "";
            return gm.launchLevelState;
        }
        nextState = "";
        return gm.currentState;
    }
    #endregion

    void OnSceneChanged(string sceneName)
    {
        if(sceneName.Substring(0,5) == "Level")
        {
            Debug.Log("New State");
            nextState = "LaunchLevelState";
        }

    }
}
