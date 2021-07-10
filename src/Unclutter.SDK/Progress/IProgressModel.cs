namespace Unclutter.SDK.Progress
{
    public interface IProgressModel
    {
        bool IsCancelable { get; set; }
        bool IsIndeterminate { get; set; }
        double ProgressValue { get; set; }
        string Message { get; set; }
        string Title { get; set; }
    }
}
