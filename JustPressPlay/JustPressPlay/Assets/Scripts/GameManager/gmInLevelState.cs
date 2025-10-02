using UnityEngine;

public class gmInLevelState : MonoBehaviour, gmState
{
    public string stateName => "InLevelState";
    public bool stateStarted = false;
    public string nextState = "";
    #region gmState implementation

    private void OnEnable()
    {
        Debug.Log("Enabled");
        PlayButton.ePlayButtonPressed.AddListener(OnPlayButtonPressed);

    }
    private void OnDisable()
    {
        PlayButton.ePlayButtonPressed.RemoveListener(OnPlayButtonPressed);
        Debug.Log("Disabled");
    }
    void OnPlayButtonPressed()
    {
        Debug.Log("Play Pressed Reached");
        nextState = "InAnimationState";
    }
    public gmState DoState(GameManager gm)
    {
        //Debug.Log("InLevel State");
        gmState state = this;
        if (!stateStarted)
        {
            state.onEntrance();
            stateStarted = true;
        }
        DebugPanel.eDebugAlert?.Invoke(0, "InLevelState");
        return gm.currentState.endConditions(gm);
    }
    public void onEntrance()
    {
        Debug.Log("Entering In Level State");
    }
    public gmState endConditions(GameManager gm)
    {
        Debug.Log("End Conditions Reached");
        if (nextState == "InAnimationState")
        {
            stateStarted = false; //Reseting for next time
            nextState = "";
            Debug.Log("Exiting InLevel State");
            return gm.animationState;
        }
        return gm.inLevelState;
    }
    #endregion

}
