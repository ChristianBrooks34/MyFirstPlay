using UnityEditor;

public sealed class ApplicationFinisher
{
    public void Finish()
    {
        EditorApplication.isPlaying = false;
    }
}
