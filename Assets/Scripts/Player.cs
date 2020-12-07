using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float movementSpeed = 4;
    private int direction  = 1; 
    [Space]
    [Header("Jump")]
    public float jumpForce = 500;
    public bool grounded;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask whatIsGround;
    [Space]
    [Header("Shooting")]
    public GameObject missilePrefab;
    public Transform shootingPositionRight;
    public Transform shootingPositionLeft;
    public float missileSpeed = 5;
    [Space]
    [Header("Health")]
    public int playerHp = 3 ;
    public bool isAlive = true;
    public Slider hpBar;

    public Transform spawnPoint;

    //REFERENCES
    private Rigidbody2D rig;
    private Animator anim;
    private SpriteRenderer spr;

    // Start is called before the first frame update
    void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        spr = GetComponent<SpriteRenderer>();
        hpBar.maxValue = playerHp;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        hpBar.value = playerHp;
        if(isAlive){
            float h = Input.GetAxis("Horizontal");

            rig.velocity = new Vector2(movementSpeed*h, rig.velocity.y);
            anim.SetFloat("speed", Mathf.Abs(h)); //ignora o sinal

            if(h > 0) Flip(true);
            else if (h < 0)Flip(false);

            grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

            anim.SetBool("grounded", grounded);

            if(Input.GetButton("Jump") && grounded){
                rig.AddForce(Vector2.up *jumpForce);
            }
        }
    }

    void Update(){
                    if(Input.GetKeyDown(KeyCode.E)){
                anim.SetTrigger("shooting");
                GameObject missile;
                if(direction == 1){
                    missile = Instantiate(missilePrefab, shootingPositionRight.position, Quaternion.identity);
                }else{
                    missile = Instantiate(missilePrefab, shootingPositionLeft.position, Quaternion.identity);
                    missile.GetComponent<SpriteRenderer>().flipX = true;
                }
                missile.GetComponent<Rigidbody2D>().velocity = new Vector2(missileSpeed*direction, 0);
            }
    }
    

    void Flip(bool faceRight){
        if(faceRight){
            direction = 1;
            spr.flipX = false;
        }else{
            direction = -1;
            spr.flipX = true;
        }
    }

    public void TakeDamage(int damage){
        playerHp -=damage;

        if(playerHp <= 0 && isAlive){
            anim.SetTrigger("dead");
            isAlive = false;
            SceneManager.LoadScene(2);
            // Invoke("DestroyPlayer", 1.5f);
        }
    }

    // void DestroyPlayer(){
    //     gameObject.SetActive(false);
    //     Invoke("RespawnPlayer", 1);
    // }

    // void RespawnPlayer(){
    //     transform.position = spawnPoint.position;
    //     gameObject.SetActive(true);
    //     isAlive = true;
    //     playerHp = 3;
    // }

    void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.tag == "CheckPoint"){
            spawnPoint.position = other.transform.position;
            Destroy(other.gameObject);
        }
        // if(other.gameObject.tag == "Respawn"){
        //     RespawnPlayer();
        // }
        if(other.gameObject.tag == "Heal") {
            TakeDamage(-2);
            Destroy(other.gameObject);
        }
    }
}
