using UnityEngine;
using UnityEngine.UI;

public class PlayerMove : MonoBehaviour
{


    public float runSpeed = 7; // controlar velocidad alo correr
    public float rotationSpeed = 250; // velocidad de rotacion
    public Animator animator;

    private float x, y;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public Rigidbody rg;
    public float fuerzaSalto = 8f;
    public bool puedoSaltar;
    public bool roundhouseKick;

    public float interactionDistance = 2f; // Distancia máxima de interacción



    void Start()
    {
        puedoSaltar = false;
        roundhouseKick = false;
        animator = GetComponent<Animator>();
    }

    void FixedUpdate()
    {
        // Aplicamos la rotación solo al eje Y para que gire sobre su eje vertical.
        transform.Rotate(0, x * Time.deltaTime * rotationSpeed, 0);

        // Movemos al jugador en la dirección que está mirando (usamos `transform.forward` para el movimiento hacia adelante/atrás)
        Vector3 moveDirection = transform.forward * y + transform.right * x;
        rg.MovePosition(transform.position + moveDirection * Time.deltaTime * runSpeed);
    }


    // Update is called once per frame
    void Update()
    {

        x = Input.GetAxis("Horizontal");
        y = Input.GetAxis("Vertical");

        animator.SetFloat("VelX", x);
        animator.SetFloat("VelY", y);

        if (puedoSaltar)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                animator.SetBool("salte", true);
                rg.AddForce(new Vector3(0, fuerzaSalto, 0), ForceMode.Impulse);
            }
            animator.SetBool("TocoSuelo", true);
        }
        else
        {
            EstoyCayendo();
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            // comentario patada
            // Debug.Log("patada");
            animator.SetBool("roundhouseKick", true);
            Invoke("ResetKick", 0.5f);
        }




    }

    public void EstoyCayendo()
    {
        animator.SetBool("TocoSuelo", false);
        animator.SetBool("salte", false);
    }

    public void ResetKick()
    {
        animator.SetBool("EndKick", true);
        animator.SetBool("roundhouseKick", false);
    }



}
