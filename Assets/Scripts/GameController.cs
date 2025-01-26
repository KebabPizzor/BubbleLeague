using System;
using System.Collections.Generic;
using BubbleLeague;
using UnityEngine;
using UnityEngine.Serialization;

public class GameController : MonoBehaviour
{
    public enum Result
    {
        Player1,
        Player2,
        Draw
    }

    public enum Role
    {
        Attacker,
        Defender
    }

    public event Action<float> TimerUpdated;
    [SerializeField] private float m_gameDuration = 60.0f;
    [SerializeField] private Transform defenderSpawnPoint;
    [SerializeField] private Transform attackerSpawnPoint;

    //GUI
    [SerializeField] private GameObject m_playerHudPrefab;
    [SerializeField] private GameObject m_resultMenuPrefab;
    [SerializeField] private GameObject m_mainMenuPrefab;


    private readonly List<Player> _players = new();
    private float m_timer;
    private float? m_player1Score;
    private bool started;
    private GameObject m_startMenuRef;

    private void Awake()
    {
        m_startMenuRef = Instantiate(m_mainMenuPrefab);
    }

    public void StartGame()
    {
        Destroy(m_startMenuRef);
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

    private void DrawPlayers()
    {
        ShowResult(Result.Draw);
    }

    private void WinPlayer1()
    {
        ShowResult(Result.Player1);
    }


    private void WinPlayer2()
    {
        ShowResult(Result.Player2);
    }

    private void ShowResult(Result result)
    {
        var resultMenu = Instantiate(m_resultMenuPrefab).GetComponent<ResultMenu>();
        resultMenu.Initialize(result, this);
        
    }

    public void QuitGame()
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

        var hud = Instantiate(m_playerHudPrefab).GetComponent<HUD>();
        var canvas = hud.GetComponentInChildren<Canvas>();
        canvas.worldCamera = player.GetComponentInChildren<Camera>();
        TimerUpdated += hud.UpdateTimer;
        player.hudRef = hud;

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
        player.hudRef.SetRole(Role.Attacker);
    }

    public void MakeDefender(Player player)
    {
        player.gameObject.layer = LayerMask.NameToLayer("Defender");
        player.transform.position = defenderSpawnPoint.position;
        player.GetComponentInChildren<BallMovement>().transform.rotation = defenderSpawnPoint.rotation;
        player.Reset();
        player.hudRef.SetRole(Role.Defender);
    }

    public void UnregisterPlayer(Player player)
    {
        _players.Remove(player);
    }
}