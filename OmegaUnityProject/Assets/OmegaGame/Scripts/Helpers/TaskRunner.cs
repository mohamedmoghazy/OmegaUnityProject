using System;
using System.Threading.Tasks;
using UnityEngine;

public static class TaskRunner
{
    /// <summary>
    /// Asynchronously executes the passed in task in a "fire-and-forget" manner.
    /// Execution continues immediately, not when the task is done.
    /// resource link https://stackoverflow.com/questions/22629951/suppressing-warning-cs4014-because-this-call-is-not-awaited-execution-of-the
    /// </summary>
    /// <param name="task">The executed task.</param>
    /// <param name="onComplete">Called after the task finishes.</param>
    /// <param name="onException">Called if the task throws an exception.</param>
    public static void Start(Task task, Action onComplete = null, Action<Exception> onException = null)
    {
#pragma warning disable 4014
        Run(task, onComplete, onException);
#pragma warning restore 4014
    }

    private static async Task Run(Task task, Action onComplete = null, Action<Exception> onException = null)
    {
        try
        {
            await task;
            onComplete?.Invoke();
        }
        catch (Exception taskException)
        {
            Debug.LogError(taskException.Message + " TaskRunner");
            try
            {
                onException?.Invoke(taskException);
            }
            catch (Exception callbackException)
            {
                Debug.LogError("TaskRunner encountered exception in exception callback: " + callbackException.Message);
            }
        }
    }
}