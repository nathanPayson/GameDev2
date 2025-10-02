using UnityEngine;

public class gmLaunchLevelState : MonoBehaviour, gmState
{
    public string stateName => "LaunchLevelState";
    public bool stateStarted = false;
    public string nextState = "";

    private void OnEnable()
    {
        LaunchGamePanel.eGameStart?.AddListener(OnStartLevel);
    }
    private void OnDisable()
    {
        LaunchGamePanel.eGameStart?.RemoveListener(OnStartLevel);
    }
    void OnStartLevel()
    {
        nextState = "InLevelState";
    }
    #region gmState implementation
    public gmState DoState(GameManager gm)
    {
        gmState state = this;
        if (!stateStarted)
        {
            state.onEntrance();
            stateStarted = true;
        }
        DebugPanel.eDebugAlert?.Invoke(0, "InLaunchLevelState");

        return gm.currentState.endConditions(gm);
    }
    public void onEntrance()
    {
        Debug.Log("Entering LaunchLevel State");
    }
    public gmState endConditions(GameManager gm)
    {
        if(nextState == "InLevelState")
        {
            stateStarted = false; //Reseting for next time
            nextState = "";
            Debug.Log("Exiting LaunchLevel State");
            return gm.inLevelState;
        }
        return gm.launchLevelState;
    }
    #endregion
}
