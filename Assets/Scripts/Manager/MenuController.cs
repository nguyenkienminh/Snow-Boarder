using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
   public GameObject BackGround;
    public AudioSource audioSource; 
    public AudioClip backgroundMusic;
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
    }
    public void StartGame()
    {
        PlayBackgroundMusic();

        SceneManager.LoadScene("Level1");
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

    public void QuitGame()
    {
        Application.Quit();
    }

    public void MoveInstruction()
    {
        BackGround.SetActive(false);
        AudioListener.pause = false;
        SceneManager.LoadScene("Instruction");
    }

    public void MoveScorePanel()
    {
        BackGround.SetActive(false);
        PlayBackgroundMusic();
        if (StoreGameData.Instance == null)
        {
            GameObject obj = new GameObject("GameStoreData");
            obj.AddComponent<StoreGameData>();
        }

        SceneManager.LoadScene("ScoreMapScene");

    }

    public void MoveLoadMapScene()
    {
        PlayBackgroundMusic();
        BackGround.SetActive(false);
        if (StoreGameData.Instance == null)
        {
            GameObject obj = new GameObject("GameStoreData");
            obj.AddComponent<StoreGameData>();
        }
        SceneManager.LoadScene("MapLoadScene");
    }

	public void MoveStoreScene()
	{
        PlayBackgroundMusic();
        BackGround.SetActive(false);
		if (StoreGameData.Instance == null)
		{
			GameObject obj = new GameObject("GameStoreData");
			obj.AddComponent<StoreGameData>();
		}
		SceneManager.LoadScene("Store");
	}

}
