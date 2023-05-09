using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script containing all UI references
/// </summary>
public class UIManager : MonoBehaviour
{
    [Header("Scoring Component References")]
    public GameObject scoreBoardGO;

    [Header("UI Panels")]
    public GameObject startGameplayUI;
    public GameObject gameOverUI;

    [Header("FPS Controller Script References")]
    public PlayerMovementController fpcMovement;
    public PlayerViewController fpcView;

    [Header("Minimap UI")]
    public GameObject minimapGO;

    // Activate scoring components
    public void SetScoringComponentActive()
    {
        scoreBoardGO.SetActive(true);
    }

    // Disable starting UI and enable minimap
    public void StartGameplay()
    {
        startGameplayUI.SetActive(false);
        minimapGO.SetActive(true);
    }

    // Activate FPS Controller scripts
    public void SetFPSControllersActive()
    {
        fpcMovement.enabled = true;
        fpcView.enabled = true;
    }
}
