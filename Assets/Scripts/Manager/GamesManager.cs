using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public string previousScene { get; private set; }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else if (Instance != this)
        {
            Destroy(gameObject);
            return;
        }
    }


    public void MoveGameOver()
    {
        if (SceneManager.GetActiveScene().name != "GameOverScene")
        {
            previousScene = SceneManager.GetActiveScene().name;
            Time.timeScale = 0f;
            SceneManager.LoadScene("GameOverScene");

        }
    }


}
