using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class Mission2Standoff : MonoBehaviour
{
    public GameObject choicePanel;
    public GameObject dialogueBox;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI speakerNameText;
    public TextMeshProUGUI pressEText;
    public GameObject pressEPanel;

    public Transform player;
    public Transform mainPoliceOfficer;
    public float talkDistance = 7f;
    public float alertDistance = 12f;
    public Mission2PoliceManager[] allCops;

    public GameObject playerGun;

    private string[] standoffDialogues = {
        "POLICE OFFICER: This is a restricted area. Stop right there!",
        "RUPESH: I'm just passing through, officer. Is there a problem?",
        "POLICE OFFICER: We've had reports of suspicious activity. Step out and let us search you."
    };

    private int currentDialogueIndex = 0;
    private bool isAlerted = false;
    private bool sequenceStarted = false;
    private bool choicePhase = false;
    private bool isMissionFinished = false;

    void Start()
    {
        if (playerGun != null) playerGun.SetActive(false);
        if (pressEPanel != null) pressEPanel.SetActive(false);
        foreach (Mission2PoliceManager cop in allCops) if (cop != null) cop.StartTalking();
    }

    void Update()
    {
        if (isMissionFinished) return;

        if (choicePhase)
        {
            if (pressEPanel != null && pressEPanel.activeSelf) pressEPanel.SetActive(false);
            return;
        }

        float dist = Vector3.Distance(player.position, mainPoliceOfficer.position);

        if (dist < alertDistance && !isAlerted)
        {
            isAlerted = true;
            foreach (Mission2PoliceManager cop in allCops) if (cop != null) cop.SetIdle();
        }

        if (dist < talkDistance && !sequenceStarted)
        {
            if (pressEPanel != null)
            {
                pressEPanel.SetActive(true);
                if (pressEText != null) pressEText.text = "Press E to Talk to Police";
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                StartDialogueSequence();
            }
        }
        else
        {
            if (!sequenceStarted && pressEPanel != null && pressEPanel.activeSelf)
            {
                pressEPanel.SetActive(false);
            }
        }

        if (sequenceStarted && !choicePhase)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                NextDialogue();
            }
        }
    }

    void StartDialogueSequence()
    {
        sequenceStarted = true;
        if (pressEPanel != null) pressEPanel.SetActive(false);
        dialogueBox.SetActive(true);
        player.GetComponent<PlayerMovement>().canMove = false;
        ShowCurrentDialogue();
    }

    void ShowCurrentDialogue()
    {
        string fullText = standoffDialogues[currentDialogueIndex];
        string[] split = fullText.Split(':');
        
        if (speakerNameText != null) speakerNameText.text = split[0].Trim();
        if (dialogueText != null) dialogueText.text = split[1].Trim();

        if (split[0].Contains("POLICE"))
        {
            foreach (Mission2PoliceManager cop in allCops) if (cop != null) cop.StartTalking();
        }
        else
        {
            foreach (Mission2PoliceManager cop in allCops) if (cop != null) cop.SetIdle();
        }
    }

    void NextDialogue()
    {
        currentDialogueIndex++;
        if (currentDialogueIndex < standoffDialogues.Length)
        {
            ShowCurrentDialogue();
        }
        else
        {
            EnterChoicePhase();
        }
    }

    void EnterChoicePhase()
    {
        choicePhase = true;
        choicePanel.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void OnSurrender()
    {
        isMissionFinished = true;
        choicePanel.SetActive(false);
        dialogueText.text = "POLICE: You're under arrest! Don't move.";
        Invoke("RestartMission", 2f);
    }

    public void OnShoot()
    {
        isMissionFinished = true;
        choicePhase = false;
        choicePanel.SetActive(false);
        dialogueBox.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        player.GetComponent<PlayerMovement>().canMove = true;
        
        if (playerGun != null) playerGun.SetActive(true);
        
        Animator playerAnim = player.GetComponent<Animator>();
        if (playerAnim != null) playerAnim.SetBool("isArmed", true);

        PlayerShooting ps = player.GetComponent<PlayerShooting>();
        if (ps != null) ps.enabled = true;

        foreach (Mission2PoliceManager cop in allCops)
        {
            EnemyController ai = cop.GetComponent<EnemyController>();
            if (ai != null) ai.enabled = true;
        }
    }

    void RestartMission()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
