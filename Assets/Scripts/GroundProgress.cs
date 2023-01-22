using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundProgress : MonoBehaviour
{
    [SerializeField]
    SpriteRenderer sprite;

    [SerializeField]
    AnimationCurve curve;

    [SerializeField]
    Ground ground;

    // Start is called before the first frame update
    void Start()
    {
        ground.OnProgressUpdate.AddListener(UpdateAlpha);
    }

    void SetAlpha(float alpha)
    {
        var color = sprite.color;
        color.a = alpha;
        sprite.color = color;
    }

    void UpdateAlpha(float progress)
    {
        SetAlpha(curve.Evaluate(progress / 100.0f));
    }
}
