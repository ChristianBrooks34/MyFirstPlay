using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

public class AnimationEventReceiver : MonoBehaviour
{
    public event Action OnFootsteps;

    public void TriggerEventFootsteps()
    {
        OnFootsteps?.Invoke();
    }
}

