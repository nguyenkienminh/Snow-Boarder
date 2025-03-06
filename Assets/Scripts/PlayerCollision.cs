using UnityEngine;

public class PlayerCollison : MonoBehaviour
{
    private ScoreManager scoreManager;
    public ShieldData shieldData;
    private void Awake()
    {
        scoreManager = FindAnyObjectByType<ScoreManager>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Coin"))
        {
            Destroy(collision.gameObject);
            SoundManager.instance?.PlayCoinClip();
            scoreManager.AddCoin(5);
        }
        else if (collision.CompareTag("Gem"))
        {
            Destroy(collision.gameObject);
            SoundManager.instance?.PlayDiamondClip();
            scoreManager.AddGem(20);
        }
        else if (collision.CompareTag("snow"))
        {
            Destroy(collision.gameObject);
            SoundManager.instance?.PlaySnowClip();
            scoreManager.AddScore(50);
        }
        else if (collision.CompareTag("Shield"))
        {
            if (shieldData != null)
            {
                ShieldManager shieldManager = FindAnyObjectByType<ShieldManager>();
                if (shieldManager != null)
                {
                    shieldManager.CollectShield(shieldData);
                }
            }
            Destroy(collision.gameObject);
            SoundManager.instance?.PlaySnowClip();
        }
    }


}
