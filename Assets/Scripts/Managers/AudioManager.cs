using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [Header("Global Settings")]
    [Tooltip("Master volume multiplier for SFX (0 to 1).")]
    [Range(0f, 1f)]
    public float sfxVolume = 1f;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Play a one-shot SFX at the given world position.
    /// A temporary GameObject with AudioSource is created, plays the clip, then is destroyed.
    /// </summary>
    /// <param name="clip">AudioClip to play.</param>
    /// <param name="position">World position for 3D spatial audio. If you want 2D (no spatialization), pass Camera.main.transform.position or use an overload.</param>
    /// <param name="volume">Volume multiplier (0 to 1). Final volume = sfxVolume * volume.</param>
    /// <param name="pitchMin">Minimum pitch for randomization. If you don’t want random pitch, set both pitchMin and pitchMax to 1.</param>
    /// <param name="pitchMax">Maximum pitch for randomization.</param>
    /// <param name="spatialBlend">
    ///     0 = fully 2D (ignores position attenuation), 
    ///     1 = fully 3D (attenuates with distance). 
    ///     You can choose intermediate values if desired.
    /// </param>
    public void PlaySFX(
        AudioClip clip,
        Vector3 position,
        float volume = 1f,
        float pitchMin = 1f,
        float pitchMax = 1f,
        float spatialBlend = 1f
    )
    {
        if (clip == null) return;

        // Create a temporary GameObject to host the AudioSource
        GameObject go = new GameObject($"SFX_{clip.name}");
        go.transform.position = position;

        AudioSource src = go.AddComponent<AudioSource>();
        src.clip = clip;
        src.volume = Mathf.Clamp01(sfxVolume * volume);
        src.pitch = Random.Range(pitchMin, pitchMax);
        src.spatialBlend = Mathf.Clamp01(spatialBlend);
        // Optional: configure rolloff / minDistance / maxDistance here if desired:
        // src.rolloffMode = AudioRolloffMode.Linear;
        // src.minDistance = 1f;
        // src.maxDistance = 50f;

        src.Play();
        // Destroy after clip finishes playing (accounting for pitch)
        float adjustedLength = clip.length / Mathf.Abs(src.pitch);
        Destroy(go, adjustedLength + 0.1f);
    }

    /// <summary>
    /// Overload: play a 2D (non-spatial) SFX globally.
    /// This can be useful for UI sounds or if you want sound not tied to world position.
    /// Internally, we'll place at AudioListener position and spatialBlend=0.
    /// </summary>
    public void PlaySFX2D(
        AudioClip clip,
        float volume = 1f,
        float pitchMin = 1f,
        float pitchMax = 1f
    )
    {
        if (clip == null) return;
        Vector3 listenerPos = Camera.main != null
            ? Camera.main.transform.position
            : Vector3.zero;
        PlaySFX(clip, listenerPos, volume, pitchMin, pitchMax, spatialBlend: 0f);
    }
}
