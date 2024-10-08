using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class AnimateImageSprites : MonoBehaviour
{
    // ---- / Serialized Variables / ---- //
    [SerializeField] private Sprite usedLeftClick;
    [SerializeField] private Sprite blinkLeftClick;
    [SerializeField] private float timePeriod = 1f;
    
    // ---- / Private Variables / ---- //
    private Camera _camera;
    private Image _image;
    
    private Coroutine _pingPongCoroutine;

    private void Start()
    {
        _camera = Camera.main;
        _image = GetComponentInChildren<Image>();

        _pingPongCoroutine = StartCoroutine(PingPongSprite());
    }

    private IEnumerator PingPongSprite()
    {
        while (true)
        {
            _image.sprite = usedLeftClick;
            yield return new WaitForSeconds(timePeriod);

            _image.sprite = blinkLeftClick;
            yield return new WaitForSeconds(timePeriod);
        }
    }

    private void OnDisable()
    {
        if (_pingPongCoroutine != null)
        {
            StopCoroutine(_pingPongCoroutine);
        }
    }
}
