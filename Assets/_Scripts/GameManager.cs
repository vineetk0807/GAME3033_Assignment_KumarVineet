using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour //Singleton<GameManager>
{

    public bool cursorActive = true;

    // Lazy singleton for GM temporarily
    private static GameManager Instance;

    [Header("Objectives")]
    public int objectiveNumber = 0;

    [Header("Prompts")] 
    public TextMeshProUGUI TMP_Prompts;

    public Dictionary<string, string> promptsDictionary;
    public List<string> promptKey;
    public List<string> promptValue;
    private bool isPromptOn = false;
    private IEnumerator promptCoroutine;
    public float promptDelay = 4f;
    private string clear = "clear";

    [Header("C4 Equipped")] 
    public bool isC4Equipped = false;
    public bool isReadyToPlant = false;
    public EquipmentScriptable C4Equipment;

    public static GameManager GetInstance()
    {
        return Instance;
    }

    public void Awake()
    {
        Instance = this;
        
    }

    private void Start()
    {
        ObjectiveManager.GetInstance().TriggerObjective(objectiveNumber);

        // Prompts
        // Load dictionary
        promptsDictionary = new Dictionary<string, string>();
        for (int i = 0; i < promptKey.Count; i++)
        {
            promptsDictionary.Add(promptKey[i],promptValue[i]);
        }

        promptCoroutine = ShowPromptCoroutine(clear);
    }

    /// <summary>
    /// Enables cursor with the event
    /// makes it visible and locked, or otherwise opposite of that
    /// </summary>
    /// <param name="enable"></param>
    void EnableCursor(bool enable)
    {
        if (enable)
        {
            cursorActive = true;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            Debug.Log("Visible");
        }
        else
        {
            cursorActive = false;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            Debug.Log("Not Visible");
        }
    }


    // On Enable / Disable events listener 
    public void OnEnable()
    {
        AppEvents.MouseCursorEnabled += EnableCursor;
    }


    public void OnDisable()
    {
        AppEvents.MouseCursorEnabled -= EnableCursor;
    }


    /// <summary>
    /// Function to give prompts to user
    /// </summary>
    public void PromptUser(string key)
    {
        if (!isPromptOn)
        {
            isPromptOn = true;
            promptCoroutine = ShowPromptCoroutine(key);
            StartCoroutine(promptCoroutine);
        }
        else
        {
            isPromptOn = true;
            StopCoroutine(promptCoroutine);
            promptCoroutine = ShowPromptCoroutine(key);
            StartCoroutine(promptCoroutine);
        }
    }

    /// <summary>
    /// Coroutine of prompt
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    IEnumerator ShowPromptCoroutine(string key)
    {
        // display prompt
        TMP_Prompts.text = promptsDictionary[key];

        // delay
        yield return new WaitForSeconds(promptDelay);

        // clear
        TMP_Prompts.text = promptsDictionary[clear];
        StopCoroutine(promptCoroutine);
        isPromptOn = false;
    }
}
