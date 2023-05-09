using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Script responsible for adding score every second until game finishes
/// </summary>
public class ScoreManager : MonoBehaviour
{
    // UI reference
    public Text scoreText;

    // Value to which time is added since start of session
    public float scoreValue;

    void Start()
    {
        scoreValue = 0f;
    }

    void Update()
    {
        scoreValue += Time.deltaTime;

        var intConvertedScore = (int)scoreValue;
        scoreText.text = intConvertedScore.ToString();
    }
}