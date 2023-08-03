using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PersonajeController : MonoBehaviour
{
    public float velMovement = 5f; // Velocidad de movimiento del personaje
    public float fuerzaJump = 7f; //Fuerza dle salto dle personaje 
    private bool enElsuelo = false; //Indicador si el personaje est� en el suelo
    private bool mirandoDerecha = true;//Indica el sentido donde mira el personaje
    
    private SpriteRenderer spriteRenderer;
    private Rigidbody2D rb;
    private Animator animator;
    private float velActual;
    private float jumpActual;

    public Vector2 velocidadRebote;


    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        //spriteRenderer = GetComponent<SpriteRenderer>();
    }


    void Update()
    {

        //movimiento horizonal
        float movimientoH = Input.GetAxis("Horizontal");

        //sentido personaje
        if(movimientoH > 0 && !mirandoDerecha){
            Girar();
        }

        else if(movimientoH < 0 && mirandoDerecha){
            Girar();
        }

        rb.velocity = new Vector2(movimientoH * velMovement, rb.velocity.y);

        animator.SetFloat("Horizontal", Mathf.Abs(movimientoH));
        animator.SetBool("enSuelo", enElsuelo);
        animator.SetFloat("VelocidadY", rb.velocity.y);

        //Salto
        if (Input.GetKey(KeyCode.UpArrow) && enElsuelo)
        {
            //rb.AddForce(new Vector2(0f, fuerzaJump));
            rb.velocity = new Vector2(rb.velocity.x, +fuerzaJump);
            enElsuelo = false;
            AudioManager.Instance.PlaySFX("jump");
        }

    }

    void OnCollisionEnter2D (Collision2D collision)
    {
        //Verificar si el personaje est� en el suelo
        if (collision.gameObject.CompareTag("Suelo") || collision.gameObject.CompareTag("PlataformaC") || collision.gameObject.CompareTag("PlataformaM"))
        {
            enElsuelo = true;
            Debug.Log("Si toco el suelo");
        }

        if (collision.gameObject.CompareTag("PlataformaM"))
        {
            transform.parent = collision.transform;
        }
    }

    private void OnCollisionExit2D (Collision2D collision)
    {
        if(collision.gameObject.CompareTag("PlataformaM"))
        {
            transform.parent = null;
        }
    }

    private void Girar()
    {
        mirandoDerecha = !mirandoDerecha;
        transform.eulerAngles = new Vector3(0, transform.eulerAngles.y + 180, 0);
    }

    public void Rebote(Vector2 puntoGolpe, float direccion)
    {
        //rb.velocity = new Vector2(velocidadRebote.x * puntoGolpe.x * direccion, velocidadRebote.y);
    }

    public void superVelocidad(float velM, float tiempoPoder)
    {
        StartCoroutine(velocidad(velM, tiempoPoder));
    }

    public void superSalto(float fuerzaJ, float tiempoPoder)
    {
        StartCoroutine(salto(fuerzaJ, tiempoPoder));
    }

    private IEnumerator velocidad(float velM, float tiempoPoder)
    {
        velActual = velMovement;
        velMovement = velM;
        yield return new WaitForSeconds(tiempoPoder);
        velMovement = velActual;
    }

    private IEnumerator salto(float fuerzaJ, float tiempoPoder)
    {
        jumpActual = fuerzaJump;
        fuerzaJump = fuerzaJ;
        yield return new WaitForSeconds(tiempoPoder);
        fuerzaJump = jumpActual;
    }

}

