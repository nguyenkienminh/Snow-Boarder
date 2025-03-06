using UnityEngine;
using UnityEngine.SceneManagement;

public class InstructionUIManager : MonoBehaviour
{
    public static InstructionUIManager Instance { get; private set; }
    public AudioSource audioSource;
    public AudioClip backgroundMusic;
    public GameObject Instruction;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }
    private void Start()
    {
        PlayBackgroundMusic();
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
    public void MoveInMenuGame()
    {
        SceneManager.LoadScene("MenuGame");
        Destroy(gameObject);
    }
}
