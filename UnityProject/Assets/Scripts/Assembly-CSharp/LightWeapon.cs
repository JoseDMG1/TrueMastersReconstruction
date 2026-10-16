using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Corrected native VA 0x12422D8..0x1243020. See elemental-owners-reconstruction.md.
public class LightWeapon : MonoBehaviour
{
    public List<Material> sword;
    public List<Material> shield;
    public List<SkinnedMeshRenderer> swordMesh;
    public List<SkinnedMeshRenderer> shieldMesh;
    public bool yellow;

    private void Start()
    {
        // Renderer materials own these runtime caches. The recovered selection
        // scene carries an obsolete null sword entry on Agata; appending would
        // retain it and send a null material to her taunt animation callback.
        // Rebuild once at the producing boundary instead of filtering consumers.
        sword = new List<Material>(swordMesh.Count);
        shield = new List<Material>(shieldMesh.Count);
        foreach (SkinnedMeshRenderer mesh in swordMesh) sword.Add(mesh.material);
        foreach (SkinnedMeshRenderer mesh in shieldMesh) shield.Add(mesh.material);
    }

    public void LightSword()
    {
        foreach (Material mat in sword) StartCoroutine(Light(mat, true));
    }

    public void LightSwordAndShield()
    {
        foreach (Material mat in sword) StartCoroutine(Light(mat, true));
        foreach (Material mat in shield) StartCoroutine(Light(mat, true));
    }

    public void LightShield()
    {
        foreach (Material mat in shield) StartCoroutine(Light(mat, false));
    }

    public void DarkShield()
    {
        foreach (Material mat in shield) StartCoroutine(Dark(mat));
    }

    public IEnumerator Light(Material mat, bool recover = true)
    {
        float duration = .2f;
        float t = 0f;
        while (t <= 1f)
        {
            t += Time.deltaTime / duration;
            // Yellow adds to the material base color, not its prior emission.
            Color emission = yellow ? mat.color + new Color(.1f, .1f, .05f, 1f)
                : new Color(0f, t * .5f, t, 1f);
            mat.SetColor("_EmissionColor", emission);
            yield return new WaitForSeconds(.02f);
        }
        if (recover) StartCoroutine(Dark(mat));
    }

    public IEnumerator Dark(Material mat)
    {
        yield return new WaitForSeconds(.2f);
        float duration = .2f;
        float t = 0f;
        while (t <= 1f)
        {
            t += Time.deltaTime / duration;
            mat.SetColor("_EmissionColor", new Color(1f - t, 1f - t, 1f - t, 1f));
            yield return new WaitForSeconds(.02f);
        }
    }

    private void Update() { }
}
