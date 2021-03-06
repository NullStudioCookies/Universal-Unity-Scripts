using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
    This is a universal timer script, the script allows for the
    change in timer function, reset, pause, and unpause.
*/

public class UniversalTimer : MonoBehaviour {
    #region Timer Properties
    protected enum TimerFunction { CountingUp, CountingDown }

    [Header("Timer Properties")]
    [Tooltip("Causes the timer to pause or resume its process.")] protected bool timerIsPause = false;
    [Tooltip("Determines if the timer will count up or down.")] [SerializeField] protected TimerFunction function = TimerFunction.CountingUp;
    [Tooltip("What is the total amount of time the timer has to count down in seconds.")]  [ConditionalEnumHide("function", (int)TimerFunction.CountingDown)] protected int allowedTime = 0;

    float CurrentTime = 0;
    string Minutes;
    string Seconds;
    #endregion

    #region Core Timer Functions
    // Timer function that can be used in other class in the update method
    public void RunTimer() {
        if (!timerIsPause) {
            switch (function) {
                case TimerFunction.CountingUp:
                    CurrentTime += Time.deltaTime;

                    Minutes = ((int)CurrentTime / 60).ToString();
                    Seconds = (CurrentTime % 60).ToString("f2");
                    break;
                case TimerFunction.CountingDown:
                    CurrentTime -= Time.deltaTime;

                    if (CurrentTime <= 0) {
                        CurrentTime = 0;
                    }

                    Minutes = ((int)CurrentTime / 60).ToString();
                    Seconds = (CurrentTime % 60).ToString("f2");
                    break;
            }
        }
    }

    // A simple way to pause and unpause the timer script
    public void PauseUnpauseTimer() {
        timerIsPause = !timerIsPause;
    }

    // Resetting the timer based on the current function, the TotalTime variable is only required for the CountingDown function
    public void ResetTimer(int TotalTime = 0) {
        timerIsPause = true;

        switch (function) {
            case TimerFunction.CountingUp:
                CurrentTime = 0;

                Minutes = ((int)CurrentTime / 60).ToString();
                Seconds = (CurrentTime % 60).ToString("f2");
                break;
            case TimerFunction.CountingDown:
                if (TotalTime > 0) {
                    CurrentTime = TotalTime;
                }
                else {
                    CurrentTime = allowedTime;
                }

                Minutes = ((int)CurrentTime / 60).ToString();
                Seconds = (CurrentTime % 60).ToString("f2");
                break;
        }
    }

    // Method to get the timer string
    public string TimerText() {
        string ReturnText = Minutes + ":" + Seconds;
        return ReturnText;
    }
    #endregion

    #region Changing Function
    // Alternates between the various timer function
    public void SwitchTimerFunction(int TotalTime = 0) {
        timerIsPause = true;

        switch (function) {
            case TimerFunction.CountingUp:
                function = TimerFunction.CountingDown;

                CurrentTime = TotalTime;

                Minutes = ((int)CurrentTime / 60).ToString();
                Seconds = (CurrentTime % 60).ToString("f2");
                break;
            case TimerFunction.CountingDown:
                function = TimerFunction.CountingUp;

                CurrentTime = 0;

                Minutes = ((int)CurrentTime / 60).ToString();
                Seconds = (CurrentTime % 60).ToString("f2");
                break;
        }
    }

    // Setter functions to change to the specific timer function
    public void SetTimerCountUp() {
        timerIsPause = true;
        function = TimerFunction.CountingUp;
        CurrentTime = 0;

        Minutes = ((int)CurrentTime / 60).ToString();
        Seconds = (CurrentTime % 60).ToString("f2");
    }
    public void SetTimerCountDown(int TotalTime = 0) {
        timerIsPause = true;
        function = TimerFunction.CountingDown;
        CurrentTime = TotalTime;

        Minutes = ((int)CurrentTime / 60).ToString();
        Seconds = (CurrentTime % 60).ToString("f2");
    }
    #endregion
}