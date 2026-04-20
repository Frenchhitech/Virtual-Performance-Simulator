using UnityEngine;

public class RoomSceneController : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        var session = SessionManager.Instance;

        Debug.Log("Room: " + session.roomType);
        Debug.Log("Audience Disruption: " + session.disruptionLevel);
        Debug.Log("Timer Enabled: " + session.timerEnabled);
        Debug.Log("Timer Length: " + session.timerLength);
    }
}
