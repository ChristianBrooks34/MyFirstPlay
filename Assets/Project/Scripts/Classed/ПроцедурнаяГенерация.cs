using UnityEngine;

public enum Country
{
    Russia,
    Ucraina
}

public class ПроцедурнаяГенерация : MonoBehaviour
{
    [SerializeField][Range(2, 128)] private int _resolution = 128;
    [SerializeField] private FilterMode _filterMode = FilterMode.Point;
    [SerializeField] private Country _country;
    [SerializeField] private bool _displayCountry;
    [SerializeField] private TextureWrapMode _textureWrapMode;
    [SerializeField] private Camera _camera;
    [SerializeField] private Collider _collider;
    [SerializeField] private Material _material;
    [SerializeField] private Color _color;
    [SerializeField] private int _brushSize = 2;

    private Texture2D _texture;
    private float _step;
    private int _oldRayX;
    private int _oldRayY;

    [Header("Круг")]
    [SerializeField] private int _outRadius;
    [SerializeField] private int _inRadius;
    [SerializeField] private Vector2 _center;
    [SerializeField] private Gradient _gradient;

    private void Update()
    {
        _brushSize += (int)Input.mouseScrollDelta.y;
        if (Input.GetMouseButton(0))
        {
            Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (_collider.Raycast(ray, out hit, 100f))
            {
                int rayX = (int)(hit.textureCoord.x * _resolution);
                int rayY = (int)(hit.textureCoord.y * _resolution);

                if (_oldRayX != rayX || _oldRayY != rayY)
                {
                    DrawCircle(rayX, rayY);

                    _oldRayX = rayX;
                    _oldRayY = rayY;
                }
            }
        }
    }

    private void OnValidate()
    {
        if (_texture == null)
        {
            _texture = new Texture2D(_resolution, _resolution);
            _step = 1f / _resolution;
        }

        if (_texture.width != _resolution)
        {
            _texture.Reinitialize(_resolution, _resolution);
        }

        _texture.filterMode = _filterMode;
        _texture.wrapMode = _textureWrapMode;

        #region 
        //if (_displayCountry)
        //{
        //    if (_country == Country.Russia)
        //    {
        //        for (int x = 0; x < _resolution; x++)
        //        {
        //            for (int y = 0; y < _resolution; y++)
        //            {
        //                if (y > (_resolution / 3) * 2)
        //                    _texture.SetPixel(x, y, Color.white);
        //                else if (y < _resolution / 3)
        //                    _texture.SetPixel(x, y, Color.red);
        //                else
        //                    _texture.SetPixel(x, y, Color.blue);
        //            }
        //        }
        //    }
        //    else if (_country == Country.Ucraina)
        //    {
        //        for (int x = 0; x < _resolution; x++)
        //        {
        //            for (int y = 0; y < _resolution; y++)
        //            {
        //                if (y < _resolution / 2)
        //                    _texture.SetPixel(x, y, Color.yellow);
        //                else
        //                    _texture.SetPixel(x, y, Color.blue);
        //            }
        //        }
        //    }
        //}
        //else
        //{
        //    var randomValue = Random.value;
        //    for (int x = 0; x < _resolution; x++)
        //    {
        //        for (int y = 0; y < _resolution; y++)
        //        {
        //            _texture.SetPixel(x, y, new Color(1, 0, 0, 1));

        //            var x2 = Mathf.Pow((x + 0.5f - _center.x) * _step, 2);
        //            var y2 = Mathf.Pow((y + 0.5f - _center.y) * _step, 2);
        //            var outR2 = Mathf.Pow(_outRadius * _step, 2);
        //            var inR2 = Mathf.Pow(_inRadius * _step, 2);
        //            var result = x2 + y2;

        //            var interpolate = Mathf.InverseLerp(inR2, outR2, result);

        //            Color color = _gradient.Evaluate(interpolate);

        //            _texture.SetPixel(x, y, color);
        //        }
        //    }
        //}
        #endregion

        _material.mainTexture = _texture;
        _texture.Apply();
    }

    private void DrawQuad(int rayX, int rayY)
    {
        for (int y = 0; y < _brushSize; y++)
        {
            for (int x = 0; x < _brushSize; x++)
            {
                _texture.SetPixel(rayX + x - _brushSize / 2, rayY + y - _brushSize / 2, _color);
            }
        }
        _texture.Apply();
    }

    private void DrawCircle(int rayX, int rayY)
    {
        for (int y = 0; y < _brushSize; y++)
        {
            for (int x = 0; x < _brushSize; x++)
            {
                var x2 = Mathf.Pow(x - _brushSize / 2, 2);
                var y2 = Mathf.Pow(y - _brushSize / 2, 2);
                var r2 = Mathf.Pow(_brushSize / 2 - 0.5f, 2);
                if (x2 + y2 < r2)
                {
                    int resultX = rayX + x - _brushSize / 2;
                    int resultY = rayY + y - _brushSize / 2;


                    if (resultX >= 0 && resultX < _resolution && resultY >= 0 && resultY < _resolution)
                    {
                        Color oldColor = _texture.GetPixel(resultX, resultY);
                        Color newColor = Color.Lerp(oldColor, _color, _color.a);

                        _texture.SetPixel(resultX, resultY, newColor);
                    }
                }
            }
        }
        _texture.Apply();
    }
}
