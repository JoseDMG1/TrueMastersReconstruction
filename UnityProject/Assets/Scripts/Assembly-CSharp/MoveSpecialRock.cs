using UnityEngine;

public class MoveSpecialRock : MonoBehaviour
{
    public float delay = 1f;
    public float speed;
    private bool ready;
    private bool soundPlayed;
    private Vector3 dir;

    private void Start()
    {
        // Native 0x129D2CC inherits its fighter's layer through two parents.
        gameObject.layer = transform.parent.parent.gameObject.layer;
        Invoke(nameof(StartMoving), delay);
        GetComponent<Description>().Damage = 20f;
        GetComponent<Description>().Defense = 20f;
    }

    private void FixedUpdate()
    {
        ChangeDirection();
        if (!ready) return;
        // Native uses serialized displacement per physics tick, no deltaTime.
        transform.position += dir * speed;
        transform.GetChild(0).Rotate(4f, 4f, 0f);
        if (soundPlayed) return;
        GetComponent<AudioSource>().Play();
        soundPlayed = true;
        GetComponent<Description>().Damage = 2f;
        GetComponent<Description>().Defense = 1f;
        if (delay == 1.5f)
        {
            GetComponent<Description>().Damage = 3f;
            GetComponent<Description>().Defense = 2f;
        }
    }

    private void ChangeDirection()
    {
        if (ready) return;
        Transform target = gameObject.layer == 9 ? GuestPlayer.guestPlayer.transform
            : HostPlayer.hostPlayer.transform;
        dir = (target.position - transform.position).normalized;
        // Normalize in 3D first, then discard Y, matching native behavior.
        dir.y = 0f;
    }

    public void StartMoving()
    {
        ready = true;
        GetComponent<SummonOnCollision>().enabled = true;
    }
}
