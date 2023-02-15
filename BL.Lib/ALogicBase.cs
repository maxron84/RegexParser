namespace BL.Lib;

public abstract class ALogicBase
{
    /// <summary>
    /// The method first creates a new task by calling "Task.Run" with the provided action. It then enters a loop that runs until the task is completed successfully. Within the loop, 
    /// the method raises an event to report progress. If the task encounters an exception, the method raises another event to indicate failure and returns a completed task. 
    /// Once the task has completed successfully, the method raises a final event to indicate success and returns a completed task.
    /// </summary>
    /// 
    /// <param name="function">The action to be executed</param>
    /// 
    /// <returns>A completed Task</returns>
    protected Task ExecuteTaskAdvanced(Func<Task> function)
    {
        Task task = Task.Run(function);

        while (!task.IsCompletedSuccessfully)
        {
            OnTaskReporting(EventArgs.Empty);

            if (task.IsFaulted)
            {
                OnTaskFail(EventArgs.Empty);
                return Task.CompletedTask;
            }
        }

        OnTaskSuccess(EventArgs.Empty);
        return Task.CompletedTask;
    }

    public event EventHandler TaskReporting = delegate { };
    protected virtual void OnTaskReporting(EventArgs e) => TaskReporting?.Invoke(this, e);

    public event EventHandler TaskFail = delegate { };
    protected virtual void OnTaskFail(EventArgs e) => TaskFail?.Invoke(this, e);

    public event EventHandler TaskSuccess = delegate { };
    protected virtual void OnTaskSuccess(EventArgs e) => TaskSuccess?.Invoke(this, e);
}
