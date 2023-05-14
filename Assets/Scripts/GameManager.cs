using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Script responsible for managing entire gameplay and sequencing
/// </summary>
public class GameManager : MonoBehaviour
{
    bool _isStartGameplay = false;

    [Header("First Person Script References")]
    public PlayerMovementController fpcMovement;
    public PlayerViewController fpcView;
    public PlayerBrain fpBrain;

    [Header("Parent object of gameplay in scene")]
    public GameObject levelObjects;

    [Header("Game Over Audio")]
    public AudioSource gameOver;

    // Subscribe
    void OnEnable()
    {
        Actions.OnRestartScene += RestartScene;
    }

    // UnSubscribe
    void OnDisable()
    {
        Actions.OnRestartScene -= RestartScene;
    }

    void Update()
    {
        if(_isStartGameplay == false)
        {
            // * Permitted only once
            if(Input.GetKeyUp("space"))
            {
                _isStartGameplay = true;

                // Begin Gameplay Sequencing
                Actions.OnGameplayStart();

                SetFPComponentsActive();
                SetEnemyAgentSpawnersActive();
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

    // Find all agent spawners in scene and activate
    void SetEnemyAgentSpawnersActive()
    {
        var spawners = FindObjectsOfType<EnemyAgentPool>();

        foreach(EnemyAgentPool sp in spawners)
        {
            sp.enabled = true;
        }
    }

    // Sequencing for restart gameplay
    IEnumerator RestartSequence()
    {
        // End Gameplay Sequencing
        Actions.OnGameplayEnd();

        // Deactivate FPC components
        SetFPComponentsDeactive();
        levelObjects.SetActive(false);

        gameOver.Play();

        yield return new WaitForSeconds(4f);

        // Load/Reload scene
        SceneManager.LoadSceneAsync("VRAI_Demo");
    }

    // Deactivate First Person Component scripts
    public void SetFPComponentsDeactive()
    {
        fpcMovement.enabled = false;
        fpcView.enabled = false;
        fpBrain.enabled = false;
    }
}
