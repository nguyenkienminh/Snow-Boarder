using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadMapManager : MonoBehaviour
{
    public Transform mapSceneParent; // Gán GameObject chứa tất cả MapScene
    public AudioSource audioSource;
    public AudioClip backgroundMusic;
    public GameObject ScorePanel;

    private int mapScore;
    private int starCount;

    private void Awake()
    {
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }
    private void Start()
    {
        PlayBackgroundMusic();
        ScorePanel.SetActive(true);
        ShowMapInfo();

    }
    private void PlayBackgroundMusic()
    {
        if (audioSource != null && backgroundMusic != null)
        {
            audioSource.PlayOneShot(backgroundMusic);
        }
        else
        {
            Debug.LogError("AudioSource hoặc backgroundMusic chưa được gán!");
        }
    }
    void ShowMapInfo()
    {
        ScorePanel.SetActive(true);
        int count = 1;
        foreach (Transform map in mapSceneParent)
        {
            foreach (Transform childMap in map)
            {
                Transform nameTransform = childMap.Find("MapName");
                Transform scoreTransform = childMap.Find("MapScore");
                Transform starContainer = childMap.Find("StarContainer");

                if (nameTransform != null && scoreTransform != null && starContainer != null)
                {
                    string mapName = StoreGameData.Instance.GetSavedLevelName(count);

                    if (mapName == "null")
                    {
                        mapScore = 0;
                        starCount = 0;
                        nameTransform.GetComponent<TextMeshProUGUI>().text = "Level" + count.ToString();
                    }
                    else
                    {

                        mapScore = StoreGameData.Instance.GetSavedScore(count);
                        starCount = StoreGameData.Instance.GetSavedStars(count);
                        nameTransform.GetComponent<TextMeshProUGUI>().text = mapName.ToString();
                    }

                    // Cập nhật UI điểm
                    scoreTransform.GetComponent<TextMeshProUGUI>().text = mapScore.ToString();

                    // Cập nhật số sao
                    for (int i = 0; i < starContainer.childCount; i++)
                    {
                        starContainer.GetChild(i).gameObject.SetActive(i < starCount);
                    }
                }
            }
            count++;
        }
    }

    public void BackToMenu()
    {
        Time.timeScale = 0f;
        ScorePanel.SetActive(false);
        SceneManager.LoadScene("MenuGame");

    }
}

