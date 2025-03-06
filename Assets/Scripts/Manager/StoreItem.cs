using System.Collections.Generic;
using UnityEngine;

public class StoreItem : MonoBehaviour
{
    public static StoreItem Instance;
    private List<BuyItems> itemsList = new List<BuyItems>();

    public SpriteRenderer[] playerSpriteRenderers;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    public void RegisterItem(BuyItems item)
    {
        if (!itemsList.Contains(item))
        {
            itemsList.Add(item);
        }
    }

    public void UpdateAllUI()
    {
        foreach (var item in itemsList)
        {
            item.UpdateCoinGemUI();
        }
    }

    public void SetActiveItem(BuyItems activeItem)
    {

        foreach (BuyItems item in itemsList)
        {

            if (PlayerPrefs.GetInt(item.itemName, 0) == 1)
            {
                bool isActive = item == activeItem;
                item.SetActiveState(!isActive);
            }
        }

        // Lấy màu từ PlayerPrefs
        string savedColor = PlayerPrefs.GetString(activeItem.itemName + "_color", "white");
        Debug.Log(savedColor);

        // Gọi PlayerController để đổi màu
        if (PlayerController.Instance != null)
        {
            PlayerController.Instance.SetBoarderColor(savedColor);
        }


    }


    public void SetPlayerSpriteRenderers(SpriteRenderer[] spriteRenderers)
    {
        playerSpriteRenderers = spriteRenderers;
    }


}
