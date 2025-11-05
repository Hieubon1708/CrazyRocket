using UnityEngine;

public class UISetting : MonoBehaviour
{
    public GameObject[] vibrate;
    public GameObject[] sound;

    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public bool IsAtiveSound
    {
        get
        {
            return PlayerPrefs.GetInt("Sound", 1) == 1;
        }
        set
        {
            PlayerPrefs.SetInt("Sound", value ? 1 : 0);
        }
    }

    public bool IsActiveVibrate
    {
        get
        {
            return PlayerPrefs.GetInt("Vibrate", 1) == 1;
        }
        set
        {
            PlayerPrefs.SetInt("Vibrate", value ? 1 : 0);
        }
    }

    public void Active(int index)
    {
        if (index == 0)
        {
            IsAtiveSound = !IsAtiveSound;

            sound[0].SetActive(!IsAtiveSound);
            sound[1].SetActive(IsAtiveSound);
        }
        else
        {
            IsActiveVibrate = !IsActiveVibrate;

            vibrate[0].SetActive(!IsActiveVibrate);
            vibrate[1].SetActive(IsActiveVibrate);
        }
    }

    public void Hide()
    {
        animator.SetTrigger("Hide");
    }

    public void Show()
    {
        if (!gameObject.activeSelf) gameObject.SetActive(true);

        animator.SetTrigger("Show");
    }

    public void Facebook()
    {

    }
}
