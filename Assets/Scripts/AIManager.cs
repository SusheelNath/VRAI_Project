using UnityEngine;

public class AIManager : MonoBehaviour
{
    GameManager _gameManager;

    public bool hasGameplayEnded = false;

    void Start()
    {
        _gameManager = FindObjectOfType<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (hasGameplayEnded)
        {
            _gameManager.RestartScene();
        }
    }
}
