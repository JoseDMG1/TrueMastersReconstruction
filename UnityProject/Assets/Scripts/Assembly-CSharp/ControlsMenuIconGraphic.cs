using UnityEngine;
using UnityEngine.UI;

/// <summary>Resolution independent gamepad badge for the desktop main menu.</summary>
public sealed class ControlsMenuIconGraphic : MaskableGraphic
{
    private static readonly Vector2[] Body = {
        new Vector2(-.21f, .18f), new Vector2(-.13f, .22f),
        new Vector2(.13f, .22f), new Vector2(.21f, .18f),
        new Vector2(.32f, -.12f), new Vector2(.28f, -.25f),
        new Vector2(.20f, -.28f), new Vector2(.10f, -.12f),
        new Vector2(-.10f, -.12f), new Vector2(-.20f, -.28f),
        new Vector2(-.28f, -.25f), new Vector2(-.32f, -.12f)
    };

    protected override void OnPopulateMesh(VertexHelper mesh)
    {
        mesh.Clear();
        Rect rect = GetPixelAdjustedRect();
        Vector2 center = rect.center;
        float size = Mathf.Min(rect.width, rect.height);
        Color foreground = Color.white * color;
        Disc(mesh, center, size * .48f, new Color(.02f, .025f, .035f, .84f) * color);
        Ring(mesh, center, size * .47f, size * .012f, new Color(1, 1, 1, .24f) * color);
        for (int i = 0; i < Body.Length; i++)
            Stroke(mesh, center + Body[i] * size, center + Body[(i + 1) % Body.Length] * size, size * .028f, foreground);

        Vector2 dpad = center + new Vector2(-.14f, .025f) * size;
        Stroke(mesh, dpad + Vector2.left * size * .066f, dpad + Vector2.right * size * .066f, size * .03f, foreground);
        Stroke(mesh, dpad + Vector2.down * size * .066f, dpad + Vector2.up * size * .066f, size * .03f, foreground);
        Vector2 buttons = center + new Vector2(.15f, .025f) * size;
        foreach (Vector2 direction in new[] { Vector2.up, Vector2.right, Vector2.down, Vector2.left })
            Disc(mesh, buttons + direction * size * .052f, size * .021f, foreground);
        Ring(mesh, center + new Vector2(-.065f, -.09f) * size, size * .029f, size * .014f, foreground);
        Ring(mesh, center + new Vector2(.065f, -.09f) * size, size * .029f, size * .014f, foreground);
    }

    private static void Disc(VertexHelper mesh, Vector2 center, float radius, Color tint)
    {
        const int steps = 64;
        int start = mesh.currentVertCount;
        mesh.AddVert(center, tint, Vector2.zero);
        for (int i = 0; i <= steps; i++)
        {
            float angle = i * Mathf.PI * 2f / steps;
            mesh.AddVert(center + new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)) * radius, tint, Vector2.zero);
            if (i > 0) mesh.AddTriangle(start, start + i, start + i + 1);
        }
    }

    private static void Ring(VertexHelper mesh, Vector2 center, float radius, float width, Color tint)
    {
        const int steps = 64;
        for (int i = 0; i < steps; i++)
        {
            float a = i * Mathf.PI * 2f / steps, b = (i + 1) * Mathf.PI * 2f / steps;
            Vector2 first = new Vector2(Mathf.Cos(a), Mathf.Sin(a));
            Vector2 second = new Vector2(Mathf.Cos(b), Mathf.Sin(b));
            Quad(mesh, center + first * (radius - width * .5f), center + first * (radius + width * .5f),
                center + second * (radius + width * .5f), center + second * (radius - width * .5f), tint);
        }
    }

    private static void Stroke(VertexHelper mesh, Vector2 from, Vector2 to, float width, Color tint)
    {
        Vector2 delta = (to - from).normalized;
        Vector2 normal = new Vector2(-delta.y, delta.x) * width * .5f;
        Quad(mesh, from - normal, from + normal, to + normal, to - normal, tint);
    }

    private static void Quad(VertexHelper mesh, Vector2 a, Vector2 b, Vector2 c, Vector2 d, Color tint)
    {
        int start = mesh.currentVertCount;
        mesh.AddVert(a, tint, Vector2.zero); mesh.AddVert(b, tint, Vector2.zero);
        mesh.AddVert(c, tint, Vector2.zero); mesh.AddVert(d, tint, Vector2.zero);
        mesh.AddTriangle(start, start + 1, start + 2); mesh.AddTriangle(start, start + 2, start + 3);
    }
}
