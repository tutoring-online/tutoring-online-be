using System.Text.Json.Nodes;

namespace DataAccess.Models;

public class ApiResponse
{
    public int? ResultCode { get; set; }
    
    public string? ResultMessage { get; set; }
    
    public object? Data { get; set; }
    
}