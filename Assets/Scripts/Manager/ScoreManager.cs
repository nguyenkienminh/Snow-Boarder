using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI speedText;
    [SerializeField] TextMeshProUGUI coinText;
    [SerializeField] TextMeshProUGUI gemText;

    private int score = 0;
    private float speed = 0f;
    private float speedMultiplier = 1f;
    private int comboCount = 0; 
    private float comboMultiplier = 1f; 
    private int coin = 0;
    private int gem = 0;

    private bool isPaused = false;

    void Start()
    {
        UpdateScore();
        UpdateSpeed();
        UpdateCoin();
        UpdateGem();
    }

    public void AddScore(int basePoints)
    {
        if (isPaused) return;
        int finalPoints = (int)(basePoints + (speedMultiplier * comboMultiplier));
        score += finalPoints;
        UpdateScore();
    }

    public void DecreaseScore(int amount)
    {
        if (isPaused) return;

        if (score > 0)
        {
            score -= amount;
        }
        UpdateScore();
    }

    public void AddCombo()
    {
        if (isPaused) return;

        comboCount++;
        comboMultiplier = 1f + (comboCount * 0.2f);
        UpdateScore();
    }

    public void ResetCombo()
    {
        if (isPaused) return;

        comboCount = 0;
        comboMultiplier = 1f;
    }

    public void SetSpeedMultiplier(float speed)
    {
        if (isPaused) return;

        speedMultiplier = Mathf.Clamp(speed / 10f, 1f, 3f); 
    }

    public int GetScore() => score;

    private void UpdateScore()
    {
        if (scoreText != null)
        {
            scoreText.text = GetScore().ToString();
        }
    }

    public void AddSpeed(float currentSpeed)
    {
        if (isPaused) return;
        speed = Mathf.Round(currentSpeed * 10) / 10;
        UpdateSpeed();
    }
    
    public void UpdateSpeed()
    {
       
        if (speedText != null)
        {
            speedText.text = $"{speed} m/s";
        }
    }

    public void AddCoin(int amount)
    {
        coin += amount;
        UpdateCoin();
    }

    public void UpdateCoin()
    {
        if(coinText != null)
        coinText.text = coin.ToString();
    }

    public void AddGem(int amount)
    {
        gem += amount;
        UpdateGem();
    }

    public void UpdateGem()
    {
        if (gemText != null)
            gemText.text = gem.ToString();
    }

    public void StopScoring()
    {
        isPaused = true;
    }

	public int GetCoin() => coin;
	public int GetGem() => gem;
}
