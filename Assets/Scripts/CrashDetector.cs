using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CrashDetector : MonoBehaviour
{
    public static CrashDetector instance { get; private set; }

    [SerializeField] ParticleSystem crashEffect;
    [SerializeField] AudioClip crashSFX;

    bool hasCrashed = false;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground") && !hasCrashed)
        {
            hasCrashed = true;
            PlayerController player = FindFirstObjectByType<PlayerController>();

            GetComponent<AudioSource>().PlayOneShot(crashSFX);
            crashEffect.Play();
            if (player != null)
            {
                float crashDuration = crashSFX.length;
                StartCoroutine(WaitAndCrash(player, 0.3f));
            }
        }
    }

    private IEnumerator WaitAndCrash(PlayerController player, float delay)
    {
        yield return new WaitForSeconds(delay);

        if (player != null)
        {
            player.Crash();
        }
    }

}
