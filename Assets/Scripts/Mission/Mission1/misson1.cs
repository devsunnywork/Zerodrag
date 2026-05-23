using UnityEngine;
using TMPro;

[System.Serializable]
public class Dialogue
{
    public string name;
    [TextArea(3, 10)]
    public string text;
}

public class misson1 : MonoBehaviour
{
    public Transform player;
    public Transform reciver;
    public Transform dropOff;
    public TextMeshProUGUI PressE;
    public GameObject PressEpannel;
    public GameObject dialogBox;
    public GameObject parcel;
    public float remainingTime = 0;
    public TextMeshProUGUI conversation;
    public TextMeshProUGUI speakerNameText;
    public TextMeshProUGUI parcelText;
    public TextMeshProUGUI missionObjectiveText;
    public TextMeshProUGUI distanceCalculatorText;
    public TextMeshProUGUI timerText;
    public AudioSource audioSource; 
    public AudioClip successSound;
    public AudioClip failureSound;
    public NPCAnimationManager npcAnimManager;
    public GameObject dropOffMarker; // Addition: Visual marker at dropoff

    [Header("Mission Dialogues")]
    public Dialogue[] dialogues;
    private int dialogueIndex = 0;
    private bool istalking = false;
    private bool isMissionStarted = false;
    private float totaldistance;




    void Start()
    {
        if (timerText != null) timerText.text = "";
        if (distanceCalculatorText != null) distanceCalculatorText.text = "";
        if (parcelText != null) parcelText.text = "";
        if (missionObjectiveText != null) missionObjectiveText.text = "";
    }

    void Update()
    {
        if (istalking == false && isMissionStarted == false)
        {
            CheckDistance();
        }
        else if (istalking == true)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                NextDialogue();
            }
        }

        if (isMissionStarted == true)
        {
            CheckDistanceDropOff();

            if (totaldistance < 2f)
            {
                endmission();
            }
        }

        if(isMissionStarted == true)
        {
            remainingTime -= Time.deltaTime;
            timerText.text = "Time Left: " + remainingTime.ToString("F0") + "s";

            if(remainingTime <= 0)
            {
                missionFailed();
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
            
            // Dynamics name from first dialogue if available
            string npcName = (dialogues.Length > 0) ? dialogues[0].name : "NPC";
            if (PressE != null) PressE.text = "Press E to Talk to " + npcName;

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

    void StartDialogue()
    {
        istalking = true;
        dialogBox.SetActive(true);
        if (PressEpannel != null) PressEpannel.SetActive(false);
        dialogueIndex = 0;

        // NPC Animation Manager ko command do
        if (npcAnimManager == null && reciver != null)
        {
            npcAnimManager = reciver.GetComponent<NPCAnimationManager>();
        }

        if (npcAnimManager != null) 
        {
            npcAnimManager.StartTalking();
        }
        else
        {
            Debug.LogWarning("Mission1: NPCAnimationManager not found on Vikram/Reciver!");
        }
        
        if (dialogues.Length > 0)
        {
            speakerNameText.text = dialogues[0].name;
            conversation.text = dialogues[0].text;
        }
    }

    void NextDialogue()
    {
        dialogueIndex++;
        if (dialogueIndex < dialogues.Length)
        {
            speakerNameText.text = dialogues[dialogueIndex].name;
            conversation.text = dialogues[dialogueIndex].text;
        }
        else
        {
            EndDialogue();
        }
    }

    void EndDialogue()
    {
        istalking = false;
        dialogBox.SetActive(false);
        
        // NPC ko wapas Idle kardo
        if (npcAnimManager != null) npcAnimManager.StopTalking();
        
        ActivateMission();
    }

    void ActivateMission()
    {
        if(parcel!=null)
        { 
            isMissionStarted = true;
            remainingTime = 120f; 
            parcel.SetActive(true);
            if (dropOffMarker != null) dropOffMarker.SetActive(true); // Show marker

            // Integrate with global missiondistance if exists
            if (missiondistance.instance != null)
            {
                missiondistance.instance.SetTarget(dropOff);
            }

            parcelText.text = "Parcel Picked !";
            missionObjectiveText.text = "Drop off the parcel at the dropoff point.";
            StartCoroutine(ClearParcelText(3f));
        }
        else
        {
            Debug.LogError("Mission1: Parcel is not assigned in the inspector!");
        }
    }

    void CheckDistanceDropOff()
    {
        totaldistance = Vector3.Distance(player.position, dropOff.position);
        distanceCalculatorText.text = "Distance: " + totaldistance.ToString("F0") + "m";
    }

    void endmission()
    {

        if(audioSource!=null)
        {
            audioSource.PlayOneShot(successSound);
        }
        isMissionStarted = false;
        parcel.SetActive(false);
        if (dropOffMarker != null) dropOffMarker.SetActive(false); // Hide marker

        // Clear global distance target
        if (missiondistance.instance != null)
        {
            missiondistance.instance.ClearTarget();
        }

        parcelText.text = "Parcel Dropped !";
        missionObjectiveText.text = "Mission Completed !";
        distanceCalculatorText.text = ""; 
        timerText.text = ""; // Hide timer
        
        StartCoroutine(ClearParcelText(3f));

        Playerstats ps = FindObjectOfType<Playerstats>();
        if (ps != null)
        {
            ps.AddMoney(500);   
            ps.AddRating(10);   
        }
    }

    System.Collections.IEnumerator ClearParcelText(float delay)
    {
        yield return new WaitForSeconds(delay);
        if (parcelText != null) parcelText.text = "";
        if (missionObjectiveText != null) missionObjectiveText.text = "";
    }

    void missionFailed()
    {

        if(audioSource!=null)
        {
            audioSource.PlayOneShot(failureSound);
        }
        isMissionStarted = false;
        parcel.SetActive(false);
        if (dropOffMarker != null) dropOffMarker.SetActive(false); // Hide marker

        // Clear global distance target
        if (missiondistance.instance != null)
        {
            missiondistance.instance.ClearTarget();
        }

        missionObjectiveText.text = "Mission Failed !";
        distanceCalculatorText.text = "";
        timerText.text = ""; // Hide timer
        StartCoroutine(ClearParcelText(3f));
    }
}
