using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    private readonly List<Player> _players = new();
    public void StartGame()
    {
        Debug.Log("Ready to Start Game!");
    }

    public void RegisterPlayer(Player player)
    {
        _players.Add(player);
    }

    public void UnregisterPlayer(Player player)
    {
        _players.Remove(player);
    }
}
