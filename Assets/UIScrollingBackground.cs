using UnityEngine;
using UnityEngine.UI;

public class UIScrollingBackground : MonoBehaviour
{
    [SerializeField] private RawImage background;
    [SerializeField] private Vector2 speed = new Vector2(-0.05f, -0.05f);

    void Update()
    {
        if (background == null || background.texture == null) return;
        var r = background.uvRect;
        r.x += speed.x * Time.unscaledDeltaTime;
        r.y += speed.y * Time.unscaledDeltaTime;
        background.uvRect = r;
    }
}
