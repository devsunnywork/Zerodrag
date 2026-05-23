using UnityEngine;
using TMPro;

[System.Serializable]
public class DialogueM2
{
    public string name;
    [TextArea(3, 10)]
    public string text;
}

public class mission2 : MonoBehaviour
{
    [Header("Transforms")]
    public Transform player;
    public Transform reciver;
    public Transform dropOff;

    [Header("UI Elements")]
    public TextMeshProUGUI PressE;
    public GameObject PressEpannel;
    public GameObject dialogBox;
    public TextMeshProUGUI conversation;
    public TextMeshProUGUI speakerNameText;
    public TextMeshProUGUI parcelText;
    public TextMeshProUGUI missionObjectiveText;
    public TextMeshProUGUI distanceCalculatorText;

    [Header("GameObjects")]
    public GameObject parcel;
    
    [Header("Audio")]
    public AudioSource audioSource; 
    public AudioClip successSound;
    public AudioClip failureSound;

    [Header("References")]
    public NPCAnimationManager npcAnimManager;

    [Header("Mission Dialogues")]
    public DialogueM2[] dialogues;
    
    private int dialogueIndex = 0;
    private bool istalking = false;
    private bool isMissionStarted = false;
    private float totaldistance;

    void Start()
    {
        // Initial UI cleanup with null checks
        if (distanceCalculatorText != null) distanceCalculatorText.text = "";
        if (parcelText != null) parcelText.text = "";
        if (missionObjectiveText != null) missionObjectiveText.text = "";
    }

    void Update()
    {
        // Dialogue trigger logic
        if (!istalking && !isMissionStarted)
        {
            CheckDistance();
        }
        else if (istalking)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                NextDialogue();
            }
        }

        // Mission progress logic
        if (isMissionStarted)
        {
            CheckDistanceDropOff();

            if (totaldistance < 2f)
            {
                EndMission();
            }
        }
    }

    void CheckDistance()
    {
        if (player == null || reciver == null) return;

        float dist = Vector3.Distance(player.position, reciver.position);
        if (dist < 2f)
        {
            if (PressEpannel != null) PressEpannel.SetActive(true);
            if (PressE != null) PressE.text = "Press E to Talk to Shree";

            if (Input.GetKeyDown(KeyCode.E))
            {
                StartDialogue();
            }
        }
        else
        {
            if (PressEpannel != null) PressEpannel.SetActive(false);
        }
    }

    public void StartDialogue()
    {
        istalking = true;
        if (dialogBox != null) dialogBox.SetActive(true);
        if (PressEpannel != null) PressEpannel.SetActive(false);
        dialogueIndex = 0;

        if (npcAnimManager != null) npcAnimManager.StartTalking();
        
        if (dialogues != null && dialogues.Length > 0)
        {
            if (speakerNameText != null) speakerNameText.text = dialogues[0].name;
            if (conversation != null) conversation.text = dialogues[0].text;
        }
    }

    void NextDialogue()
    {
        dialogueIndex++;
        if (dialogues != null && dialogueIndex < dialogues.Length)
        {
            if (speakerNameText != null) speakerNameText.text = dialogues[dialogueIndex].name;
            if (conversation != null) conversation.text = dialogues[dialogueIndex].text;
        }
        else
        {
            FinshDialogue();
        }
    }

    void FinshDialogue()
    {
        istalking = false;
        if (dialogBox != null) dialogBox.SetActive(false);
        if (npcAnimManager != null) npcAnimManager.StopTalking();
        ActivateMission();
    }

    void ActivateMission()
    {
        isMissionStarted = true;
        if (parcel != null) parcel.SetActive(true);
        if (parcelText != null) parcelText.text = "Restricted Parcel Picked!";
        if (missionObjectiveText != null) missionObjectiveText.text = "Deliver the package carefully.";
        StartCoroutine(ClearParcelText(3f));
    }

    void CheckDistanceDropOff()
    {
        if (player == null || dropOff == null) return;
        totaldistance = Vector3.Distance(player.position, dropOff.position);
        if (distanceCalculatorText != null) distanceCalculatorText.text = "Distance: " + totaldistance.ToString("F0") + "m";
    }

    void EndMission()
    {
        isMissionStarted = false;
        if (audioSource != null && successSound != null)
        {
            audioSource.PlayOneShot(successSound);
        }
        
        if (parcelText != null) parcelText.text = "Delivery Successful!";
        if (missionObjectiveText != null) missionObjectiveText.text = "Mission Completed!";
        if (distanceCalculatorText != null) distanceCalculatorText.text = ""; 
        
        StartCoroutine(ClearParcelText(3f));

        Playerstats ps = FindObjectOfType<Playerstats>();
        if (ps != null)
        {
            ps.AddMoney(800);
            ps.AddRating(15);
        }
    }

    System.Collections.IEnumerator ClearParcelText(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (parcelText != null) parcelText.text = "";
        if (missionObjectiveText != null) missionObjectiveText.text = "";
    }

    public void MissionFailed()
    {
        isMissionStarted = false;
        if (audioSource != null && failureSound != null)
        {
            audioSource.PlayOneShot(failureSound);
        }
        if (missionObjectiveText != null) missionObjectiveText.text = "Mission Failed!";
        if (distanceCalculatorText != null) distanceCalculatorText.text = "";
        StartCoroutine(ClearParcelText(3f));
    }
}
