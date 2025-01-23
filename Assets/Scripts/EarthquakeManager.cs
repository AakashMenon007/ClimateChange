using UnityEngine;

public class EarthquakeManagerV2 : MonoBehaviour
{
    public LowFrequencyEarthquake lowFrequencyEarthquake;
    public MidFrequencyEarthquake midFrequencyEarthquake;
    public HighFrequencyEarthquake highFrequencyEarthquake;

    public void EmergencyStopAll()
    {
        Debug.Log("Emergency Stop Triggered: Stopping All Earthquakes");

        // Stop all earthquakes
        if (lowFrequencyEarthquake != null) lowFrequencyEarthquake.EmergencyStop();
        if (midFrequencyEarthquake != null) midFrequencyEarthquake.EmergencyStop();
        if (highFrequencyEarthquake != null) highFrequencyEarthquake.EmergencyStop();
    }
}
