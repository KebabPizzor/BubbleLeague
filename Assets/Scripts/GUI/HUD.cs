using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_timerText;
    [SerializeField] private Slider m_energySlider;
    public void Initialize()
    {
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
