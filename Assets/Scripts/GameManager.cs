using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Script responsible for managing entire gameplay and sequencing
/// </summary>
public class GameManager : MonoBehaviour
{
    bool _isStartGameplay = false;

    [Header("Script References")]
    public UIManager uiManager;

    [Header("First Person Script References")]
    public PlayerMovementController fpcMovement;
    public PlayerViewController fpcView;
    public PlayerBrain fpBrain;

    void Update()
    {
        if(_isStartGameplay == false)
        {
            // * Permitted only once
            if(Input.GetKeyUp("space"))
            {
                _isStartGameplay = true;

                // Begin Gameplay Sequencing
                uiManager.SetBeginGameplayUI();
                SetFPComponentsActive();
            }
        }
    }

    // Restart Gameplay
    public void RestartScene()
    {
        StartCoroutine(RestartSequence());
    }

    // Activate First Person Component scripts
    public void SetFPComponentsActive()
    {
        fpcMovement.enabled = true;
        fpcView.enabled = true;
        fpBrain.enabled = true;
    }

    // Sequencing for restart gameplay
    IEnumerator RestartSequence()
    {
        // End Gameplay Sequencing
        uiManager.SetEndGameplayUI();

        yield return new WaitForSeconds(2f);

        // Load/Reload scene
        SceneManager.LoadSceneAsync("VRAI_Demo");
    }
}
