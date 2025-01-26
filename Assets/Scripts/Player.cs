using System;
using System.Collections;
using System.Collections.Generic;
using GUI;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class Player : MonoBehaviour
{
    [SerializeField] private GameObject duck;
    [SerializeField] private GameObject frog;
    public float minPitch = 0.9f;
    public float maxPitch = 1.1f;
    public TargetIndicator TargetIndicator;

    public List<AudioClip> playerSoftHitSounds;
    public List<AudioClip> playerHeavyHitSounds;
    public List<AudioClip> sinkHitSounds;
    public List<AudioClip> pickUpSounds;
    public HUD hudRef;

    private AudioSource _audioSource;
    public Color Color { get; set; }

    public void SetCharacter(int character)
    {
        duck.SetActive(character == 0);
        frog.SetActive(character == 1);
    }

    void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _audioSource.playOnAwake = false;
        _audioSource.loop = false;
        _audioSource.reverbZoneMix = 0;
    }

    public void OnCollisionEnter(Collision other)
    {
        var player = other.gameObject.GetComponentInParent<Player>() ??
                     other.gameObject.GetComponentInChildren<Player>();
        if (player != null && FindFirstObjectByType<GameController>().GetPlayerNumber(this) < FindFirstObjectByType<GameController>().GetPlayerNumber(player))
        {
            Debug.Log($"Impact Magnitude: {other.impulse.magnitude}");
            if (other.impulse.magnitude > 200f)
            {
                if (playerHeavyHitSounds.Count <= 0)
                {
                    Debug.LogWarning("No player hit sounds found", this);
                    return;
                }

                _audioSource.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
                _audioSource.PlayOneShot(playerHeavyHitSounds[UnityEngine.Random.Range(0, playerHeavyHitSounds.Count)]);
            }
            else
            {
                if (playerSoftHitSounds.Count <= 0)
                {
                    Debug.LogWarning("No player hit sounds found", this);
                    return;
                }

                _audioSource.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
                _audioSource.PlayOneShot(playerSoftHitSounds[UnityEngine.Random.Range(0, playerSoftHitSounds.Count)]);
            }
            
        }
        else if (other.transform.CompareTag("Sink"))
        {
            if (sinkHitSounds.Count <= 0)
            {
                Debug.LogWarning("No wall hit sounds found", this);
                return;
            }

            _audioSource.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
            _audioSource.PlayOneShot(sinkHitSounds[UnityEngine.Random.Range(0, sinkHitSounds.Count)]);
        }
    }

    public void PlayPickUpSound()
    {
        if (pickUpSounds.Count <= 0)
        {
            Debug.LogWarning("No blob hit sounds found", this);
            return;
        }

        _audioSource.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
        _audioSource.PlayOneShot(pickUpSounds[UnityEngine.Random.Range(0, pickUpSounds.Count)]);
    }

    public void Reset()
    {
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
    }
}