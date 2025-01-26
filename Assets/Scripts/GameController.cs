using System;
using System.Collections.Generic;
using BubbleLeague;
using UnityEngine;
using UnityEngine.Serialization;

public class GameController : MonoBehaviour
{
    public event Action<float> TimerUpdated;
    [SerializeField] private float m_gameDuration = 60.0f;
    [SerializeField] GameObject playerHudPrefab;
    [SerializeField] private Transform defenderSpawnPoint;
    [SerializeField] private Transform attackerSpawnPoint;

    private readonly List<Player> _players = new();
    private float m_timer;
    private float? m_player1Score;
    private bool started;

    public void StartGame()
    {
        Debug.Log("Start Game!");
        m_timer = m_gameDuration;
        MakeDefender(_players[0]);
        MakeAttacker(_players[1]);
        FindFirstObjectByType<Hoop>().Reset();
        Time.timeScale = 1f;
        started = true;
    }

    public void EndGame()
    {
        if (m_timer < 0) m_timer = 0;
        m_player1Score = m_timer;
        Time.timeScale = 0f;
        StartRematch();
    }

    public void StartRematch()
    {
        Debug.Log("Start Rematch!");
        m_timer = m_gameDuration - m_player1Score!.Value;
        MakeDefender(_players[1]);
        MakeAttacker(_players[0]);
        FindFirstObjectByType<Hoop>().Reset();
        Time.timeScale = 1f;
    }

    public void EndRematch()
    {
        if (m_timer < 0) m_timer = 0;
        Time.timeScale = 0f;
        if (m_timer == 0)
        {
            Time.timeScale = 0f;
            if (m_player1Score == 0)
            {
                DrawPlayers();
            }

            WinPlayer1();
        }
        else
        {
            WinPlayer2();
        }
    }

    private void DrawPlayers()
    {
        Debug.Log("Draw between both players.");
        QuitGame();
    }

    private void Update()
    {
        if (!started) return;
        m_timer -= Time.deltaTime;
        TimerUpdated?.Invoke(m_timer);
        if (m_timer <= 0)
        {
            TimerEnded();
        }
    }

    public void TimerEnded()
    {
        if (m_player1Score == null)
        {
            EndGame();
        }
        else
        {
            EndRematch();
        }
    }

    public void HoopTriggered()
    {
        if (m_player1Score == null)
        {
            EndGame();
        }
        else
        {
            EndRematch();
        }
    }

    private void WinPlayer1()
    {
        Debug.Log("Player 1 Wins.");
        QuitGame();
    }

    private void WinPlayer2()
    {
        Debug.Log("Player 2 Wins.");
        QuitGame();
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    public void RegisterPlayer(Player player)
    {
        _players.Add(player);

        var hud = Instantiate(playerHudPrefab).GetComponent<HUD>();
        var canvas = hud.GetComponentInChildren<Canvas>();
        canvas.worldCamera = player.GetComponentInChildren<Camera>();
        hud.Initialize();
        TimerUpdated += hud.UpdateTimer;
        
        var pa = player.GetComponentInChildren<PlayerAttributes>();
        pa.Initialize();
        pa.EnergyUpdated += hud.UpdateEnergy;
        pa.BroadcastEnergy();
        
        if (_players.Count == 2) StartGame();
    }

    public void MakeAttacker(Player player)
    {
        player.gameObject.layer = LayerMask.NameToLayer("Attacker");
        player.transform.position = attackerSpawnPoint.position;
        player.GetComponentInChildren<BallMovement>().transform.rotation = attackerSpawnPoint.rotation;
        player.Reset();
    }

    public void MakeDefender(Player player)
    {
        player.gameObject.layer = LayerMask.NameToLayer("Defender");
        player.transform.position = defenderSpawnPoint.position;
        player.GetComponentInChildren<BallMovement>().transform.rotation = defenderSpawnPoint.rotation;
        player.Reset();
    }

    public void UnregisterPlayer(Player player)
    {
        _players.Remove(player);
    }
}