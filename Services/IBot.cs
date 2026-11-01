namespace ElkollegeAdmissionsBot.Services;

public interface IBot
{
    public Task PrepareAsync(CancellationToken cancellationToken = default);
}
