using System.Collections;
using TMPro;
using UnityEngine;

public class PresentationTimer : MonoBehaviour
{
    [Tooltip("Assign the TextMeshPro component on the clock face child object.")]
    public TextMeshPro timerText;

    [Tooltip("Fallback duration in minutes if SessionManager is not present.")]
    public float totalMinutes = 5f;

    private float remaining;
    private bool running;

    void Start()
    {
        if (SessionManager.Instance != null)
            totalMinutes = SessionManager.Instance.timerLength;

        ResetTimer();
        StartTimer();
    }

    public void StartTimer()
    {
        running = true;
        StartCoroutine(Countdown());
    }

    public void StopTimer()
    {
        running = false;
        StopAllCoroutines();
    }

    public void ResetTimer()
    {
        StopTimer();
        remaining = totalMinutes * 60f;
        UpdateDisplay();
    }

    private IEnumerator Countdown()
    {
        while (remaining > 0f && running)
        {
            yield return null;
            remaining -= Time.deltaTime;
            UpdateDisplay();
        }

        remaining = 0f;
        UpdateDisplay();
    }

    private void UpdateDisplay()
    {
        if (timerText == null) return;
        int m = Mathf.FloorToInt(remaining / 60f);
        int s = Mathf.FloorToInt(remaining % 60f);
        timerText.text = $"{m:00}:{s:00}";
    }
}
