using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;


[System.Serializable]
public class DialogueLine
{
    public string speakerName;   // "Player" ya "Salim Bhai" etc.
    [TextArea(1, 4)]
    public string text;          // Jo bolna hai
}

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager instance;

    [Header("Dialogue UI")]
    public GameObject dialoguePanel;
    public TextMeshProUGUI speakerNameText;
    public TextMeshProUGUI dialogueText;

    [Header("Press E UI")]
    public GameObject pressEPanel;
    public TextMeshProUGUI pressEText;

    [Header("Settings")]
    public float typingSpeed = 0.04f;

    // Player ka naam (Inspector se set karo)
    [Header("Player Name")]
    public string playerName = "Rupesh";

    private DialogueLine[] currentLines;
    private int currentLineIndex = 0;
    private bool isTyping = false;
    private bool isDialogueActive = false;
    private System.Action onDialogueComplete;

    void Awake()
    {
        if (instance == null) instance = this;
        else Destroy(gameObject);
    }

    void Start()
    {
        dialoguePanel.SetActive(false);
        pressEPanel.SetActive(false);
        dialogueText.alignment = TextAlignmentOptions.Left;
    }

    public void ShowPressE(string npcName)
    {
        if (!isDialogueActive)
        {
            pressEPanel.SetActive(true);
            pressEText.text = $"[E]  Talk to {npcName}";
        }
    }

    public void HidePressE()
    {
        pressEPanel.SetActive(false);
    }

    // Ab DialogueLine[] array leta hai
    public void StartDialogue(DialogueLine[] lines, System.Action onComplete = null)
    {
        if (isDialogueActive) return;

        isDialogueActive = true;
        onDialogueComplete = onComplete;
        currentLines = lines;
        currentLineIndex = 0;

        dialoguePanel.SetActive(true);
        HidePressE();

        ShowCurrentLine();
    }

    void Update()
    {
        if (isDialogueActive && Input.GetKeyDown(KeyCode.E))
        {
            if (isTyping)
            {
                // Puri line ek baar mein dikha do (skip typing)
                StopAllCoroutines();
                dialogueText.text = currentLines[currentLineIndex].text;
                isTyping = false;
            }
            else
            {
                // Agli line pe jao
                currentLineIndex++;
                if (currentLineIndex < currentLines.Length)
                    ShowCurrentLine();
                else
                    EndDialogue();
            }
        }
    }

    void ShowCurrentLine()
    {
        DialogueLine line = currentLines[currentLineIndex];

        // Speaker ka naam dikhao
        speakerNameText.text = line.speakerName;

        StopAllCoroutines();
        StartCoroutine(TypeLine(line.text));
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        dialogueText.text = "";
        foreach (char letter in line)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
        isTyping = false;
    }

    void EndDialogue()
    {
        isDialogueActive = false;
        dialoguePanel.SetActive(false);
        onDialogueComplete?.Invoke();
    }

    public bool IsDialogueActive() => isDialogueActive;
}
