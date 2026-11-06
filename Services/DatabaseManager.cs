using Dapper;
using ElkollegeAdmissionsBot.Models.Bot;
using ElkollegeAdmissionsBot.Models.Database;
using Microsoft.Data.Sqlite;

namespace ElkollegeAdmissionsBot.Services;

public class DatabaseManager
{
    private readonly string _sqliteConnectionString;

    public DatabaseManager()
    {
        _sqliteConnectionString = new SqliteConnectionStringBuilder
        {
            DataSource = "Application.db",
            DefaultTimeout = 5,
            ForeignKeys = true
        }.ToString();

        using var connection = new SqliteConnection(_sqliteConnectionString);

        connection.Query(
            """
            PRAGMA journal_mode = WAL;

            CREATE TABLE IF NOT EXISTS states (
                UserId        INTEGER NOT NULL,
                MessengerType INTEGER NOT NULL,
                Type          INTEGER NOT NULL,
                PRIMARY KEY (UserId, MessengerType) ON CONFLICT REPLACE
            );

            CREATE TABLE IF NOT EXISTS messages (
                Id             TEXT    NOT NULL PRIMARY KEY,
                MessengerType  INTEGER NOT NULL,
                OriginalId     TEXT    NOT NULL,
                OriginalChatId INTEGER NOT NULL,
                UNIQUE (MessengerType, OriginalId, OriginalChatId) ON CONFLICT REPLACE
            );
            """);
    }

    public async Task<DatabaseState> UpsertStateAsync(DatabaseState state,
        CancellationToken cancellationToken = default)
    {
        await using var connection = new SqliteConnection(_sqliteConnectionString);

        return await connection.QuerySingleAsync<DatabaseState>(
            new CommandDefinition(
                """
                INSERT INTO states (UserId, MessengerType, Type)
                VALUES (@UserId, @MessengerType, @Type)
                RETURNING *;
                """,
                state,
                cancellationToken: cancellationToken));
    }

    public async Task<DatabaseState> GetStateAsync(long userId, BotMessengerType messengerType,
        CancellationToken cancellationToken = default)
    {
        await using var connection = new SqliteConnection(_sqliteConnectionString);

        var state = await connection.QuerySingleOrDefaultAsync<DatabaseState>(
            new CommandDefinition(
                "SELECT * FROM states WHERE UserId = @UserId AND MessengerType = @MessengerType;",
                new { UserId = userId, MessengerType = messengerType },
                cancellationToken: cancellationToken));

        return state ?? await UpsertStateAsync(
            state: new DatabaseState { UserId = userId, MessengerType = messengerType },
            cancellationToken: cancellationToken);
    }

    public async Task InsertMessageAsync(DatabaseMessage message, CancellationToken cancellationToken = default)
    {
        await using var connection = new SqliteConnection(_sqliteConnectionString);

        await connection.QueryAsync(
            new CommandDefinition(
                """
                INSERT INTO messages (Id, MessengerType, OriginalId, OriginalChatId)
                VALUES (@Id, @MessengerType, @OriginalId, @OriginalChatId);
                """,
                message,
                cancellationToken: cancellationToken));
    }

    public async Task<DatabaseMessage?> GetMessageAsync(string id, CancellationToken cancellationToken = default)
    {
        await using var connection = new SqliteConnection(_sqliteConnectionString);

        return await connection.QuerySingleOrDefaultAsync<DatabaseMessage>(
            new CommandDefinition(
                "SELECT * FROM messages WHERE Id = @Id;",
                new { Id = id },
                cancellationToken: cancellationToken));
    }
}
