public interface IGrabbable
{
    void OnGrab();
    void OnRelease();
    float GetWeight();
    float GetValue();
    string GetName();
}