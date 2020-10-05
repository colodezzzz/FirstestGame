using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootMonster : Monster
{
    [SerializeField]
    private float rate = 2f;

    [SerializeField]
    private GameObject player = null;

    private Bullet bullet;
    private SpriteRenderer sprite;

    protected override void Awake()
    {
        bullet = Resources.Load<Bullet>("Bullet");
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    protected override void Start()
    {
        InvokeRepeating(nameof(Shoot), rate, rate);
    }

    protected override void Update()
    {
        if (transform.position.y <= player.transform.position.y && player.transform.position.y < player.transform.position.y + 1f)
        {
            sprite.flipX = player.transform.position.x - transform.position.x >= 0;
        }
    }

    private void Shoot()
    {
        Vector3 position = transform.position;

        position.y += 0.7f;

        Bullet newBullet = Instantiate<Bullet>(bullet, position, bullet.transform.rotation) as Bullet;

        newBullet.Parent = gameObject;
        newBullet.Direction = (player.transform.position.x - transform.position.x < 0 ? -1f : 1f) * newBullet.transform.right;
    }

    protected override void OnTriggerEnter2D(Collider2D collider)
    {
        Unit unit = collider.GetComponent<Unit>();

        if (unit && unit is Character)
        {
            if (Mathf.Abs(unit.transform.position.x - transform.position.x) < 0.5f)
            {
                RecieveDamage();
            }
            else
            {
                unit.RecieveDamage(unit.transform.position.x - transform.position.x);
            }
        }
    }
}
