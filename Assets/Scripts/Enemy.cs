using UnityEngine;

public class Enemy : MonoBehaviour {

    public float speed = 10f;
    public float health = 100f;
    public int goldDrop = 50;
    public GameObject deathEffect;

    private Transform target;
    private int waypointIndex = 0;

    private void Start()
    {
        target = Waypoints.points[0];
    }

    private void Update()
    {
        Vector3 direction = target.position - transform.position;
        transform.Translate(direction.normalized *
            speed * Time.deltaTime, Space.World);

        if (Vector3.Distance(transform.position,
            target.position) <= 0.2f)
            GetNextWaypoint();
    }

    private void GetNextWaypoint()
    {
        if (waypointIndex >= Waypoints.points.Length - 1)
        {
            Destroy(gameObject);
            PlayerStats.Lives--;
            return;
        }

        waypointIndex++;
        target = Waypoints.points[waypointIndex];
    }

    private void ShowDeathAnimation()
    {
        GameObject effect = Instantiate(deathEffect, 
            transform.position, Quaternion.identity);

        Destroy(effect, 5f);
        Destroy(gameObject);
        PlayerStats.Money += goldDrop;
    }

    public float Health
    {
        get { return health;  }
        set {
            health = value;
            if (health <= 0)
                ShowDeathAnimation();
        }
    }
}