// Infrastructure/Repositories/DapperOrderRepository.cs
using Dapper;
using Microsoft.Data.SqlClient;
using OrderService.Core.Entities;
using OrderService.Core.Interfaces.Repositories;

namespace OrderService.Infrastructure.Repositories;

public class DapperOrderRepository : IOrderRepository
{
    private readonly string _connectionString;

    public DapperOrderRepository(string connectionString)
    {
        _connectionString = connectionString;
    }

    public async Task<IEnumerable<Order>> GetAllAsync(CancellationToken ct = default)
    {
        await using var connection = new SqlConnection(_connectionString);
        return await connection.QueryAsync<Order>(
            "SELECT Id, Customer, Total, Status, CreatedAt FROM Orders ORDER BY CreatedAt DESC");
    }

    public async Task<Order?> GetByIdAsync(int id, CancellationToken ct = default)
    {
        await using var connection = new SqlConnection(_connectionString);
        return await connection.QuerySingleOrDefaultAsync<Order>(
            "SELECT Id, Customer, Total, Status, CreatedAt FROM Orders WHERE Id = @Id",
            new { Id = id });
    }

    public async Task<Order> AddAsync(Order order, CancellationToken ct = default)
    {
        await using var connection = new SqlConnection(_connectionString);
        var id = await connection.ExecuteScalarAsync<int>("""
            INSERT INTO Orders (Customer, Total, Status, CreatedAt)
            VALUES (@Customer, @Total, @Status, @CreatedAt);
            SELECT CAST(SCOPE_IDENTITY() AS INT);
            """,
            new
            {
                order.Customer,
                order.Total,
                Status = order.Status.ToString(),
                order.CreatedAt
            });

        order.Id = id;
        return order;
    }
}
