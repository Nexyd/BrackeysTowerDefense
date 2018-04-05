using UnityEngine;

public class Bullet : MonoBehaviour
{
    private Transform target;

    public float speed = 70f;
    public float explosionRadius = 0f;
    public float damage = 50f;
    public GameObject impactEffect;

    // Update is called once per frame
    void Update()
    {
        if (target == null)
            return;

        Vector3 dir = target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
            HitTarget();

        transform.Translate(dir.normalized *
            distanceThisFrame, Space.World);

        transform.LookAt(target);
    }

    public void Seek(Transform target)
    {
        this.target = target;
    }

    void HitTarget()
    {
        GameObject effectObj = Instantiate(impactEffect,
            transform.position, transform.rotation);

        Destroy(effectObj, 5f);
        if (explosionRadius > 0f)
            Explode();
        else
            Damage(target);

        Destroy(gameObject);
    }

    void Damage(Transform enemyPos)
    {
        Enemy enemy = enemyPos
            .GetComponent<Enemy>();

        if (enemy != null)
            enemy.Health -= damage;
    }

    void Explode()
    {
        Collider[] colliders = Physics.OverlapSphere(
            transform.position, explosionRadius);

        foreach (Collider collider in colliders)
            if (collider.tag == "Enemy")
                Damage(collider.transform);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform
            .position, explosionRadius);
    }
}