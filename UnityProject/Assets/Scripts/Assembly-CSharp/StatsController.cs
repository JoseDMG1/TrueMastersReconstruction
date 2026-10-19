using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Readable reconstruction of native StatsController; no original C# method bodies survived.
public class StatsController : MonoBehaviour
{
    public GameObject healthbarOp;
    public GameObject healthbar;
    public GameObject lifePlayer;
    public GameObject lifeOp;
    public GameObject Shieldbar;
    public GameObject ShieldbarOp;
    private float shieldbarMax;
    public Image iconA;
    public Image iconB;
    public Image iconAop;
    public Image iconBop;
    public TMP_Text player1Name;
    public TMP_Text player2Name;
    public ParticleSystem healFxP1;
    public ParticleSystem healFxP2;
    public static StatsController i;

    private void Start()
    {
        if (i == null) i = this;
        else if (i != this) { Destroy(this); return; }
        SetPlayersName();
        shieldbarMax = 5f;
    }
    public void SetPlayersName()
    {
        if (GameController.i != null && GameController.i.online)
        {
            Debug.LogError("Online player identity requires the unreconstructed online provider.", this);
            return;
        }
        player1Name.text = "Player";
        player2Name.text = SystemManager.i != null && SystemManager.i.mode == GameMode.LocalOffline ? "Player 2" : "CPU";
    }
    public void LoseHealth(float amount, bool opponent = false)
    {
        var bar = opponent ? healthbarOp : healthbar;
        if (bar.transform.localScale.x == 0.05f) return;
        if (bar.transform.localScale.x < 0.6f) bar.GetComponent<Image>().color = new Color(1f, 0f, 0f, 0.8f);
        bar.transform.localScale = new Vector3(bar.transform.localScale.x - amount, 1f, 1f);
        if (bar.transform.localScale.x < 0.05f) bar.transform.localScale = new Vector3(0.05f, 1f, 1f);
    }
    public void SetHealth(float damage, bool opponent = false)
    {
        foreach (var crystal in LifeImages(opponent))
        {
            bool destroyed = (crystal.name == "3" && damage >= 1f)
                || (crystal.name == "2" && damage >= 5f)
                || (crystal.name == "1" && damage >= 10f);
            if (destroyed) SetCrystalDestroyed(crystal);
            bool flash = (crystal.name == "3" && damage < 1f)
                || (crystal.name == "2" && damage > 1f && damage < 5f)
                || (crystal.name == "1" && damage > 5f && damage < 10f);
            if (flash) StartCoroutine(Hit(crystal.gameObject));
        }
    }
    public void SetHealthLM(float playerHP, bool opponent = false)
    {
        foreach (var crystal in LifeImages(opponent))
        {
            if (playerHP == 20f)
            {
                crystal.tag = "Untagged";
                crystal.color = Color.white;
                continue;
            }
            bool flash = (crystal.name == "3" && playerHP > 15f)
                || (crystal.name == "2" && playerHP > 10f && playerHP < 15f)
                || (crystal.name == "1" && playerHP > 5f && playerHP <= 10f);
            if (flash) StartCoroutine(Hit(crystal.gameObject));
            bool destroyed = (crystal.name == "3" && playerHP <= 15f)
                || (crystal.name == "2" && playerHP <= 10f)
                || (crystal.name == "1" && playerHP < 5f);
            if (destroyed) SetCrystalDestroyed(crystal);
        }
    }
    private Image[] LifeImages(bool opponent) => (opponent ? lifeOp : lifePlayer).GetComponentsInChildren<Image>();
    private static void SetCrystalDestroyed(Image crystal)
    {
        crystal.tag = "Destroyed";
        crystal.color = new Color(0f, 0f, 0f, 0.4f);
    }
    public void RestoreHealth(bool opponent = false)
    {
        foreach (var crystal in LifeImages(opponent)) crystal.color = new Color(0f, 0f, 0f, 0.4f);
        StartCoroutine(RestoreHeal2(opponent));
    }
    private IEnumerator RestoreHeal2(bool opponent)
    {
        yield return new WaitForSeconds(0.75f);
        foreach (var crystal in LifeImages(opponent))
        {
            crystal.color = Color.white;
            crystal.tag = "Untagged";
        }
    }
    private IEnumerator Hit(GameObject crystalLife)
    {
        var image = crystalLife.GetComponent<Image>();
        image.color = new Color(0.7f, 0f, 0f, 1f);
        Color originalColor = Color.white;
        float t = 0f;
        while (t <= 1f)
        {
            t += Time.deltaTime * 2f;
            if (crystalLife.CompareTag("Destroyed")) originalColor = new Color(0f, 0f, 0f, 0.4f);
            image.color = Color.Lerp(image.color, originalColor, 0.06f);
            yield return new WaitForSeconds(0.02f);
        }
        if (crystalLife.CompareTag("Destroyed")) originalColor = new Color(0f, 0f, 0f, 0.4f);
        image.color = originalColor;
    }
    public IEnumerator GrassHealing(bool opponent)
    {
        var life = opponent ? lifeOp : lifePlayer;
        yield return new WaitForSeconds(1f);
        (opponent ? healFxP2 : healFxP1).Play();
        var children = life.GetComponentsInChildren<Image>();
        for (int cycle = 0; cycle < 5; cycle++)
        {
            foreach (var crystal in children)
            {
                crystal.color = new Color(0f, 0.7f, 0f, 1f);
                crystal.tag = "Untagged";
                float t = 0f;
                while (t <= 0.75f)
                {
                    t += Time.deltaTime * 2f;
                    crystal.color = Color.Lerp(crystal.color, Color.white, 0.06f);
                    yield return new WaitForSeconds(0.02f);
                }
                crystal.color = Color.white;
            }
        }
    }
    public void ChangeColorIcon(int color, string button, bool opponent = false)
    {
        if (button == "A")
            (opponent ? iconAop : iconA).color = color != 0 ? new Color(0f, 0f, 0f, 0.7f) : new Color(1f, 1f, 1f, 0.9f);
        else if (button == "B")
        {
            (opponent ? iconBop : iconB).color = color != 0 ? new Color(0.7f, 0f, 0f, 1f) : Color.clear;
            if (color != 0) StartCoroutine(ChargeShieldBar(opponent));
        }
    }
    public void SetShieldBar(float shielDamage, bool opponent = false)
    {
        var bar = opponent ? ShieldbarOp : Shieldbar;
        bar.transform.localScale = new Vector3(shieldbarMax - Mathf.Min(shieldbarMax, shielDamage), 2f, 1f);
    }
    public IEnumerator ChargeShieldBar(bool opponent = false)
    {
        var bar = opponent ? ShieldbarOp : Shieldbar;
        var image = bar.GetComponent<Image>();
        image.color = new Color(0.6f, 0f, 0f, 0.7f);
        while (bar.transform.localScale.x < shieldbarMax)
        {
            bar.transform.localScale += new Vector3(0.0105f, 0f, 0f);
            yield return new WaitForSeconds(0.02f);
        }
        image.color = new Color(0.3f, 0.5f, 0.3f, 0.8f);
    }
    private void Update() { }
    public int GetPlayerLifeCrystals(bool guest)
    {
        // Native online branch swaps perspective using OnlineManager.IsPlayer2.
        if (GameController.i != null && GameController.i.online)
            throw new System.InvalidOperationException("Online life-crystal perspective has not been reconstructed.");
        int count = 0;
        foreach (var crystal in LifeImages(guest)) if (!crystal.CompareTag("Destroyed")) count++;
        return count;
    }
}
