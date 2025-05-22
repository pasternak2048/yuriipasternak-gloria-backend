namespace GLORIA.BuildingBlocks.Security
{
    public interface ISignatureService
    {
        string Generate();
        string Generate(DateTime utcNow);
        string ServiceName { get; }
    }
}
