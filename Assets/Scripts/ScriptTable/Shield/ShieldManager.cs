using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class ShieldManager : MonoBehaviour
{
    public ShieldData shieldData;      
    public Image shieldIconImage; 

    private bool shieldAvailable = false; // Cờ báo có khiên hay không
    private bool isShieldActive = false;  // Cờ báo shield đang hoạt động

    private void Start()
    {
        if (shieldIconImage != null)
        {
            shieldIconImage.enabled = false;
        }
    }

    // Phương thức gọi khi player thu thập được khiên
    public void CollectShield(ShieldData newShieldData)
    {
        shieldData = newShieldData;
        shieldAvailable = true;

        if (shieldIconImage != null)
        {
            shieldIconImage.sprite = shieldData.shieldIcon;
            shieldIconImage.enabled = true;
        }
    }

    private void Update()
    {
        // Khi có khiên, nếu player nhấn Z và shield chưa hoạt động, kích hoạt shield
        if (shieldAvailable && !isShieldActive && Input.GetKeyDown(KeyCode.Z))
        {
            ActivateShield();
        }
    }

    private void ActivateShield()
    {
        isShieldActive = true;
        shieldAvailable = false;

        if (shieldIconImage != null)
        {
            shieldIconImage.enabled = false;
        }

        PlayerController player = FindAnyObjectByType<PlayerController>();
        if (player != null)
        {
            player.SetInvincible(true);
        }

        StartCoroutine(ShieldDuration());
    }

    private IEnumerator ShieldDuration()
    {
        yield return new WaitForSeconds(shieldData.duration);

        PlayerController player = FindAnyObjectByType<PlayerController>();
        if (player != null)
        {
            player.SetInvincible(false);
        }
        isShieldActive = false;
    }
}
