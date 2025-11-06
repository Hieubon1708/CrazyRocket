using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class UIPlayTimes : MonoBehaviour
{
    public TextMeshProUGUI textPlayTimes;
    public TextMeshProUGUI textCountdownTime;

    public int totalPlayTimes;
    public float totalCountdownTime;

    private void Start()
    {
        if (AmountPlayTimes == totalPlayTimes) return;

        DateTime currentTime = DateTime.Now;
        DateTime startTime = StartTime;

        while (currentTime.AddSeconds(-totalCountdownTime) >= startTime)
        {
            if (AmountPlayTimes >= totalPlayTimes)
            {
                return;
            }

            AmountPlayTimes++;

            currentTime = currentTime.AddSeconds(-totalCountdownTime);

            StartTime = StartTime.AddSeconds(totalCountdownTime);
        }

        double remainingTime = (currentTime - startTime).TotalSeconds;

        StartCoroutine(Countdown(remainingTime));

        UpdatePlayTimes();
    }

    DateTime StartTime
    {
        get
        {
            return DateTime.Parse(PlayerPrefs.GetString("StartTime", DateTime.Now.ToString()));
        }
        set
        {
            PlayerPrefs.SetString("StartTime", value.ToString());
        }
    }

    int AmountPlayTimes
    {
        get
        {
            return PlayerPrefs.GetInt("PlayTimes", totalPlayTimes);
        }
        set
        {
            PlayerPrefs.SetInt("PlayTimes", Math.Clamp(value, 0, totalPlayTimes));
        }
    }

    public void Play()
    {
        if (AmountPlayTimes == totalPlayTimes)
        {
            StartTime = DateTime.Now;

            StartCoroutine(Countdown(0));
        }

        AmountPlayTimes--;

        UpdatePlayTimes();
    }

    public IEnumerator Countdown(double remainingTime)
    {
        Active(true);

        while (remainingTime < totalCountdownTime)
        {
            TimeSpan time = TimeSpan.FromSeconds(totalCountdownTime - remainingTime);
            textCountdownTime.text = string.Format("{0:D2}:{1:D2}", time.Minutes, time.Seconds);

            remainingTime += Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }

        Active(false);

        AmountPlayTimes++;

        UpdatePlayTimes();

        if (AmountPlayTimes < totalPlayTimes)
        {
            StartTime = DateTime.Now;

            StartCoroutine(Countdown(0));
        }
    }

    void UpdatePlayTimes()
    {
        textPlayTimes.text = AmountPlayTimes + "/" + totalPlayTimes;
    }

    void Active(bool active)
    {
        textCountdownTime.transform.parent.gameObject.SetActive(active);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            Play();
        }
    }
}
