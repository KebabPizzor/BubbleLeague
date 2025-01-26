using System;
using System.Collections.Generic;
using System.Linq;
using BubbleLeague;
using GUI;
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
    [SerializeField] private AudioSource _audioSource;
    public float minPitch = 0.9f;
    public float maxPitch = 1.1f;
    
    public List<AudioClip> countdownSounds;
    public List<AudioClip> hoopSounds;
    public List<AudioClip> timeUpSounds;
    public List<AudioClip> gameStartSounds;
    
    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.playOnAwake = false;
        _audioSource.loop = false;
        _audioSource.reverbZoneMix = 0;
    }

    void PlayCountDownSound()
    {
        if (countdownSounds.Count <= 0)
        {
            Debug.LogWarning("No countdown sounds found", this);
            return;
        }

        _audioSource.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
        _audioSource.PlayOneShot(countdownSounds[UnityEngine.Random.Range(0, countdownSounds.Count)]);
    }
    
    void PlayHoopSound()
    {
        if (hoopSounds.Count <= 0)
        {
            Debug.LogWarning("No hoop sounds found", this);
            return;
        }

        _audioSource.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
        _audioSource.PlayOneShot(hoopSounds[UnityEngine.Random.Range(0, hoopSounds.Count)]);
    }
    
    void PlayTimeUpSound()
    {
        if (timeUpSounds.Count <= 0)
        {
            Debug.LogWarning("No timeUpSounds found", this);
            return;
        }

        _audioSource.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
        _audioSource.PlayOneShot(timeUpSounds[UnityEngine.Random.Range(0, timeUpSounds.Count)]);
    }
    
    void PlayGameStartSound()
    {
        if (gameStartSounds.Count <= 0)
        {
            Debug.LogWarning("No gameStartSounds found", this);
            return;
        }

        _audioSource.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
        _audioSource.PlayOneShot(gameStartSounds[UnityEngine.Random.Range(0, gameStartSounds.Count)]);
    }
    
    public int GetPlayerNumber(Player player)
    {
        return _players.IndexOf(player);
    }
    

    private readonly List<Player> _players = new();
    private float m_timer;
    private float? m_player1Score;
    private bool started;
    private GameObject m_mainMenuRef;
    public float startCountDownSoundAt = 6.5f;

    private void Awake()
    {
        m_mainMenuRef = Instantiate(m_mainMenuPrefab);
    }

    public void StartGame()
    {
        Debug.Log("Start Game!");
        m_timer = m_gameDuration;
        MakeDefender(_players[0]);
        MakeAttacker(_players[1]);
        FindFirstObjectByType<Hoop>().Reset();
        Time.timeScale = 1f;
        started = true;
        PlayGameStartSound();
    }

    public void EndGame()
    {
        if (m_timer <= 0)
        {
            m_timer = 0;
            PlayTimeUpSound();
        }
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
        if (m_timer <= 0)
        {
            m_timer = 0;
            PlayTimeUpSound();
        }
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
        if(m_timer >= startCountDownSoundAt && m_timer - Time.deltaTime < startCountDownSoundAt)
        {
            PlayCountDownSound();
        }
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
        PlayHoopSound();
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
        if(m_mainMenuRef != null) Destroy(m_mainMenuRef);
        _players.Add(player);
        var cam = player.GetComponentInChildren<Camera>();
        var hud = Instantiate(m_playerHudPrefab).GetComponent<HUD>();
        var canvas = hud.GetComponentInChildren<Canvas>();
        canvas.worldCamera = cam;
        TimerUpdated += hud.UpdateTimer;
        player.hudRef = hud;

        var pa = player.GetComponentInChildren<PlayerAttributes>();
        pa.Initialize();
        pa.EnergyUpdated += hud.UpdateEnergy;
        pa.BroadcastEnergy();

        var targetIndicator = hud.GetComponentInChildren<TargetIndicator>();
        player.TargetIndicator = targetIndicator;
        targetIndicator.cam = cam;
        targetIndicator.target = FindFirstObjectByType<Hoop>().transform;

        if (_players.Count == 2) StartGame();
    }

    public void MakeAttacker(Player player)
    {
        player.gameObject.layer = LayerMask.NameToLayer("Attacker");
        player.transform.position = attackerSpawnPoint.position;
        player.GetComponentInChildren<BallMovement>().transform.rotation = attackerSpawnPoint.rotation;
        player.TargetIndicator.target = FindFirstObjectByType<Hoop>().transform;
        player.Reset();
        player.hudRef.SetRole(Role.Attacker);
    }

    public void MakeDefender(Player player)
    {
        player.gameObject.layer = LayerMask.NameToLayer("Defender");
        player.transform.position = defenderSpawnPoint.position;
        player.GetComponentInChildren<BallMovement>().transform.rotation = defenderSpawnPoint.rotation;
        player.TargetIndicator.target = _players.FirstOrDefault(it => it != player)?.transform;
        player.Reset();
        player.hudRef.SetRole(Role.Defender);
    }

    public void UnregisterPlayer(Player player)
    {
        _players.Remove(player);
    }
}