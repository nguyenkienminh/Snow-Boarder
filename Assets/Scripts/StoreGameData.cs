using UnityEngine;
using UnityEngine.SceneManagement;

public class StoreGameData : MonoBehaviour
{
	public static StoreGameData Instance;

	private void Awake()
	{

		if (Instance == null)
		{
			Instance = this;
			DontDestroyOnLoad(gameObject); // Không bị xóa khi chuyển Scene
		}
		else
		{
			Destroy(gameObject); // Nếu đã có 1 instance, xóa cái mới
		}


	}

	public void SaveGameResult(int level, int score, int stars)
	{
		int bestScore = GetSavedScore(level);

		string levelName = SceneManager.GetActiveScene().name;
		PlayerPrefs.SetString($"Level_{level}_Name", levelName);

		if (score > bestScore)
		{
			PlayerPrefs.SetInt($"Level_{level}_Score", score);
		}

		int bestStars = GetSavedStars(level);
		if (stars > bestStars)
		{
			PlayerPrefs.SetInt($"Level_{level}_Stars", stars);
		}

		PlayerPrefs.Save();
	}

	public int GetSavedScore(int level)
	{
		return PlayerPrefs.GetInt($"Level_{level}_Score", 0);
	}

	public int GetSavedStars(int level)
	{
		return PlayerPrefs.GetInt($"Level_{level}_Stars", 0);
	}
	public string GetSavedLevelName(int level)
	{
		return PlayerPrefs.GetString($"Level_{level}_Name", $"null");
	}

	public void DeleteSavedData(int level)
	{
		PlayerPrefs.DeleteKey($"Level_{level}_Score");
		PlayerPrefs.DeleteKey($"Level_{level}_Stars");
		PlayerPrefs.DeleteKey($"Level_{level}_Name");

		PlayerPrefs.Save();
	}

	public void DeleteAllSavedData()
	{
		PlayerPrefs.DeleteAll();
		PlayerPrefs.Save();
	}


	//SAVE COIN AND DIAMOND
	public int GetSavedCoin()
	{
		return PlayerPrefs.GetInt("Coin", 0);
	}

	public int GetSavedGem()
	{
		return PlayerPrefs.GetInt("Gem", 0);
	}

	public void SaveCoinAndGem(int coin, int gem)
	{
		int coinStored = GetSavedCoin();
		int gemStored = GetSavedGem();

		PlayerPrefs.SetInt("Gem", gemStored + gem);
		PlayerPrefs.SetInt("Coin", coinStored + coin);

		PlayerPrefs.Save();
	}


	public void SaveCoin(int coin) => PlayerPrefs.SetInt("Coin", coin);
	public void SaveGem(int gem) => PlayerPrefs.SetInt("Gem", gem);
}
