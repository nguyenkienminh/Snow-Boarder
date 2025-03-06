using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StoreManagement : MonoBehaviour
{
	public Transform mapStatus; // Gán GameObject chứa tất cả MapScene
    public AudioSource audioSource;
    public AudioClip backgroundMusic;
    public GameObject CoinPanel;
	public GameObject GemPanel;

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
		foreach (Transform map in mapStatus) // Lặp qua tất cả MapScene
		{
			foreach (Transform item in map)
			{
				Transform nameTransform = map.Find("Text");
				if (nameTransform != null)
				{
					if (map.name == "Coin")
					{
						int coinData = StoreGameData.Instance.GetSavedCoin();
						nameTransform.GetComponent<TextMeshProUGUI>().text = coinData.ToString();
					}
					else if (map.name == "Gem")
					{
						int gemData = StoreGameData.Instance.GetSavedGem();
						nameTransform.GetComponent<TextMeshProUGUI>().text = gemData.ToString();
					}
				}
			}
		}
	}

	public void BackToMenu()
	{
		Time.timeScale = 0f;
		SceneManager.LoadScene("MenuGame");
	}


}
