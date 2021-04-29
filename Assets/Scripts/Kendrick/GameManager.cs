using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public enum GameState { unpaused, isPaused };
    public static GameManager instance;
    public string currentLevel;
    public bool paused;

    public Animator anim;
    private void Awake()
    {
        instance = this;
        //DontDestroyOnLoad(this.gameObject);
    }
    void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //currentLevel = scene.name;
        currentLevel = SceneManager.GetSceneAt(0).name;
        Debug.Log("OnSceneLoaded: " + scene.name);
        Debug.Log(mode);
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePauseMenu();
        }
        FreezeGameOnPause();

    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(currentLevel);
    }
    public void TogglePauseMenu()
    {
        anim.SetTrigger("Pause");
    }
    public void FreezeGameOnPause()
    {
        if (paused)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
    public void ChangeScene(string SceneToLoad)
    {
        SceneManager.LoadScene(SceneToLoad);

    }
}
