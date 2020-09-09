namespace IoT.ServiceHost.Gpio
{
    public interface IDuplexSerialService
    {
        void StartListening();
        void StopListening();
    }
}