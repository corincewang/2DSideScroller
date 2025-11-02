using UnityEngine;
using TMPro;

public class ScoreUI : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    
    void Update()
    {
        if (GameManager.Gary != null)
        {
            scoreText.text = "Score: " + GameManager.Gary.score;
        }
    }
}

