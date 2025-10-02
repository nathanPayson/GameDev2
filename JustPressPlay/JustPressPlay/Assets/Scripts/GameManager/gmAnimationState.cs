using UnityEngine;
using UnityEngine.Events;

public class gmAnimationState : MonoBehaviour, gmState
{
    public string stateName => "AnimationState";
    public bool stateStarted = false;
    public string nextState = "";
    public static UnityEvent playAnimation = new UnityEvent();

    private void OnEnable()
    {
        animationTimelineHolder.animationComplete?.AddListener(OnAnimationComplete);
    }
    private void OnDisable()
    {
        animationTimelineHolder.animationComplete?.RemoveListener(OnAnimationComplete);
    }
    void OnAnimationComplete()
    {
        nextState = "LaunchLevelState";
    }
    #region gmState implementation
    public gmState DoState(GameManager gm)
    {
        //Debug.Log("Animation State");
        gmState state = this;
        if (!stateStarted)
        {
            state.onEntrance();
            stateStarted = true;
        }
        DebugPanel.eDebugAlert?.Invoke(0, "AnimationState");
        return gm.currentState.endConditions(gm);
    }
    public void onEntrance()
    {
        playAnimation?.Invoke();
        Debug.Log("Entering Animation State");
    }
    public gmState endConditions(GameManager gm)
    {
        if (nextState == "LaunchLevelState")
        {
            stateStarted = false; //Reseting for next time
            nextState = "";
            Debug.Log("Exiting Animation State");
            return gm.newLevelState;
        }
        return gm.animationState;
    }
    #endregion

}
