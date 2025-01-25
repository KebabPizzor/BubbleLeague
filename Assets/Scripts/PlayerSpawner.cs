using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerSpawner : MonoBehaviour
{
    public List<Player> players;
    private readonly Color[] _colors = new[] { Color.blue, Color.red };

    [UsedImplicitly]
    public void OnPlayerJoined(PlayerInput playerInput)
    {
        Debug.Log("Player joined!");
        int playerCount = players.Count;
        players.Add(playerInput.GetComponent<Player>());
        players[playerCount].Color = _colors[playerCount];
        if (playerCount == 1)
        {
            FindFirstObjectByType<GameController>().StartGame();
        }
        FindFirstObjectByType<GameController>().RegisterPlayer(players[playerCount]);
        
        if (playerInput.currentControlScheme == "KeyboardMouse")
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }

    [UsedImplicitly]
    public void OnPlayerLeft(PlayerInput playerInput)
    {
        var player = players.FirstOrDefault(it => it.GetComponent<PlayerInput>() == playerInput);
        players.Remove(player);
        FindFirstObjectByType<GameController>()?.UnregisterPlayer(player);
    }
}
