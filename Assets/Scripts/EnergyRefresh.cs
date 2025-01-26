using System;
using UnityEngine;

public class EnergyRefresh : MonoBehaviour
{
    public float respawnTimer = 9f;
    private float _currentRespawnTimer;
    private void OnTriggerEnter(Collider other)
    {
        var comp = other.GetComponentInChildren<BallMovement>();
        if (comp != null)
        {
            comp.OnPowerUpCollected();
            GetComponent<Renderer>().enabled = false;
            GetComponent<Collider>().enabled = false;
            _currentRespawnTimer = respawnTimer;
        }
    }

    void Update()
    {
        if (_currentRespawnTimer > 0f)
        {
            _currentRespawnTimer -= Time.deltaTime;
            if (_currentRespawnTimer <= 0f)
            {
                GetComponent<Renderer>().enabled = true;
                GetComponent<Collider>().enabled = true;
            }
        }
    }
}
