using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Script responsible for managing entire gameplay and sequencing
/// </summary>
public class GameManager : MonoBehaviour
{
    bool _isStartGameplay = false;
    bool _isGameOver = false;

    [Header("Script References")]
    public ScoreManager scoreManager;
    public UIManager uiManager;

    void Update()
    {
        if(_isStartGameplay == false)
        {
            // * Permitted only once
            if(Input.GetKeyUp("space"))
            {
                _isStartGameplay = true;

                // Begin Gameplay Sequencing
                scoreManager.gameObject.SetActive(true);

                uiManager.SetScoringComponentActive();
                uiManager.StartGameplay();
                uiManager.SetFPSControllersActive();
            }
        }

        if(_isGameOver)
        {
            //todo: Add in logic of NavMeshAgents talking to NavMeshManager (crete script)
            //and setting _isGameOver = true on collider touch with player

            //todo: Load Game Over UI from UI Manager
            RestartScene();
        }
    }

    void RestartScene()
    {
        //todo: add scene to build and rename scene to below
        SceneManager.LoadSceneAsync("VRAI_Demo");
    }
}
