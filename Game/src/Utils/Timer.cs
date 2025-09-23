/* An object to help handle elements that evolve with time. */

public class Timer
{
    /// <summary>
    /// Time elapsed since the timer started to count time.
    /// </summary>
    public float TimeElapsed { get; private set; }

    /// <summary>
    /// The time limit that the timer is evaluating.
    /// </summary>
    private float _timeLimit;

    /// <summary>
    /// Does our timer loop when it is over?
    /// </summary>
    private readonly bool _isLooping;

    /// <summary>
    /// Creates a new Timer.
    /// </summary>
    /// <param name="timeLimit"> Time Limit set for the Timer</param>
    /// <param name="isLooping"> Does the Timer reset itself at the end or does it just stop? </param>
    public Timer(float timeLimit, bool isLooping = true)
    {
        SetTimeLimit(timeLimit);
        _isLooping = isLooping;
        TimeElapsed = 0f;
    }

    #region Setters & Getters
    /// <summary>
    /// Change time limit of the timer.
    /// We have no fixed max and min values. We can add min and max vules with math.Clamp.
    /// </summary>
    /// <param name="timeLimit"></param>
    public void SetTimeLimit(float timeLimit)
    {
        _timeLimit = timeLimit;
    }

    public int GetTime()
    {
        return (int)(_timeLimit - TimeElapsed);
    }

    /// <summary>
    /// Resets the TimeElapsed value to 0.
    /// </summary>
    public void Reset()
    {
        TimeElapsed = 0f;
    }

    #endregion


    #region Update timer values
    /// <summary>
    /// Main method to update our timer
    /// </summary>
    /// <param name="deltaTime"> Time elapsed since last call.</param>
    /// <returns></returns>
    public bool Update(float deltaTime)
    {
        Increment(deltaTime);
        return CheckTimeLimit();
    }

    /// <summary>
    /// Increments our timer by a given value.
    /// </summary>
    /// <param name="deltaTime">Time to increment the timer by</param>
    public void Increment(float deltaTime)
    {
        TimeElapsed += deltaTime;
    }

    #endregion


    #region Checks on Timer values
    /// <summary>
    /// Checks if our timer has reached its end. If we have a Looping Timer, resets it.
    /// </summary>
    /// <returns></returns>
    private bool CheckTimeLimit()
    {
        if (TimeElapsed >= _timeLimit)
        {
            if (_isLooping)
            {
                Reset();
            }
            return true;
        }
        return false;
    }

    #endregion
}
