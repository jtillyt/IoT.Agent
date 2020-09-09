namespace IoT.Agent.Services
{
    public interface IMetricsService
    {
        void AddValueTypeMapping(int valueTypeId, string name);
    }
}