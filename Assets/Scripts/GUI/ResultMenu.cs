using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultMenu : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_resultText;
    [SerializeField] private Button m_rematchButton;
    
    public void Initialize(GameController.Result result)
    {
        switch (result)
        {
            case GameController.Result.Player1:
                m_resultText.text = "Player 1 Wins!";
                break;
            case GameController.Result.Player2:
                m_resultText.text = "Player 2 Wins!";
                break;
            case GameController.Result.Draw:
                m_resultText.text = "Draw!";
                break;
        }
        
        m_rematchButton.onClick.AddListener(() =>
        {
            SceneManager.LoadScene("Test");
        });
    }
}