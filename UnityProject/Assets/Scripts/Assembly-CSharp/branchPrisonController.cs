using UnityEngine;

// Corrected ARM64 0x121ABAC..0x121B2AC. Projectile-owned trap lifecycle.
public class branchPrisonController : MonoBehaviour
{
    public RefHitCharacter hitCharacter;
    public PathController branch1;
    public PathController branch2;
    public BranchPrisonAnim branchAnim;
    public GameObject explotionPrefab;
    public float destroyByTime;
    public bool enemyTrapped;
    public AudioSource crushSound;
    private GameObject owner;
    private DamageOnCollision enemyDamageOwner;

    private void Start()
    {
        Invoke(nameof(DestroyInTime), destroyByTime);
        owner = GameController.i.GetAttackOwner(gameObject.layer);
        owner.GetComponent<BranchPrisonCharController>().actualPrisonAtk = this;
    }
    public void PlayCrushAnimation()
    {
        branchAnim.StartCoroutine(branchAnim.RetrieveAttack());
        crushSound.Play();
    }
    public void TrapEnemy()
    {
        enemyTrapped = true;
        if (owner.GetComponent<Animator>().GetBool("Weak")) return;
        owner.GetComponent<BranchPrisonCharController>().PlayCharCrushAnim();
    }
    public void DestroyOnEnemyHit()
    {
        var enemy = GameController.i.GetEnemyByOwnerLayer(gameObject.layer);
        if (enemy.GetComponent<Animator>().GetNextAnimatorStateInfo(0).IsName("PrisonFree")) return;
        Instantiate(explotionPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }
    public void AddListernerOnEnemyDamage()
    {
        enemyDamageOwner = GameController.i.GetEnemyByOwnerLayer(gameObject.layer).GetComponent<DamageOnCollision>();
        enemyDamageOwner.onDamageRecived.AddListener(DestroyOnEnemyHit);
    }
    public void PlayHideAnimation()
    {
        if (enemyTrapped) return;
        branch1.Stop();
        branch2.Stop();
        branchAnim.StartCoroutine(branchAnim.RetrieveAttack());
    }
    private void DestroyInTime()
    {
        if (hitCharacter.hitCharacter != null)
            Instantiate(explotionPrefab, transform.position, transform.rotation);
        Destroy(gameObject);
    }
    private void OnDestroy()
    {
        // Retain the registering event owner and remove exactly our callback.
        if (enemyDamageOwner != null) enemyDamageOwner.onDamageRecived.RemoveListener(DestroyOnEnemyHit);
        if (owner != null)
        {
            var controller = owner.GetComponent<BranchPrisonCharController>();
            if (controller != null && controller.actualPrisonAtk == this) controller.actualPrisonAtk = null;
        }
    }
}
