using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class LobbyMenuController : MonoBehaviour
{
    
    public TMP_Dropdown roomDropdown;
    public TMP_Dropdown disruptionDropdown;
    public Slider timerSlider;
    public TMP_Text timerValueText;

    private void Start()
    {
        OnTimerSliderChanged(timerSlider.value);
    }

    public void OnStartPressed()
    {
        if(SessionManager.Instance == null)
        {
            Debug.LogError("Session Manager not found");
            return;
        }

        var session = SessionManager.Instance;

        session.roomType = (RoomType)roomDropdown.value;
        session.disruptionLevel = (DisruptionLevel)disruptionDropdown.value;
        session.timerLength = Mathf.RoundToInt(timerSlider.value);

        LoadScene();
    }

    private void LoadScene()
    {
        //Debug.Log("Loading Room: " + SessionManager.Instance.roomType.ToString());
        SceneManager.LoadScene(SessionManager.Instance.roomType.ToString());
    }

    public void OnTimerSliderChanged(float value)
    {
        int minutes = Mathf.RoundToInt(value);
        //Debug.Log("Setting Timer Value Text To: " + minutes);

        if(timerValueText == null)
        {
            Debug.LogError("timerValueText is not assigned in LobbyMenuController!");
            return;
        }

        timerValueText.text = $"{minutes} min";
    }
}
