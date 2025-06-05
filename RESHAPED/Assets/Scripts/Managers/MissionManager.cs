using UnityEngine;
using UnityEngine.UI;

public class MissionManager : MonoBehaviour
{
    private static MissionManager instance;
    public static MissionManager Instance { get { return instance; } }

    void Awake()
    {
        if (instance != null) { Destroy(instance); }
        instance = this;
    }

    [Header("Mission Settings")]
    public bool isMissionActive;
    public float missionTimeInterval;
    public float minMissionTimeInterval, maxMissionTimeInterval;
    private float missionTimeIntervalTimer;
    public float missionDuration;
    public float minMissionDuration, maxMissionDuration;
    private float currentMissionTime;
    private float missionProgress;
    public float MissionProgress
    {
        get { return missionProgress; }
    }

    private float maxMissionProgress;
    public float MaxMissionProgress
    {
        get { return maxMissionProgress; }
    }


    // [Header("Mission Types")]
    public enum MissionType
    {
        None,
        Collect,
        Defeat,
        Hitless,
        // Capture
    }

    public MissionType activeMissionType;

    [Header("Mission Reference")]
    public GameObject collectMissionPrefab;
    public GameObject missionUI;
    private TMPro.TextMeshProUGUI missionText;
    public SpriteRenderer missionInactiveSprite, missionActiveSprite;
    public GameObject sliderObj;
    private Slider slider;

    void Start()
    {
        missionText = missionUI.GetComponentInChildren<TMPro.TextMeshProUGUI>();
        slider = sliderObj.GetComponent<Slider>();
        MissionReset(); // Initialize mission state
    }

    void Update()
    {
        if (!isMissionActive && missionTimeIntervalTimer >= 0f)
        {
            missionTimeIntervalTimer += Time.deltaTime;
            if (missionTimeIntervalTimer >= missionTimeInterval)
            {
                StartMission((MissionType)Random.Range(1, System.Enum.GetValues(typeof(MissionType)).Length), Random.Range(minMissionDuration, maxMissionDuration));
                missionText.text = $"Mission: {activeMissionType}";
                missionTimeIntervalTimer = 0f; // Reset the timer after starting a mission
            }
        }
        if (isMissionActive)
        {
            currentMissionTime += Time.deltaTime;
            slider.value = currentMissionTime;

            if (currentMissionTime >= missionDuration)
            {
                OnMissionFailed();
            }
        }
    }

    public void ProgressMission(float value)
    {
        if (isMissionActive)
        {
            missionProgress += value;
            missionText.text = $"Mission: {activeMissionType} \n Mission Progress: {missionProgress}/{maxMissionProgress}";
            if (missionProgress >= maxMissionProgress)
            {
                OnMissionSuccess();
            }
        }
    }

    public void StartMission(MissionType missionType, float duration)
    {
        if (!isMissionActive)
        {
            isMissionActive = true;
            activeMissionType = missionType;
            missionDuration = duration;
            currentMissionTime = 0f;
            missionProgress = 0f;

            switch (missionType)
            {
                case MissionType.Defeat:
                    maxMissionProgress = 10f; // Example value for collect missions
                    break;
                case MissionType.Collect:
                    maxMissionProgress = 7f; // Example value for defeat missions
                    break;
                case MissionType.Hitless:
                    maxMissionProgress = 1f; // Example value for Hitless missions
                    break;
                // case MissionType.Capture:
                //     maxMissionProgress = 1f; // Example value for capture missions
                //     break;
                default:
                    maxMissionProgress = 0f;
                    break;
            }
            
            sliderObj.SetActive(true);
            slider.maxValue = missionDuration;
            slider.value = 0f;

            missionInactiveSprite.enabled = false;
            missionActiveSprite.enabled = true;

            Debug.Log($"Mission Started: {missionType} for {duration} seconds.");
        }
    }

    public void MissionReset()
    {
        isMissionActive = false;
        activeMissionType = MissionType.None;
        currentMissionTime = 0f;
        missionProgress = 0f;

        slider.value = 0;;
        sliderObj.SetActive(false);

        missionInactiveSprite.enabled = true;
        missionActiveSprite.enabled = false;

        SetMissionTimeInterval(); // Reset the mission time interval

        missionText.text = "No Active Mission";

        Debug.Log("Mission Reset.");
    }

    public void OnMissionFailed()
    {
        if (isMissionActive)
        {
            if (activeMissionType == MissionType.Hitless && currentMissionTime >= missionDuration)
            {
                OnMissionSuccess();
            }
            Debug.Log("Mission Failed!");
            // Handle mission failure logic here, e.g., reset progress, notify player, etc.
            MissionReset();
        }
    }

    public void OnMissionSuccess()
    {
        if (isMissionActive)
        {
            SkillTreeManager.Instance.AddSkillPoint();
            Debug.Log("Mission Completed!");
            MissionReset();
        }
    }

    void SetMissionTimeInterval()
    {
        missionTimeInterval = Random.Range(minMissionTimeInterval, maxMissionTimeInterval);
    }
    
    public void SpawnCollectMission(Vector3 position)
    {
        if (collectMissionPrefab != null)
        {
            GameObject collectMission = Instantiate(collectMissionPrefab, position, Quaternion.identity);
            collectMission.transform.SetParent(transform); // Set the parent to MissionManager
        }
        else
        {
            Debug.LogError("Collect mission prefab is not assigned!");
        }
    }
}
