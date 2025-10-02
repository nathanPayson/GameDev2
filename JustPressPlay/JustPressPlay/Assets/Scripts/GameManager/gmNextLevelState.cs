using UnityEngine;

public class gmNextLevelState : MonoBehaviour, gmState
{
    public string stateName => "NextLevelState";
    public bool stateStarted = false;
    public string nextState = "";

    private void OnEnable()
    {
        NewLevelPanel.eNextLevelSelected?.AddListener(OnNextLevelSelected);
    }
    private void OnDisable()
    {
        NewLevelPanel.eNextLevelSelected?.RemoveListener(OnNextLevelSelected);
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
        DebugPanel.eDebugAlert?.Invoke(0, "NextLevelState");
        return gm.currentState.endConditions(gm);
    }
    public void onEntrance()
    {
        Debug.Log("Entering NextLevel State");
        gmAnimationState.playAnimation?.Invoke();
    }
    public gmState endConditions(GameManager gm)
    {
        if (nextState == "LaunchLevelState")
        {
            stateStarted = false; //Reseting for next time
            nextState = "";
            Debug.Log("Exiting NextLevel State");
            return gm.launchLevelState;
        }
        return gm.newLevelState;
    }
    #endregion
    void OnNextLevelSelected(int i)
    {
        nextState = "LaunchLevelState";
    }
}
