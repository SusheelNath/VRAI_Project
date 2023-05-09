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
    }

    // Restart Gameplay
    public void RestartScene()
    {
        StartCoroutine(RestartSequence());
    }

    IEnumerator RestartSequence()
    {
        uiManager.gameOverUI.SetActive(true);

        scoreManager.enabled = false;

        yield return new WaitForSeconds(2f);

        SceneManager.LoadSceneAsync("VRAI_Demo");
    }
}
