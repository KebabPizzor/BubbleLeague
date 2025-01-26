using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_timerText;
    [SerializeField] private TextMeshProUGUI m_roleText;
    [SerializeField] private Slider m_energySlider;
    public void SetRole(GameController.Role role)
    {
        switch (role)
        {
            case GameController.Role.Attacker:
                m_roleText.text = "Attacker";
                break;
            case GameController.Role.Defender:
                m_roleText.text = "Defender";
                break;
        }
    }
    
    public void UpdateEnergy(float energy)
    {
        m_energySlider.value = energy / 100;
    }
    
    public void UpdateTimer(float time)
    {
        m_timerText.text = time.ToString("F2");
    }
}
