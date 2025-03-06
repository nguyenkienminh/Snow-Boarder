using UnityEngine;
using UnityEngine.SceneManagement;

public class SoundMenuGameManager : MonoBehaviour
{
    public static SoundMenuGameManager instance;
    private AudioSource audioSource;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
            audioSource.Play(); 
        }
        else
        {
            Destroy(gameObject); 
        }
    }

    public void StopBackgroundMusic()
    {
        if (audioSource != null && SceneManager.GetActiveScene().name == "MenuGame")
        {
            audioSource.Stop();
        }
    }

    public void PlayBackgroundMusic()
    {
        if (audioSource != null)
        {

            if (SceneManager.GetActiveScene().name == "MenuGame")
            {
                AudioListener.pause = false;
                audioSource.mute = false;
                if (!audioSource.isPlaying)
                {
                    audioSource.Play();
                }
            }
        }
        else
        {
            Debug.LogWarning("AudioSource chưa được khởi tạo!");
        }
    }
}
