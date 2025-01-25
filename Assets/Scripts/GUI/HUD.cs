using TMPro;
using UnityEngine;

public class HUD : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI m_timerText;
    
    public void Initialize()
    {
    }
    
    public void UpdateTimer(float time)
    {
        m_timerText.text = time.ToString("F2");
    }
}
