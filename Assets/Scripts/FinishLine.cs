using NUnit.Framework.Internal;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FinishLine : MonoBehaviour
{
    //[SerializeField] float loadDelay = .5f;
    [SerializeField] ParticleSystem finishEffect;

    [SerializeField] GameObject resultPanel;  // UI kết quả
    [SerializeField] TextMeshProUGUI finalScoreText;
    [SerializeField] GameObject[] stars;
    [SerializeField] GameObject nextButton;

    [Header("Map Score")]
    [SerializeField] int baseMapScore;

    private int level;
    private int bestScore;
    private int bestStars;
    private string levelName;
    private int coin;
    private int gem;

    private TimeManager timeManager;
    private ScoreManager scoreManager;
    private PlayerController player;

    private void Start()
    {
        resultPanel.SetActive(false); // Ẩn UI lúc đầu
        foreach (GameObject star in stars) star.SetActive(false);
        nextButton.SetActive(false);

        timeManager = FindAnyObjectByType<TimeManager>();
        scoreManager = FindAnyObjectByType<ScoreManager>();

        level = SceneManager.GetActiveScene().buildIndex;
        bestScore = StoreGameData.Instance.GetSavedScore(level);
        bestStars = StoreGameData.Instance.GetSavedStars(level);
        levelName = StoreGameData.Instance.GetSavedLevelName(level);

        Debug.Log($"Level {level}: Best Score = {bestScore}, Best Stars = {bestStars}, Name = {levelName}");

        coin = StoreGameData.Instance.GetSavedCoin();
        gem = StoreGameData.Instance.GetSavedGem();
        Debug.Log($"Coin: {coin}, Gem: {gem}");
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            player = FindAnyObjectByType<PlayerController>();

            if (player != null && player.hasCrashed)
            {
                return; 
            }

            finishEffect.Play();
            GetComponent<AudioSource>().Play();
            timeManager.StopTimer();
            SoundManager.instance.StopAllSounds();
            StopGame();

            //Invoke("LoadNextScene", loadDelay);
        }
    }

    void StopGame()
    {
        player = FindAnyObjectByType<PlayerController>();
        if (player != null) player.enabled = false;

       scoreManager.StopScoring();

        Time.timeScale = 0f; 

        CalculateResult();
    }

    void SavedCoinAndGem()
    {
        int coin = scoreManager.GetCoin();
        int gem = scoreManager.GetGem();
		StoreGameData.Instance.SaveCoinAndGem(coin, gem);
	}


	void CalculateResult()
    {
        float healthCurrent = player.currentHealth;
        int healthBonus = CalculateHealthBonus(healthCurrent);

        float totalTime = timeManager.GetCurrentTime(); // Lấy thời gian từ TimeManager
        int processScore = scoreManager.GetScore(); // Lấy điểm quá trình

        int finalScore = (int) (baseMapScore / totalTime) + processScore + healthBonus;

        // Hiển thị điểm
        resultPanel.SetActive(true);
        finalScoreText.text = $"Final Score: {finalScore}";

        // Tính số sao đạt được
       var starCount = CalculateStars(totalTime, healthCurrent);


        for (int i = 0; i < starCount; i++)
        {
            stars[i].SetActive(true);
        }

        nextButton.SetActive(true);
		SavedCoinAndGem();
		StoreGameData.Instance.SaveGameResult(level, finalScore, starCount);
    }

    private int CalculateHealthBonus(float health)
    {
        if (health == 3) return 1200;
        if (health == 2) return 900;
        if (health == 1) return 600;
        return 0;
    }

    private int CalculateStars(float time, float health)
    {
        if (time <= 30f && health == 3) return 3; // Chỉ đạt 3 sao khi full máu
        if (time <= 40f && health > 1) return 2;
        return 1;
    }

    public void LoadNextScene()
    {
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        int nextSceneIndex = currentSceneIndex + 1;

        if (nextSceneIndex < SceneManager.sceneCountInBuildSettings)
        {
            SceneManager.LoadScene(nextSceneIndex);
            Time.timeScale = 1f;
        }
        else
        {
            SceneManager.LoadScene(0);
        }
    }
}
