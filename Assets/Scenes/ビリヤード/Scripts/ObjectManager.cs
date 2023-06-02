using UnityEngine;
using TMPro;

public class ObjectManager : MonoBehaviour
{
    public GameObject textUI;
    public GameObject obstacleInstance;
    private GameObject[] obstacles;
    public bool willCreateObstacles;

    private void Start()
    {
        _attemptTimes = 0;
    }
    private int _score;
    public int Score
    {
        get => _score;
        set
        {
            if(_score<value&& willCreateObstacles)
            {
                UpdateStageObjects();
            }
            _score = value;
            UpdateUI();
        }
    }

    private int _attemptTimes;
    public int AttemptTimes
    {
        get => _attemptTimes;
        set
        {
            _attemptTimes = value;
            UpdateUI();
        }
    }
    private void UpdateUI()
    {
        textUI.GetComponent<TextMeshProUGUI>().text = "Score:"+_score+"\nAttempt:"+_attemptTimes+"\nClick Right Click to Speed Down";
    }
    private void UpdateStageObjects()
    {
        GameObject obstacle = Instantiate(obstacleInstance, new Vector3(Random.Range(-20f, 20f), 2, Random.Range(-20f, 20f)), Quaternion.identity, transform);
    }
}
