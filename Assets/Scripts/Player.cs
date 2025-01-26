using System;
using System.Collections;
using System.Collections.Generic;
using GUI;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(AudioSource))]
public class Player : MonoBehaviour
{

    public float minPitch = 0.9f;
    public float maxPitch = 1.1f;
    public TargetIndicator TargetIndicator;

    public List<AudioClip> playerHitSounds;
    public List<AudioClip> sinkHitSounds;
    public List<AudioClip> pickUpSounds;

    private AudioSource _audioSource;
    public Color Color { get; set; }

    void Start()
    {

        _audioSource = GetComponent<AudioSource>();
        _audioSource.playOnAwake = false;
        _audioSource.loop = false;
        _audioSource.reverbZoneMix = 0;
    }

    public void OnCollisionEnter2D(Collision2D other)
    {
        var player = other.gameObject.GetComponentInParent<Player>() ?? other.gameObject.GetComponentInChildren<Player>();
        if (player != null)
        {
            Debug.Log($"Collided with player: {other.gameObject}");
            if(playerHitSounds.Count <= 0)
            {
                Debug.LogWarning("No player hit sounds found",this);
                return;
            }
            _audioSource.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
            _audioSource.PlayOneShot(playerHitSounds[UnityEngine.Random.Range(0, playerHitSounds.Count)]);
        }
        else if(other.transform.CompareTag("Sink"))
        {
            if(sinkHitSounds.Count <= 0)
            {
                Debug.LogWarning("No wall hit sounds found",this);
                return;
            }
            _audioSource.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
            _audioSource.PlayOneShot(sinkHitSounds[UnityEngine.Random.Range(0, sinkHitSounds.Count)]);
        }
        else if(other.transform.CompareTag("PickUp"))
        {
            if(pickUpSounds.Count <= 0)
            {
                Debug.LogWarning("No blob hit sounds found",this);
                return;
            }
            _audioSource.pitch = UnityEngine.Random.Range(minPitch, maxPitch);
            _audioSource.PlayOneShot(pickUpSounds[UnityEngine.Random.Range(0, pickUpSounds.Count)]);
        }
    }

    public void Reset()
    {
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
    }
}
