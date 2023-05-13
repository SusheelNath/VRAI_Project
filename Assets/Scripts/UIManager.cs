using UnityEngine;

/// <summary>
/// Script containing all UI references
/// </summary>
public class UIManager : MonoBehaviour
{
    [Header("Scoring Component References")]
    public GameObject scoreBoardGO;
    public GameObject scoreManagerGO;

    [Header("UI Panels")]
    public GameObject welcomeUI;
    public GameObject gameOverUI;

    [Header("Minimap UI")]
    public GameObject minimapGO;

    // Subscribe
    void OnEnable()
    {
        Actions.OnGameplayStart += SetBeginGameplayUI;
        Actions.OnGameplayEnd += SetEndGameplayUI;
    }

    // UnSubscribe
    void OnDisable()
    {
        Actions.OnGameplayStart -= SetBeginGameplayUI;
        Actions.OnGameplayEnd -= SetEndGameplayUI;
    }

    // Present Start Gameplay Sequence UI
    public void SetBeginGameplayUI()
    {
        welcomeUI.SetActive(false);
        minimapGO.SetActive(true);
        scoreBoardGO.SetActive(true);
        scoreManagerGO.SetActive(true);
    }

    // Present End Gameplay Sequence UI
    public void SetEndGameplayUI()
    {
        gameOverUI.SetActive(true);
        minimapGO.SetActive(false);
        scoreBoardGO.SetActive(false);
        scoreManagerGO.SetActive(false);
    }
}
