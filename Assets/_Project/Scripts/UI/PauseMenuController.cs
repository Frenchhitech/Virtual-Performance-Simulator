using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenu;
    public InputActionProperty showButton;
    public Transform head;
    public float spawnDistance = 0.8f;
    public float lowerDistance = 0.13f;

    void Start()
    {
        HideMenu();
    }

    void Update()
    {
        if (showButton.action.WasPressedThisFrame())
        {
            ToggleMenu();
        }
    }

    private void ToggleMenu()
    {
        if (pauseMenu.activeSelf)
        {
            HideMenu();
        }
        else
        {
            ShowMenu();
        }
    }

    private void HideMenu()
    {
        pauseMenu.SetActive(false);
    }

    private void ShowMenu()
    {
        PositionMenu();
        pauseMenu.SetActive(true);
    }

    private void PositionMenu()
    {
        Vector3 forward = head.forward;
        forward.y = 0f;
        forward.Normalize();

        Vector3 spawnPosition = head.position + forward * spawnDistance + new Vector3(0f, -lowerDistance, 0f);

        pauseMenu.transform.position = spawnPosition;
        pauseMenu.transform.rotation = Quaternion.LookRotation(forward, Vector3.up);
    }

    public void OnRestartPressed()
    {
        HideMenu();
        SceneManager.LoadScene(SessionManager.Instance.roomType.ToString());
    }

    public void OnQuitPressed()
    {
        HideMenu();
        SceneManager.LoadScene("Lobby");
    }

    public void OnResumePressed()
    {
        HideMenu();
    }
}
