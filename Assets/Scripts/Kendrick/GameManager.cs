using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public enum GameState { unpaused, isPaused };
    public static GameManager instance;
    public string currentLevel;
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
        
    }
    public void RestartLevel()
    {
        SceneManager.LoadScene(currentLevel);
    }
}
