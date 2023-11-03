using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;//Add this namespace
using UnityEngine.UI; //makes ui variables available for canvas

public enum EGamestates//These are used for swapping out scenes
{
    LobbyScene, RulesScene, PlayScene, ScoreScene, QUIT
};

public class Startup : MonoBehaviour
{
    private static Startup instance;
    public int levelNum = 0;// set the level number here
    public EGamestates e_gamestate = EGamestates.LobbyScene;   // for setting the initial game state
    


    public static Startup GetInstance()
    {
        return instance;
    }

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Debug.Log("MORE THAN ONE INSTANCE OF GAME MANAGER AAAAAAAAAAAAAA");
        }
    }

    void Start()
    {
        DontDestroyOnLoad(transform.gameObject);
    }


    void Init() //sets up the game and runs the menu
    {

        
        //e_gamestate = EGamestates.MENU;
        //LoadCurrentScene(e_gamestate);
    }
    public void LoadCurrentScene_callable(int state) {
        LoadCurrentScene((EGamestates)state);
    
    }

    public void LoadCurrentScene(EGamestates State)
    {
        e_gamestate = State;
        switch (e_gamestate)
        {
            case EGamestates.LobbyScene:
                SceneManager.LoadScene("LobbyScene");
                break;

            case EGamestates.RulesScene:
                SceneManager.LoadScene("RulesScene");

                break;

            case EGamestates.PlayScene:
                SceneManager.LoadScene("PlayScene");
                break;

            case EGamestates.ScoreScene:
                SceneManager.LoadScene("ScoreScene");
                break;

            case EGamestates.QUIT:
                Application.Quit();
                break;

            default:
                break;
        }

    }

    public void LoadNextLevel()
    {
        int enumValue = (int)e_gamestate;
        enumValue++;
        LoadCurrentScene((EGamestates)enumValue);
        Debug.Log(enumValue);        
    }

    public EGamestates GetCurrentLevel()
    {
        return e_gamestate;
    }

}