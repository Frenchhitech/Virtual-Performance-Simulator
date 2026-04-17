using UnityEngine;
using TMPro;
using System;
using UnityEngine.SceneManagement;

public class LobbyMenuController : MonoBehaviour
{
    
    public TMP_Dropdown sceneSelect;

    public void StartSession()
    {
        int sceneVal = sceneSelect.value;
        switch (sceneVal)
        {
            case 0:
                SceneManager.LoadScene("Classroom_Small");
                break;
            case 1:
                SceneManager.LoadScene("LectureHall_Large");
                break;
            case 2:
                SceneManager.LoadScene("PitchRoom_Small");
                break;
        }
    }
}
