using UnityEngine;

public class SessionManager : MonoBehaviour
{
    
    public static SessionManager Instance {get; private set; }

    public RoomType roomType;
    public DisruptionLevel disruptionLevel;
    public bool timerEnabled = false;
    public int timerLength = 1;

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
