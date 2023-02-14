namespace BL.Lib;

public abstract class ALogicBase
{
    public event EventHandler TaskReporting = delegate { };
    protected virtual void OnTaskReporting(EventArgs e) => TaskReporting?.Invoke(this, e);

    public event EventHandler TaskFail = delegate { };
    protected virtual void OnTaskFail(EventArgs e) => TaskFail?.Invoke(this, e);

    public event EventHandler TaskSuccess = delegate { };
    protected virtual void OnTaskSuccess(EventArgs e) => TaskSuccess?.Invoke(this, e);
}
