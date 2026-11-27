using System;
using UnityEngine;
using Zenject;

public abstract class Drop : MonoBehaviour, IPickable 
{
    [NonSerialized] public DropProfile DropProfile;
    public bool IsCollected { get; set; }

    private DropAudio _dropAudio;

    [Inject]
    public void ConstructBase(DropAudio dropAudio)
    {
        _dropAudio = dropAudio;
    }

    public abstract bool CanBePicked();

    public virtual void OnPicked()
    {
        _dropAudio.PlayPickableAudio(DropProfile);
    }
}
