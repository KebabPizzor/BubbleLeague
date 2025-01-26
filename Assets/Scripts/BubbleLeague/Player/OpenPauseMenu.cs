using UnityEngine;

public class OpenPauseMenu : MonoBehaviour
{
    [SerializeField] private GameObject m_pauseMenuPrefab;

    private GameObject m_pauseMenuRef;

    public void OnPause()
    {
        if (m_pauseMenuRef == null)
        {
            m_pauseMenuRef = Instantiate(m_pauseMenuPrefab);
        }
        else
        {
            Destroy(m_pauseMenuRef);
        }
    }
}