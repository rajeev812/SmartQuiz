using SmartQuiz.Domain.Entities;

namespace SmartQuiz.Application.Interfaces.Repositories;

public interface IBoardRepository
{
    Task<IEnumerable<Board>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<Board?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task AddAsync(Board board, CancellationToken cancellationToken = default);
    Task UpdateAsync(Board board, CancellationToken cancellationToken = default);
    Task DeleteAsync(Board board, CancellationToken cancellationToken = default);
}
