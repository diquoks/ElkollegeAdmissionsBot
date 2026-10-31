using Dapper;
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
                UserId INTEGER NOT NULL PRIMARY KEY ON CONFLICT REPLACE,
                Type   INTEGER NOT NULL
            );

            CREATE TABLE IF NOT EXISTS messages (
                Id             TEXT    NOT NULL PRIMARY KEY,
                Type           INTEGER NOT NULL,
                OriginalId     TEXT    NOT NULL,
                OriginalChatId INTEGER NOT NULL,
                UNIQUE (Type, OriginalId, OriginalChatId) ON CONFLICT REPLACE
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
                INSERT INTO states (UserId, Type)
                VALUES (@UserId, @Type)
                RETURNING *;
                """,
                state,
                cancellationToken: cancellationToken));
    }

    public async Task<DatabaseState> GetStateAsync(long userId, CancellationToken cancellationToken = default)
    {
        await using var connection = new SqliteConnection(_sqliteConnectionString);

        var state = await connection.QuerySingleOrDefaultAsync<DatabaseState>(
            new CommandDefinition(
                "SELECT * FROM states WHERE UserId = @UserId;",
                new { UserId = userId },
                cancellationToken: cancellationToken));

        return state ?? await UpsertStateAsync(new DatabaseState { UserId = userId }, cancellationToken);
    }

    public async Task InsertMessageAsync(DatabaseMessage message, CancellationToken cancellationToken = default)
    {
        await using var connection = new SqliteConnection(_sqliteConnectionString);

        await connection.QueryAsync(
            new CommandDefinition(
                """
                INSERT INTO messages (Id, Type, OriginalId, OriginalChatId)
                VALUES (@Id, @Type, @OriginalId, @OriginalChatId);
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
