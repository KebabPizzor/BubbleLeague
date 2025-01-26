using System;
using UnityEngine;

public class PlayerAttributes : MonoBehaviour
{
    public event Action<float> EnergyUpdated;
    [SerializeField] private float m_maxEnergy = 100.0f;
    private float m_currentEnergy;

    public void Initialize()
    {
        m_currentEnergy = m_maxEnergy;
    }

    public void ChangeEnergy(float amount)
    {
        m_currentEnergy += amount;
        if (m_currentEnergy > m_maxEnergy)
        {
            m_currentEnergy = m_maxEnergy;
        }
        else if (m_currentEnergy < 0)
        {
            m_currentEnergy = 0;
        }

        EnergyUpdated?.Invoke(m_currentEnergy);
    }

    public void RefillEnergy()
    {
        ChangeEnergy(m_maxEnergy / 2);
    }
    
    public bool IsAtMaxEnergy()
    {
        return m_currentEnergy >= m_maxEnergy;
    }

    public void BroadcastEnergy()
    {
        EnergyUpdated?.Invoke(m_currentEnergy);
    }

    public float GetCurrentEnergy()
    {
        return m_currentEnergy;
    }
}