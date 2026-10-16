using System.Collections;
using Photon.Pun;
using UnityEngine;
using UnityEngine.UI;

// Reconstructed from mapped ARM64 and IL2CPP metadata, not original C# source.
// Evidence/native-disassembly-SpecialAtk*.txt and special-atk-native-constants.json.
public class SpecialAtk : MonoBehaviour
{
    public static SpecialAtk specialAtk;
    public RectTransform charge;
    public Vector3 newCharge;
    public Vector3 lastCharge;
    public float speed = 0.1f; // Constructor native VA0x12B1384.
    public Text letter;
    public Image circle;
    public Image chargeImg;
    public Color sYellow;
    public Color sBlack;
    public bool charged;
    public bool soundPlayed;
    public int numberOfSpecial = 1;
    public bool decharging;

    // Native VA0x12B0F04.
    private void Start()
    {
        specialAtk = this;
    }

    // Native VA0x12B0F50. Original clears this static field unconditionally.
    private void OnDestroy()
    {
        specialAtk = null;
    }

    // Original FixedUpdate at VA0x12B0F98 is only RET and has been removed.

    // Native VA0x12B0F9C. Readiness is checked before adding this invocation's charge.
    public void ChargeSpecial(float val)
    {
        if (decharging)
            return;

        if (!(chargeImg.fillAmount < 0.9f) || charged)
        {
            charged = true;
            letter.color = Color.black;
            circle.color = Color.black;
            chargeImg.fillAmount = 1f;
            if (!soundPlayed)
            {
                GetComponent<AudioSource>().Play();
                soundPlayed = true;
            }
        }
        else
        {
            chargeImg.fillAmount += val / numberOfSpecial;
        }
    }

    // Native VA0x12B10D0. sBlack is serialized per prefab and is not replaced with Color.black.
    public void ResetCharge()
    {
        chargeImg.fillAmount = 0f;
        letter.color = sBlack;
        circle.color = sBlack;
        GameController.i.animMainChar.SetInteger("NumSpecials", 0);
        letter.text = "X";
        charged = false;
        soundPlayed = false;
    }

    // Factory VA0x12B11D8; original MoveNext VA0x12B161C.
    // LM intentionally has no decharging/charged/sound/aura cleanup in the original.
    public IEnumerator DechargeSpecialLM(float time)
    {
        chargeImg.fillAmount = 1f;
        while (chargeImg.fillAmount > 0f)
        {
            chargeImg.fillAmount += -0.034f / time;
            yield return new WaitForSeconds(0.04f);
        }
    }

    // Factory VA0x12B1270; original MoveNext VA0x12B13A4.
    public IEnumerator DechargeSpecial(float time)
    {
        decharging = true;
        chargeImg.fillAmount = 1f;
        while (chargeImg.fillAmount > 0f)
        {
            chargeImg.fillAmount += -0.034f / time;
            yield return new WaitForSeconds(0.04f);
        }

        GameController.i.animMainChar.GetComponent<AuraController>().StopAura();
        charged = false;
        soundPlayed = false;
        decharging = false;
        if (SystemManager.i.mode != GameMode.Survival || PlayerPrefs.GetString("Character") != "Maya")
        {
            letter.text = "S";
            GameController.i.animMainChar.SetInteger("NumSpecials", 1);
        }
    }

    // Native VA0x12B1308. Retained RPC contract; this implementation only changes local state.
    [PunRPC]
    public void FinishEnemyAwakening()
    {
        GameController.i.OpponentCharacter.GetComponent<AnimatorStatesManager>().awakening = false;
    }
}

