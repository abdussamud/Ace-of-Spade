using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class AlphaExtensions
{
    public static void Fade(this SpriteRenderer renderer, float alpha)
    {
        Color color = renderer.color;
        color.a = alpha;
        renderer.color = color;
    }

    public static void Fade(this Image image, float alpha)
    {
        Color color = image.color;
        color.a = alpha;
        image.color = color;
    }

    public static T GetRandom<T>(this IList<T> ts, int initialInclusive = 0, int finalExclusive = 0)
    {
        if (finalExclusive == 0) { finalExclusive = ts.Count; }
        return ts[UnityEngine.Random.Range(initialInclusive, finalExclusive)];
    }

    public static T GetRandom<T>(this T[] ts, int initialInclusive = 0, int finalExclusive = 0)
    {
        if (finalExclusive == 0) { finalExclusive = ts.Length; }
        return ts[UnityEngine.Random.Range(initialInclusive, finalExclusive)];
    }

    public static void DestroyChildren(this Transform t)
    {
        foreach (Transform child in t) { UnityEngine.Object.Destroy(child.gameObject); }
    }

    public static void SetLayersRecursively(this GameObject gameObject, int layer)
    {
        gameObject.layer = layer;
        foreach (Transform t in gameObject.transform)
        {
            t.gameObject.SetLayersRecursively(layer);
        }
    }

    public static Vector2 ToV2(this Vector3 input) => new(input.x, input.y);

    public static Vector3 Flat(this Vector3 input) => new(input.x, 0, input.z);

    public static Vector3Int ToVector3Int(this Vector3 vec3) => new((int)vec3.x, (int)vec3.y, (int)vec3.z);
}
