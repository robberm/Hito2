using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float h; //Horizontal
    public float speed = 10;
    public float jump = 10;
    bool isJumping = false;
    private int saltosquellevas = 0;
    private int saltosadicionales = 1;
    public BoxCollider2D bc;
    private bool facingRight = true;
    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;
    public float StartDashTime;
    public float dashSpeed;
    private float DashTime;
    private bool mR, mL, mU, mD;
    






    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        DashTime = StartDashTime;
    }


    // Update is called once per frame
    void Update()
    { //Si apretas dcha o izqda añada fuerzas.
        h = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(h * speed, rb.velocity.y);

        //DIRECCIONES
        if (Input.GetKeyDown(KeyCode.A))
        {
            mR = false;
            mL = true;
            mU = false;
            mD = false;
        }
        else if (Input.GetKeyDown(KeyCode.W))
        {
            mR = false;
            mL = false;
            mU = true;
            mD = false;
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {

            mR = true;
            mL = false;
            mU = false;
            mD = false;
        }
        else if (Input.GetKeyDown(KeyCode.S))
        {
            mR = false;
            mL = false;
            mU = false;
            mD = true;
        }



        Jump();
        BetterJump();
        FlipFace();
        Dash(mL, mR, mU, mD, rb.velocity);


        //No es universal, deberia ser que ese Keycode sea a eleccion del jugador




    }

    void Jump()
    {



        if (Input.GetKeyDown(KeyCode.Space))
        {

            if (saltosquellevas <= saltosadicionales)
            {
                isJumping = true;
                //rb.velocity = new Vector2( rb.velocity.x, jump);//
                rb.velocity = Vector2.up * jump;
                saltosquellevas++;

            }
        }
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            saltosquellevas = 0;
            isJumping = false;
        }
    }
    void BetterJump()
    {

        if (rb.velocity.y < 0)
        {


            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
        }
        if (rb.velocity.y > 0 && !Input.GetKey(KeyCode.Space))
        {
            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    void FlipFace()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            facingRight = false;

        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            facingRight = true;

        }

        float xhorizontal = gameObject.transform.localScale.x;
        if (facingRight)
        {

            gameObject.transform.localScale = new Vector3(Mathf.Abs(xhorizontal), gameObject.transform.localScale.y, 1f);


        }
        else if (!facingRight)
        {
            gameObject.transform.localScale = new Vector3(-Mathf.Abs(xhorizontal), gameObject.transform.localScale.y, 1f);

        }


    }

    void Dash(bool movingDirectionLeft, bool movingDirectionRight, bool movingDirectionUp, bool movingDirectionDown, Vector2 velocidadnormal)
    {
       
        
        if (DashTime <= 0)
        {
            DashTime = StartDashTime;
            rb.velocity = velocidadnormal;
            gameObject.GetComponent<ParticleSystem>().Stop();
        }
        else{
            
            if (Input.GetKeyDown(KeyCode.Q))
            {
                gameObject.GetComponent<ParticleSystem>().Play();
                if (movingDirectionLeft)
                {
                    ;
                    rb.velocity = Vector2.left * dashSpeed;
                }
                else if (movingDirectionRight)
                {
                    rb.velocity = Vector2.right * dashSpeed;

                }
                else if (movingDirectionDown)
                {
                    rb.velocity = Vector2.down * dashSpeed;
                }
                else if (movingDirectionUp)
                {
                    rb.velocity = Vector2.up * dashSpeed;
                }
            }
            DashTime -= Time.deltaTime;




        }
        }
    }

