using UnityEngine;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    public static UIManager sharedInstance;
    public GameObject mainMenuUI;
    public GameObject pauseMenuUI;
    public GameObject playButton;
    public GameObject[] LevelUIs;
    public GameObject LevelCompletePanel;

    //public levelUI[] LevelUIs; //TO DO: Make a levelUI interface instead.

    private void Awake()
    {
        if(sharedInstance == null)
        {
            sharedInstance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnEnable()
    {
        SceneChanger.eChangeSceneRequested?.AddListener(OnSceneChanged);
        GameManager.ePause?.AddListener(activatePauseMenu);
        LaunchGamePanel.eGameStart?.AddListener(() => { playButton.SetActive(true); });
        
    }
    private void OnDisable()
    {
        SceneChanger.eChangeSceneRequested?.RemoveListener(OnSceneChanged);
    }
    void OnSceneChanged(string nextScene)//Takes Scene Change name and adjusts UI. Level UI components are added via level. MainMenu means all other UI's are off.
    {
        if(nextScene.Substring(0,5) == "Level")
        {
            hideMainMenuUI();

            if (int.TryParse(nextScene.Substring(6), out int number)){
                displayLevel(number);
            }
            //UIManagerState = InLevel;
        }
        else if(nextScene == "MainMenu")
        {
            displayMainMenuUI();
            //UIManagerState = MainMenu;
        }
    }
    void hideMainMenuUI()
    {
        mainMenuUI.SetActive(false);
    }

    void displayLevel(int number) //Level UIs stack with each level as they get more complex.
                                  //TO DO, Level UIs are a gameObject with an interface that has a "Activate()" method.
                                  //Which enables things to more easily change without linearity. (Example, changing a previous button).
                                  //Example: Play Button eventually becomes Text instead so the play button is removed. Or a previous rule now has a UI Change.

    {
        for(int i = 0; i <LevelUIs.Length; i++)
        {
            if(i <= number)
            {
                //LevelUIs[i].Activate();
                LevelUIs[i].SetActive(true);
            }
            else
            {
                LevelUIs[i].SetActive(false);
            }
        }
    }
    void displayMainMenuUI()//Activates Main Menu UI. Deactivates all Level UIs.
    {
        mainMenuUI.SetActive(true);
        resetLevelUI();
        playButton.SetActive(false);
    }
    void resetLevelUI()
    {
        for(int i = 0; i < LevelUIs.Length; i++)
        {
            LevelUIs[i].SetActive(false);
        }
    }

    void activatePauseMenu()
    {
        if(mainMenuUI.activeSelf == true)//Pause doesn't occur in Main Menu.    
        {
            return;
        }
        if(pauseMenuUI.activeSelf == true)//Doesn't change if pause menu is already active.
        {
            return;
        }
        pauseMenuUI.SetActive(!pauseMenuUI.activeSelf);
    }


}
