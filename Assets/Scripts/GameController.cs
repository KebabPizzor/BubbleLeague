using System;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public event Action<float> TimerUpdated;
    [SerializeField] private float m_gameDuration = 60.0f;
    [SerializeField] GameObject hudPrefab;
    
    private readonly List<Player> _players = new();
    private float m_timer;
    
    public void StartGame()
    {
        m_timer = m_gameDuration;
        var hud = Instantiate(hudPrefab).GetComponent<HUD>();
        hud.Initialize();
        TimerUpdated += hud.UpdateTimer;
    }

    private void Update()
    {
        m_timer -= Time.deltaTime;
        TimerUpdated?.Invoke(m_timer);
        if (m_timer <= 0)
        {
            //EndGame();
        }
    }

    private void EndGame()
    {
        Time.timeScale = 0;
    }
    
    public void RegisterPlayer(Player player)
    {
        _players.Add(player);
        if (_players.Count == 1)
        {
            player.MakeAttacker();
        }
        else
        {
            player.MakeDefender();
        }
    }

    public void UnregisterPlayer(Player player)
    {
        _players.Remove(player);
    }
}
