using UnityEngine;
using UnityEngine.UI;

public class TimerBar : MonoBehaviour
{
    [SerializeField] private Slider timerSlider;

    private float countdownTimer;


    private void OnEnable()
    {
        timerSlider.maxValue = Recordings.instance.RecMaxDuration;
        timerSlider.value = Recordings.instance.RecMaxDuration;
        countdownTimer = Recordings.instance.RecMaxDuration;
    }

    private void Update()
    {
        countdownTimer = countdownTimer - Time.deltaTime;

        timerSlider.value = countdownTimer;

        if(timerSlider.value <= 0) 
        {
            Actions.OnTimerReachedZero?.Invoke();
            gameObject.SetActive(false);
        }
    }


}
