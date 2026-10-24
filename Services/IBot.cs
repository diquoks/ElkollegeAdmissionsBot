namespace ElkollegeAdmissionsBot.Services;

public interface IBot<out THandler> where THandler : Handler
{
    public THandler Handler { get; }

    public Task PrepareAsync(CancellationToken cancellationToken = default)
    {
        return Handler.PrepareAsync(cancellationToken);
    }
}
