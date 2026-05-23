using UnityEngine;
using TMPro;

public class MissionManager : MonoBehaviour
{
    public int currentMissionIndex = 1;
    public TextMeshProUGUI objectiveText;

    [Header("Mission Objects")]
    public GameObject mission1Objects;
    public GameObject mission2Objects;

    void Start()
    {
        UpdateObjective();
    }

    public void CompleteMission()
    {
        currentMissionIndex++;
        UpdateObjective();
    }

    void UpdateObjective()
    {
        if (objectiveText == null) return;

        // Reset all mission objects first
        if (mission1Objects != null) mission1Objects.SetActive(false);
        if (mission2Objects != null) mission2Objects.SetActive(false);

        switch (currentMissionIndex)
        {
            case 1:
                StartMission1();
                break;
            case 2:
                StartMission2();
                break;
            default:
                objectiveText.text = "All Missions Complete.";
                break;
        }
    }

    void StartMission1()
    {
        objectiveText.text = "Mission 1: Go to your burning house.";
        if (mission1Objects != null) mission1Objects.SetActive(true);
    }

    void StartMission2()
    {
        objectiveText.text = "Mission 2: Meet Shree at the parking lot.";
        if (mission2Objects != null) mission2Objects.SetActive(true);
    }
}
