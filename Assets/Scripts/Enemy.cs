using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 6;
    public int enemyHP = 2;
    public int enemyDamage = 1;
    public float attackDistance = 2;

    private Rigidbody2D rig;
    private Animator anim;
    private SpriteRenderer spr;
    public Transform playerPosition;
    private float distance;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spr = GetComponent<SpriteRenderer>();
        playerPosition = GameObject.FindWithTag("Player").transform;
    }

    void FixedUpdate(){
        rig.velocity = new Vector2(moveSpeed, rig.velocity.y);
        anim.SetFloat("speed", Mathf.Abs(rig.velocity.x));
    }

    // Update is called once per frame
    void Update()
    {
        distance = Vector2.Distance(transform.position, playerPosition.position);

        if(distance <= attackDistance){
            Attack();
        }
    }

    void Attack(){
        anim.SetTrigger("attack");
    }
    public void EnemyDamage(){
        if(distance <= attackDistance){
            playerPosition.GetComponent<Player>().TakeDamage(enemyDamage);
        }
    }

    public void TakeDamage(int damage){
        enemyHP -= damage;
        if(enemyHP<=0){
            Destroy(gameObject);
        }
    }
    
    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "Limiter"){
            moveSpeed *= -1;
            if(moveSpeed > 0){
                spr.flipX = false;
            }else
            {
                spr.flipX = true;
            }
        }
    }

    void OnCollisionEnter2D(Collision2D other){
        if(other.gameObject.tag == "Missile"){
            TakeDamage(1);
            Destroy(other.gameObject);
        }
    }
}
