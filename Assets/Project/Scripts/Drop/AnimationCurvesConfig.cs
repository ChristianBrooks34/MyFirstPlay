using UnityEngine;

[CreateAssetMenu(fileName = "AnimationCurvesConfig", menuName = "Configs/AnimationCurvesConfig")]
public class AnimationCurvesConfig : ScriptableObject
{
    [SerializeField] private AnimationCurve _pulseScaleCurve;
    [SerializeField] private AnimationCurve _waveVerticalCurve;

    public AnimationCurve PulseScale => _pulseScaleCurve;
    public AnimationCurve WaveVertical => _waveVerticalCurve;
}