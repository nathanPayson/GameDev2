using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{

    public static UnityEvent eNewLevel;
    public static UnityEvent eTasksComplete;
    public static UnityEvent eTasksIncomplete;
    public static UnityEvent eAnimationLaunched;
    public static UnityEvent eAnimationComplete;
    public static UnityEvent ePause;

    //Game Manager States
    public gmMainMenuState mainMenuState;
    public gmLaunchLevelState launchLevelState;
    public gmInLevelState inLevelState;
    public gmAnimationState animationState;
    public gmNextLevelState newLevelState;



    public gmState currentState;

    public InputAction IAPause;


    private void Awake()
    {
        currentState = mainMenuState;
        DontDestroyOnLoad(gameObject);
    }
    private void OnEnable()
    {
        IAPause.Enable();
        IAPause.performed += ctx => pauseClicked();
    }

    private void OnDisable()
    {
        PlayButton.ePlayButtonPressed?.RemoveListener(OnPlayButtonPressed);
    }

    void OnPlayButtonPressed()
    {
        eNewLevel?.Invoke();
    }

    private void Update()
    {
        currentState = currentState.DoState(this.GetComponent<GameManager>());
    }
    //State System Rules
    //MainMenu --> Launch New Level --> InLevel --> LaunchAnimation --> New Level Panel --> Launch New Level

    public void pauseClicked()
    {
        if(currentState.stateName == inLevelState.stateName)
        {
            //pauseGame();
            ePause?.Invoke();
        }

    }
}
