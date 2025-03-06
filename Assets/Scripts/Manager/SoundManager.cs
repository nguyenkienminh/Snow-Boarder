using System.Collections;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance { get; private set; }

    [Header("Audio Sources")]
    private AudioSource jumpSource;
    private AudioSource rotateSource;
    private AudioSource boostSource;
    private AudioSource movementSource;
    private AudioSource coinSource;
    private AudioSource diamondSource;
    private AudioSource snowSource;
    private AudioSource dieSource;
    private AudioSource hurtSource;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip jumpClip;
    [SerializeField] private AudioClip rotateSkyClip;
    [SerializeField] private AudioClip boostClip;
    [SerializeField] private AudioClip movementClip;
    [SerializeField] private AudioClip coinClip;
    [SerializeField] private AudioClip diamondClip;
    [SerializeField] private AudioClip snowClip;
    [SerializeField] private AudioClip dieClip;
    [SerializeField] private AudioClip hurtClip;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Tạo từng AudioSource riêng biệt
        jumpSource = CreateAudioSource("JumpSource");
        rotateSource = CreateAudioSource("RotateSource");
        boostSource = CreateAudioSource("BoostSource", true); // Loop
        movementSource = CreateAudioSource("MovementSource", true); // Loop
        coinSource = CreateAudioSource("CoinSource");
        diamondSource = CreateAudioSource("DiamondSource");
        snowSource = CreateAudioSource("SnowSource");
        dieSource = CreateAudioSource("DieSource");
        hurtSource = CreateAudioSource("HurtSource"); 
    }

    // Tạo AudioSource mới
    private AudioSource CreateAudioSource(string name, bool loop = false)
    {
        GameObject audioObj = new GameObject(name);
        audioObj.transform.SetParent(transform);
        AudioSource newSource = audioObj.AddComponent<AudioSource>();
        newSource.loop = loop;
        newSource.playOnAwake = false;
        return newSource;
    }

    public void PlayJumpClip()
    {
        jumpSource.PlayOneShot(jumpClip);
    }


    public void PlayRotateSkyClip()
    {
        if (!rotateSource.isPlaying)
        {
            rotateSource.PlayOneShot(rotateSkyClip);
        }
    }

    public void StopRotateSkyClip()
    {
        rotateSource.Stop();
    }

    public bool IsPlayingRotateSkyClip()
    {
        return rotateSource.isPlaying;
    }

    public void PlayBoostSound()
    {
        if (!boostSource.isPlaying)
        {
            boostSource.clip = boostClip;
            boostSource.PlayOneShot(boostClip);
        }
    }

    public void StopBoostSound()
    {
        boostSource.Stop();
    }


    public void PlayMovementClip()
    {
        if (!movementSource.isPlaying)
        {
            movementSource.clip = movementClip;
            movementSource.volume = 0.5f;
            movementSource.PlayOneShot(movementClip);
        }
    }

    public void StopMovementClip()
    {
        movementSource.Stop();
    }

    public void PlayCoinClip()
    {
        coinSource.PlayOneShot(coinClip);
    }


    public void PlayDiamondClip()
    {
        diamondSource.PlayOneShot(diamondClip);
    }


    public void PlaySnowClip()
    {
        snowSource.PlayOneShot(snowClip);
    }
    public AudioSource PlayDieClip()
    {
        dieSource.volume = 0.7f;
        dieSource.clip = dieClip;
        dieSource.Play();
        return dieSource; // Trả về AudioSource để kiểm tra thời gian phát
    }

    public void PlayHurtClip()
    {
       hurtSource.PlayOneShot(hurtClip);
    }

    public bool IsPlayingMovement()
    {
        return movementSource.isPlaying;
    }

    public void StopAllSounds()
    {
        jumpSource.Stop();
        rotateSource.Stop();
        boostSource.loop = false;
        boostSource.Stop();
        movementSource.loop = false;
        movementSource.Stop();
        movementSource.Pause();  
        movementSource.clip = null;
        coinSource.Stop();
        diamondSource.Stop();
        snowSource.Stop();
        dieSource.Stop();
        hurtSource.Stop();

        Debug.Log("Stop All Sounds");
    }

    public void PlayAllSounds()
    {
        if (!jumpSource.isPlaying) jumpSource.Play();
        if (!rotateSource.isPlaying) rotateSource.Play();
        if (!boostSource.isPlaying) boostSource.Play();
        if (!movementSource.isPlaying) movementSource.Play();
        if (!coinSource.isPlaying) coinSource.Play();
        if (!diamondSource.isPlaying) diamondSource.Play();
        if (!snowSource.isPlaying) snowSource.Play();
        if (!dieSource.isPlaying) dieSource.Play();
        if (!hurtSource.isPlaying) hurtSource.Play();

        Debug.Log("Play All Sounds");
    }
}
