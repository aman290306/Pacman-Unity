using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class MainMenu : MonoBehaviour
{   
    public TMP_Text HighScoreText;
    public void Start()
    {
        int highScore = PlayerPrefs.GetInt("HighScore", 0);
        HighScoreText.text = "HighScore" + highScore;
    }
    public void GoToScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Game Quit");
    }
  
}
