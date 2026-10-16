using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SpriteUIAnimation : MonoBehaviour
{
    public Image m_Image;
    public Sprite[] m_SpriteArray;
    public float m_Speed = 0.02f;
    private int m_IndexSprite;
    private Coroutine m_CorotineAnim;
    private bool IsDone;
    public bool playOnEnable;

    private void OnEnable() { if (playOnEnable) Func_PlayUIAnim(); }
    private void OnDisable() { Func_StopUIAnim(); }
    public void Func_PlayUIAnim()
    {
        IsDone = false;
        // Single owned loop replaces the native chain of one-frame coroutines.
        // Repeated play cannot orphan a previous loop.
        if (m_CorotineAnim != null) StopCoroutine(m_CorotineAnim);
        m_CorotineAnim = StartCoroutine(Func_PlayAnimUI());
    }
    public void Func_StopUIAnim()
    {
        IsDone = true;
        if (m_CorotineAnim != null) StopCoroutine(m_CorotineAnim);
        m_CorotineAnim = null;
    }
    private IEnumerator Func_PlayAnimUI()
    {
        // MoveNext 0x12C1BF4: wait first, then wrap/set/increment sprite.
        do
        {
            yield return new WaitForSeconds(m_Speed);
            if (m_IndexSprite >= m_SpriteArray.Length) m_IndexSprite = 0;
            m_Image.sprite = m_SpriteArray[m_IndexSprite++];
        } while (!IsDone);
        m_CorotineAnim = null;
    }
}
