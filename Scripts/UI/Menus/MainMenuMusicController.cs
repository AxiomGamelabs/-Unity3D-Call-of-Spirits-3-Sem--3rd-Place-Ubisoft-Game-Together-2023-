using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuMusicController : MonoBehaviour
{
    public static MainMenuMusicController instance;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (SceneManager.GetActiveScene() == SceneManager.GetSceneByName("ChrisScene"))
        {
            Destroy(gameObject);
        }
    }


}
