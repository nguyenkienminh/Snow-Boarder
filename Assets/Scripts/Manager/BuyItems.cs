using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class BuyItems : MonoBehaviour
{
	public string itemName;
	public int itemCoin;
	public int itemGem;
	public Image BG;

	public TextMeshProUGUI coinText;
	public TextMeshProUGUI gemText;

	public GameObject buyButton;
	public GameObject activeButton;
	public GameObject inActiveButton;

	public bool IsActive { get; private set; }

	private void Start()
	{
		StoreItem.Instance.RegisterItem(this);

		// Kiểm tra nếu chưa gán Text UI thì tìm trong Scene
		if (coinText == null)
		{
			GameObject coinObj = GameObject.Find("CoinText");
			if (coinObj != null)
			{
				coinText = coinObj.GetComponent<TextMeshProUGUI>();
			}
			else
			{
				Debug.LogError("Không tìm thấy CoinText trong Scene!");
			}
		}

		if (gemText == null)
		{
			GameObject gemObj = GameObject.Find("GemText");
			if (gemObj != null)
			{
				gemText = gemObj.GetComponent<TextMeshProUGUI>();
			}
			else
			{
				Debug.LogError("Không tìm thấy GemText trong Scene!");
			}
		}

		bool isPurchased = PlayerPrefs.GetInt(itemName, 0) == 1;
		bool isActive = PlayerPrefs.GetInt(itemName + "_active", 0) == 1;

		buyButton?.SetActive(!isPurchased);
		activeButton?.SetActive(isPurchased && !isActive);
		BG.enabled = isActive;
		inActiveButton?.SetActive(isPurchased && isActive);

		UpdateCoinGemUI();
	}

	public void OnBuyButtonClick()
	{
		int coinData = StoreGameData.Instance.GetSavedCoin();
		int gemData = StoreGameData.Instance.GetSavedGem();

		int coinCurrent = coinData - itemCoin;
		int gemCurrent = gemData - itemGem;

		if (coinCurrent >= 0 && gemCurrent >= 0)
		{
			// Cập nhật dữ liệu vào StoreGameData
			StoreGameData.Instance.SaveCoin(coinCurrent);
			StoreGameData.Instance.SaveGem(gemCurrent);


			PlayerPrefs.SetInt(itemName, 1);
			PlayerPrefs.Save();

            string purchasedSkateboards = PlayerPrefs.GetString("PurchasedSkateboards", "");
            if (!purchasedSkateboards.Contains(itemName))
            {
                if (string.IsNullOrEmpty(purchasedSkateboards))
                {
                    purchasedSkateboards = itemName;
                }
                else
                {
                    purchasedSkateboards += "," + itemName;
                }
                PlayerPrefs.SetString("PurchasedSkateboards", purchasedSkateboards);
                PlayerPrefs.Save();
            }

            buyButton?.SetActive(false);
			activeButton?.SetActive(true);

			StoreItem.Instance.UpdateAllUI();
		}
		else
		{
			Debug.Log("Không đủ tiền để mua!");
		}

	}

	public void UpdateCoinGemUI()
	{
		if (coinText != null)
		{
			coinText.text = StoreGameData.Instance.GetSavedCoin().ToString();
		}
		else
		{
			Debug.LogError("coinText chưa được gán hoặc không tìm thấy!");
		}

		if (gemText != null)
		{
			gemText.text = StoreGameData.Instance.GetSavedGem().ToString();
		}
		else
		{
			Debug.LogError("gemText chưa được gán hoặc không tìm thấy!");
		}
	}


	public void OnActiveButtonClick()
	{
		if (PlayerPrefs.GetInt(itemName, 0) == 1)
		{
			StoreItem.Instance.SetActiveItem(this);
		}
	}

	public void OnInActiveButtonClick()
	{
		if (PlayerPrefs.GetInt(itemName, 0) == 1) 
		{
			SetActiveState(true);
		}
	}


	public void SetActiveState(bool isActive)
	{
		activeButton?.SetActive(isActive);
		BG.enabled = !isActive;
        inActiveButton?.SetActive(!isActive);

		PlayerPrefs.SetInt(itemName + "_active", !isActive ? 1 : 0);
		Debug.Log(itemName + "_active: " + !isActive);
        PlayerPrefs.Save();
	}
}


