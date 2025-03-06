using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{

    [SerializeField] GameObject UI;

    private void Awake()
    {
        UI.SetActive(true);
    }

    private void Start()
    {
        UI.SetActive(true);
    }

    public void NewGame()
    {
        if (!string.IsNullOrEmpty(GameManager.Instance.previousScene))
        {
            UI.SetActive(false);
            SceneManager.LoadScene(GameManager.Instance.previousScene);
            Time.timeScale = 1f;
        }
    }

    public void ReturnMenuGame()
    {
        Time.timeScale = 0f;
        SoundManager.instance.StopAllSounds();
        UI.SetActive(false);
        SceneManager.LoadScene("MenuGame");
    }

}
