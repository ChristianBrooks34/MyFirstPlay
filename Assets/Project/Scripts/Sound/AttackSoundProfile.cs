using UnityEngine;

[CreateAssetMenu(menuName = "Sound/Attack Music Config")]
public class AttackSoundProfile : ScriptableObject
{
    [SerializeField] private BaseSoundType[] _chargeClips;  // Замах / раскрутка
    [SerializeField] private BaseSoundType[] _releaseClips; // Основной удар / выстрел
    [SerializeField] private BaseSoundType[] _failClips;    // Осечка / КД
    [SerializeField] private BaseSoundType[] _reloadClips;  // Перезарядка

    public BaseSoundType GetRandomChargeClip() => GetRandom(_chargeClips);
    public BaseSoundType GetRandomReleaseClip() => GetRandom(_releaseClips);
    public BaseSoundType GetRandomFailClip() => GetRandom(_failClips);
    public BaseSoundType GetRandomReloadClip() => GetRandom(_reloadClips);

    private BaseSoundType GetRandom(BaseSoundType[] clips)
    {
        if (clips == null || clips.Length == 0) return null;
        return clips[UnityEngine.Random.Range(0, clips.Length)];
    }
}
