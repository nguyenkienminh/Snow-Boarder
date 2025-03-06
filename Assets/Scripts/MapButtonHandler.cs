using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MapButtonHandler : MonoBehaviour
{
    public TextMeshProUGUI warning; 

    public void OnMapButtonClick(GameObject clickedButton)
    {
        Transform mapScene = null;
        warning.text = "";
        // Tìm Transform có tên chứa "MapScene"
        foreach (Transform child in clickedButton.transform)
        {
            if (child.name.Contains("MapScene"))
            {
                mapScene = child;
                break;
            }
        }

        if (mapScene != null)
        {
            TextMeshProUGUI nameText = mapScene.Find("MapName")?.GetComponent<TextMeshProUGUI>();

            if (nameText != null)
            {
                string mapName = nameText.text;
                int mapIndex = ExtractMapIndex(mapName);

                if (CanPlayMap(mapIndex))
                {
                    SceneManager.LoadScene(mapName);
                    Time.timeScale = 1f;
                }
                else
                {
                    warning.text = "You need to complete the previous map first!";
                }
            }
        }
    }


    private int ExtractMapIndex(string mapName)
    {
        int index = 0;
        if (mapName.StartsWith("Level"))
        {
            int.TryParse(mapName.Substring(5), out index);
        }
        return index;
    }

    private bool CanPlayMap(int mapIndex)
    {
        if (mapIndex <= 1) return true;

        int previousMapScore = PlayerPrefs.GetInt($"Level_{mapIndex - 1}_Score", 0);
        return previousMapScore > 0;
    }
}

