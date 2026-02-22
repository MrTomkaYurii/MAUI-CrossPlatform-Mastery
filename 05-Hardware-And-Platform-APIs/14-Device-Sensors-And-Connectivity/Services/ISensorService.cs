namespace MauiMastery.DeviceSensors.Services;

public record GeoPoint(double Latitude, double Longitude, double Accuracy);

public interface ISensorService
{
    bool IsConnected { get; }
    double BatteryLevel { get; }
    Task<GeoPoint> GetCurrentLocationAsync();
}

public class MockSensorService : ISensorService
{
    public bool IsConnected => true;
    public double BatteryLevel => 0.85;
    public Task<GeoPoint> GetCurrentLocationAsync()
        => Task.FromResult(new GeoPoint(50.4501, 30.5234, 10.0)); // Київ
}