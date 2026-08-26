using System.Text.Json;
using SmartQuiz.Domain.Entities;

namespace SmartQuiz.Infrastructure.Services;

public class JsonUserStore
{
    private readonly string _filePath = Path.Combine(AppContext.BaseDirectory, "users.json");
    private readonly SemaphoreSlim _lock = new(1, 1);
    private readonly JsonSerializerOptions _jsonOptions = new(JsonSerializerDefaults.Web) { WriteIndented = true };

    public async Task<User?> FindByEmailAsync(string email, CancellationToken cancellationToken = default)
    {
        await _lock.WaitAsync(cancellationToken);
        try
        {
            var users = await ReadUsersAsync(cancellationToken);
            return users.FirstOrDefault(user => string.Equals(user.Email, email.Trim(), StringComparison.OrdinalIgnoreCase));
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task AddAsync(User user, CancellationToken cancellationToken = default)
    {
        await _lock.WaitAsync(cancellationToken);
        try
        {
            var users = await ReadUsersAsync(cancellationToken);
            if (users.Any(existing => string.Equals(existing.Email, user.Email, StringComparison.OrdinalIgnoreCase)))
            {
                throw new InvalidOperationException("An account with this email already exists.");
            }

            users.Add(user);
            await WriteUsersAsync(users, cancellationToken);
        }
        finally
        {
            _lock.Release();
        }
    }

    private async Task<List<User>> ReadUsersAsync(CancellationToken cancellationToken)
    {
        if (!File.Exists(_filePath))
        {
            return new List<User>();
        }

        await using var stream = File.OpenRead(_filePath);
        return await JsonSerializer.DeserializeAsync<List<User>>(stream, _jsonOptions, cancellationToken) ?? new List<User>();
    }

    private async Task WriteUsersAsync(List<User> users, CancellationToken cancellationToken)
    {
        Directory.CreateDirectory(Path.GetDirectoryName(_filePath)!);
        await using var stream = File.Create(_filePath);
        await JsonSerializer.SerializeAsync(stream, users, _jsonOptions, cancellationToken);
    }
}
