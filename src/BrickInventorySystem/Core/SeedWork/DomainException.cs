namespace BrickManager.BrickInventorySystem.Core.SeedWork;
 
public class DomainException(string code, string message) : Exception(message)
{
    public string Code { get; } = code;
} 