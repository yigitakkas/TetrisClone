using UnityEngine;
using UnityEngine.UI;
using System.Collections;
[RequireComponent(typeof(MaskableGraphic))]

public class ScreenFader : MonoBehaviour
{
    public float StartAlpha = 1f;
    public float TargetAlpha = 0f;
    public float Delay = 0f;
    public float TimeToFade = 1f;

    private float _inc;
    private float _currentAlpha;
    private MaskableGraphic _graphic;
    private Color _originalColor;

    // Use this for initialization
    void Start()
    {
        _graphic = GetComponent<MaskableGraphic>();

        _originalColor = _graphic.color;

        _currentAlpha = StartAlpha;

        Color tempColor = new Color(_originalColor.r, _originalColor.g, _originalColor.b, _currentAlpha);

        _graphic.color = tempColor;

        _inc = ((TargetAlpha - StartAlpha) / TimeToFade) * Time.deltaTime;

        StartCoroutine(FadeRoutine());
    }

    private IEnumerator FadeRoutine()
    {
        yield return new WaitForSeconds(Delay);

        while ((_inc > 0 && _currentAlpha < TargetAlpha) || (_inc < 0 && _currentAlpha > TargetAlpha))
        {
            _currentAlpha += _inc;
            Color tempColor = new Color(_originalColor.r, _originalColor.g, _originalColor.b, _currentAlpha);
            _graphic.color = tempColor;
            yield return null;
        }

        _currentAlpha = TargetAlpha;
        Color finalColor = new Color(_originalColor.r, _originalColor.g, _originalColor.b, _currentAlpha);
        _graphic.color = finalColor;
    }
}
