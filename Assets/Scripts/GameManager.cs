using UnityEngine;

/// <summary>
/// Script responsible for managing entire gameplay and sequencing
/// </summary>
public class GameManager : MonoBehaviour
{
    bool _isStartGameplay = false;

    [Header("Scoring Component References")]
    public ScoreManager scoreManager;
    public GameObject scoreBoardGO;

    [Header("Starting UI")]
    public GameObject startGameplayUI;

    [Header("FPS Controller Script References")]
    public PlayerMovementController fpcMovement;
    public PlayerViewController fpcView;

    void Update()
    {
        if(_isStartGameplay == false)
        {
            // * Permitted only once
            if(Input.GetKeyUp("space"))
            {
                _isStartGameplay = true;

                // Begin Gameplay Sequencing
                SetScoringComponentActive();
                StartGameplay();
                SetFPSControllersActive();
            }
        }
    }

    // Activate scoring components
    void SetScoringComponentActive()
    {
        scoreManager.gameObject.SetActive(true);
        scoreBoardGO.SetActive(true);
    }

    // Activate scoring components
    void StartGameplay()
    {
        startGameplayUI.SetActive(false);
    }

    // Activate FPS Controller scripts
    void SetFPSControllersActive()
    {
        fpcMovement.enabled = true;
        fpcView.enabled = true;
    }
}
