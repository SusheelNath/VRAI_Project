using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script responsible for adding score every second until game finishes
/// </summary>
public class ScoreManager : MonoBehaviour
{
    // UI reference
    public Text scoreTextMain;
    public Text scoreTextGameOver;

    // Value to which time is added since start of session
    public float scoreValue;

    void Start()
    {
        scoreValue = 0f;
    }

    void Update()
    {
        // Increment score over time
        scoreValue += Time.deltaTime;

        // Convert to int (whole numbers)
        var _intConvertedScore = (int)scoreValue;

        // Assing score to in-game score board and post game results
        scoreTextMain.text = _intConvertedScore.ToString();
        scoreTextGameOver.text = _intConvertedScore.ToString();
    }
}
