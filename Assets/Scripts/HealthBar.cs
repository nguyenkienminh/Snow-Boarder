using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public PlayerController playerHealth;

    public Image totalHealthBar;

    public Image currentHealthBar;

    private void Start()
    {
        if (playerHealth == null)
        {
            playerHealth = FindAnyObjectByType<PlayerController>();
        }

        if (playerHealth != null)
        {
            totalHealthBar.fillAmount = playerHealth.currentHealth / 10;
        }
    }

    private void Update()
    {
        currentHealthBar.fillAmount = playerHealth.currentHealth / 10;
    }
}
