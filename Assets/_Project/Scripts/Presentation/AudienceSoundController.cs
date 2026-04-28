using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudienceSoundController : MonoBehaviour
{
    [Header("Sound Clips")]
    public AudioClip[] coughClips;
    public AudioClip[] phoneClips;
    public AudioClip[] mumbleClips;

    [Header("Interval Ranges (seconds) — higher disruption = shorter intervals")]
    public Vector2 intervalLow    = new Vector2(15f, 30f);
    public Vector2 intervalMedium = new Vector2(6f,  14f);
    public Vector2 intervalHigh   = new Vector2(2f,   6f);

    [Header("Chance each category plays per interval (0 to 1)")]
    public float coughChance  = 0.4f;
    public float phoneChance  = 0.3f;
    public float mumbleChance = 0.5f;

    [Header("Active Level (change this during Play to test)")]
    public DisruptionLevel currentLevel = DisruptionLevel.Low;

    private AudioSource audioSource;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (SessionManager.Instance != null)
            currentLevel = SessionManager.Instance.disruptionLevel;

        StartCoroutine(SoundLoop());
    }

    public void SetDisruptionLevel(DisruptionLevel level)
    {
        currentLevel = level;
    }

    private IEnumerator SoundLoop()
    {
        yield return new WaitForSeconds(Random.Range(4f, 8f));
        while (true)
        {
            if (Random.value < coughChance)  PlayRandomFrom(coughClips);
            if (Random.value < phoneChance)  PlayRandomFrom(phoneClips);
            if (Random.value < mumbleChance) PlayRandomFrom(mumbleClips);
            yield return new WaitForSeconds(RandomInterval());
        }
    }

    private float RandomInterval()
    {
        Vector2 r = currentLevel switch
        {
            DisruptionLevel.Low    => intervalLow,
            DisruptionLevel.Medium => intervalMedium,
            DisruptionLevel.High   => intervalHigh,
            _                      => intervalLow
        };
        return Random.Range(r.x, r.y);
    }

    private void PlayRandomFrom(AudioClip[] clips)
    {
        var pool = new List<AudioClip>();
        AddNonNull(pool, clips);
        if (pool.Count == 0) return;
        audioSource.PlayOneShot(pool[Random.Range(0, pool.Count)]);
    }

    private static void AddNonNull(List<AudioClip> pool, AudioClip[] clips)
    {
        if (clips == null) return;
        foreach (var c in clips)
            if (c != null) pool.Add(c);
    }
}
