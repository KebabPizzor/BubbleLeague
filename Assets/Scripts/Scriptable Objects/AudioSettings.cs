using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Scriptable_Objects
{
  [CreateAssetMenu(fileName = "AudioSettings", menuName = "Audio Settings")]
  public class AudioSettings : ScriptableObject
  {
    [Tooltip("In Percentage"), Range(0.0001f, 1), SerializeField]
    float masterVolume = 0.5f, musicVolume = 1, sfxVolume = 1;
  
    public float MasterVolume{  get { return masterVolume; }  set { masterVolume = value; SetMixerVolume("masterVolume",masterVolume); } }
    public float MusicVolume{  get { return musicVolume; }  set { musicVolume = value; SetMixerVolume("musicVolume",musicVolume); } }
    public float SfxVolume{  get { return sfxVolume; }  set { sfxVolume = value; SetMixerVolume("sfxVolume",sfxVolume); } }
  

    public float lowPassCutoffFrequency = 5000f;
    public float maxLowPassCutoffFrequency = 22000f;
  
    public AudioMixer mainMixer;
  
    public AudioClip startGameButtonAudio;
    public AudioClip clickedButtonAudio;
    public AudioClip hoverButtonAudio;
    public AudioClip winAudio;
  
    public AudioClip introMenuMusic;
    public AudioClip menuMusic;
  
    [Tooltip("Part of the music, but only meant to be played when starting the game")]
    public AudioClip startGameIntroAudio;
    public List<AudioClip> gameMusic;
    public AudioClip creditsMusic;


    private void Awake()
    {
      SetMixerVolume("masterVolume",masterVolume);
      SetMixerVolume("musicVolume",musicVolume);
      SetMixerVolume("sfxVolume",sfxVolume);
    }

    private void OnValidate()
    {
      SetMixerVolume("masterVolume",masterVolume);
      SetMixerVolume("musicVolume",musicVolume);
      SetMixerVolume("sfxVolume",sfxVolume);
    }


    public void ActivateLowPassFilter()
    {
      //Set the low pass cutoff frequency to 5000 Hz
      mainMixer.SetFloat("masterCutoffFrequency", lowPassCutoffFrequency);
    }
    
    public void DeactivateLowPassFilter()
    {
      mainMixer.SetFloat("masterCutoffFrequency", maxLowPassCutoffFrequency);
    }
  
    public int GetMasterDisplayValue()
    {
      return Mathf.RoundToInt(masterVolume*100);
    }
    public int GetMusicDisplayValue()
    {
      return Mathf.RoundToInt(musicVolume*100);
    }
    public int GetSfxDisplayValue()
    {
      return Mathf.RoundToInt(sfxVolume*100);
    }
  
    private void SetMixerVolume(string floatName, float volume)
    {
      mainMixer.SetFloat(floatName, Mathf.Log10(volume) * 20);
    }
  }
}
