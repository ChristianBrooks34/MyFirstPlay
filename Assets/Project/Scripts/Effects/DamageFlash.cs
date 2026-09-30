using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class DamageFlash : IEffect
{
    private Color _flashColor;
    private float _flashDuration;
    private CancellationTokenSource _cts;
    private SpriteRenderer _spriteRenderer;
    private Color _originalColor;

    public DamageFlash(SpriteRenderer spriteRenderer, Color originalColor, Color flashColor, float flashDuration)
    {
        _flashColor = flashColor;
        _spriteRenderer = spriteRenderer;
        _originalColor = originalColor;
        _flashDuration = flashDuration;
    }

    public bool IsActive { get; private set; }

    public void Apply()
    {
        if (IsActive) return;

        if (_cts != null)
        {
            _cts.Cancel();
            _cts.Dispose();
        }

        _cts = new CancellationTokenSource();

        IsActive = true;

        Flash(_cts.Token).Forget();
    }

    public void Remove()
    {
        if (_cts != null)
        {
            _cts.Cancel();
            _cts.Dispose();
            _cts = null;
        }

        IsActive = false;
        _spriteRenderer.color = _originalColor;
    }


    private async UniTaskVoid Flash(CancellationToken cancellationToken)
    {
        _spriteRenderer.color = _flashColor;

        try
        {
            await UniTask.Delay((int)(_flashDuration * 1000));

            float time = 0;
            while (time < 1 && !cancellationToken.IsCancellationRequested)
            {
                time += Time.deltaTime / _flashDuration;

                if (time >= 1)
                {
                    _spriteRenderer.color = _originalColor;
                    break;
                }

                _spriteRenderer.color = Color.Lerp(_flashColor, _originalColor, time);
                await UniTask.DelayFrame(1);
            }
        }
        catch { }
    }
}
