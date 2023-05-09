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

    [Header("Minimap UI")]
    public GameObject minimapGO;

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

    // Disable starting UI and enable minimap
    void StartGameplay()
    {
        startGameplayUI.SetActive(false);
        minimapGO.SetActive(true);
    }

    // Activate FPS Controller scripts
    void SetFPSControllersActive()
    {
        fpcMovement.enabled = true;
        fpcView.enabled = true;
    }
}
