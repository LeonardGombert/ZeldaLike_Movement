using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Timer : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    private BombParameter bombParameter;
    [SerializeField]
    private PipeParameter pipeParameter;

    [Space]

    [Header("Pipe Difficulty")]
    [SerializeField]
    private List<float> pipeSpeed;
    [SerializeField]
    private List<float> pipeResetTime;

    [Space]

    [Header("Bomb Difficulty")]
    [SerializeField]
    private List<Vector2> fuseTimeDecrease;
    [SerializeField]
    private List<Vector2Int> radiusIncrease;

    [SerializeField]
    private int stepNbr;
    private float setpTime;

    [SerializeField]
    private float timer;
    private float timerActual;

    private int currentStep = 1;

    [Space]

    [Header("PowerUps")]
    [SerializeField]
    private int powerUpNbr;
    [SerializeField]
    private Transform powerUpHolder;
    [SerializeField]
    private List<GameObject> powerUpPrefabs;

    private List<float> powerUpSpawnTimes = new List<float>();

    void Awake()
    {
        UpdateBombParameter();
        UpdatePipeParameter();

        setpTime = timer / stepNbr;

        for (int i = 0; i < powerUpNbr; i++)
        {
            powerUpSpawnTimes.Add(Random.Range(0, timer));
        }

        StartCoroutine(RunTimer());
    }

    private void UpdateBombParameter()
    {
        bombParameter.fuseTimeRange = fuseTimeDecrease[currentStep - 1];
        bombParameter.radiusRange = radiusIncrease[currentStep - 1];
    }

    private void UpdatePipeParameter()
    {
        pipeParameter.speed = pipeSpeed[currentStep - 1];
        pipeParameter.resetTime = pipeResetTime[currentStep - 1];

        pipeParameter.SetPipesDifficulty();
    }

    private void PowerUpSpawner()
    {
        int randomIndex = Random.Range(0, powerUpPrefabs.Count);
        Grid.PlaceOnRandomPos(1, powerUpPrefabs[randomIndex], powerUpHolder);
    }

    IEnumerator RunTimer()
    {
        int lastCurrentStep = currentStep;

        for (timerActual = 0; timerActual <= timer; timerActual += Time.deltaTime)
        {
            currentStep = Mathf.FloorToInt(timerActual / setpTime) + 1;

            if(lastCurrentStep != currentStep)
            {
                UpdateBombParameter();
                UpdatePipeParameter();
            }

            foreach (float spawnTime in powerUpSpawnTimes)
            {
                if (timerActual > spawnTime)
                {
                    PowerUpSpawner();
                    powerUpSpawnTimes.Remove(spawnTime);
                    break;
                }
            }

            yield return null;
        }
    }
}
