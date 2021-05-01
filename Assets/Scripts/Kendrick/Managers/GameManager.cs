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
    public bool inDialogue;
    public bool inKnightGame;
    [Header("Cameras")]
    public Camera knightCam;
    public Camera OsCam;
    public GameObject knightGameHolder;
    public GameObject OSHolder;
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
        if (inKnightGame)
        {
            knightCam.gameObject.SetActive(true);
            OsCam.gameObject.SetActive(false);
            UpdateDialogue();
            FreezeGameOnPause();
        }
        else
        {
            knightCam.gameObject.SetActive(false);
            OsCam.gameObject.SetActive(true);
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (Knight.instance != null && !Knight.instance.stats.dead && !Knight.instance.stats.UIanim.GetBool("Dead"))
            TogglePauseMenu();
        }
        //DEBUG
        if (Input.GetKeyDown(KeyCode.G))
        {
            StartCoroutine(ToggleKnightGame());
        }

    }
    void UpdateDialogue()
    {
        if (inDialogue && !paused)
        {
            DialogueManager.instance.animator.SetBool("DialogueOpen", true);
            Knight.instance.disableMovement = true;
            if (Input.GetKeyDown(KeyCode.Z) || Input.GetKeyDown(KeyCode.X))
            {
                if (DialogueManager.instance.CheckIfDoneTyping())
                {
                    DialogueManager.instance.DisplayNextSentence();
                }
                else { DialogueManager.instance.SkipTyping(); }
            }
        }
        else
        {
            DialogueManager.instance.animator.SetBool("DialogueOpen", false);
            if (Knight.instance.disableMovement)
            {
                StartCoroutine(WaitTillDisable());
            }
        }
    }
    IEnumerator ToggleKnightGame()
    {
        yield return new WaitForSeconds(0.5f);
        if (inKnightGame)
        {
            inKnightGame = false;
            knightGameHolder.SetActive(false);
            OSHolder.SetActive(true);
            CooldownManager.instance.enabled = false;
            CombatManager.instance.enabled = false;
            Knight.instance.gameObject.SetActive(false);

        }
        else if (!inKnightGame)
        {
            inKnightGame = true;
            knightGameHolder.SetActive(true);
            OSHolder.SetActive(false);
            CooldownManager.instance.enabled = true;
            CombatManager.instance.enabled = true;
            Knight.instance.gameObject.SetActive(true);
        }
    }
    IEnumerator WaitTillDisable()
    {
        yield return new WaitForSeconds(0.2f);
        Knight.instance.disableMovement = false;
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
