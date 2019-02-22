using UnityEngine;
using System.Collections;
using BlackGardenStudios.HitboxStudioPro;

public class GuardScript : MonoBehaviour, ICharacter {

    [SerializeField] float      speed = 1.0f;
    [SerializeField] float      jumpForce = 4.0f;

    private float               inputX;
    private Animator            animator;
    private Rigidbody2D         body2d;
    private bool                combatIdle = false;
    private bool                isGrounded = true;

    #region copied ICharacter from Character stuff

    protected SpriteRenderer m_Renderer;
    [SerializeField]
    protected float m_BasePoise = 1f;
    protected float m_Poise;
    [SerializeField]
    protected SpritePalette m_ActivePalette;
    [SerializeField]
    protected SpritePaletteGroup m_PaletteGroup;
    protected bool LockFlip { get; set; }
    protected Color m_DefaultColor;


    public SpritePalette ActivePalette { get { return m_ActivePalette; } }
    public SpritePaletteGroup PaletteGroup { get { return m_PaletteGroup; } }

    public float Poise
    {
        get { return m_BasePoise + m_Poise; }
        set { m_Poise = value; }
    }


    public bool FlipX {
        get { return m_Renderer.flipX; }
        protected set { if (LockFlip == false) m_Renderer.flipX = value; }
    }


    private void Awake()
    {

        m_Renderer = GetComponent<SpriteRenderer>();
        m_DefaultColor = m_Renderer.color;

        SetPalette(m_ActivePalette);
    }

    public void SetPalette(SpritePalette palette)
    {
        m_ActivePalette = palette;

        if (palette != null)
        {
            var block = new MaterialPropertyBlock();

            if (m_Renderer == null) m_Renderer = GetComponent<SpriteRenderer>();
            m_Renderer.GetPropertyBlock(block);
            block.SetTexture("_SwapTex", palette.Texture);
            m_Renderer.SetPropertyBlock(block);
        }
    }
    #endregion

    // Use this for initialization
    void Start () {
        animator = GetComponent<Animator>();
        body2d = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
        // -- Handle input and movement --
        inputX = Input.GetAxis("Horizontal");

        // Swap direction of sprite depending on walk direction
        if (inputX > 0)
            transform.localScale = new Vector3(-1.0f, 1.0f, 1.0f);
        else if (inputX < 0)
            transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);

        // Move
        body2d.velocity = new Vector2(inputX * speed, body2d.velocity.y);

        // -- Handle Animations --
        isGrounded = IsGrounded();
        animator.SetBool("Grounded", isGrounded);

        //Death
        if (Input.GetKeyDown("k"))
            animator.SetTrigger("Death");
        //Hurt
        else if (Input.GetKeyDown("h"))
            animator.SetTrigger("Hurt");
        //Recover
        else if (Input.GetKeyDown("r"))
            animator.SetTrigger("Recover");
        //Change between idle and combat idle
        else if (Input.GetKeyDown("i"))
            combatIdle = !combatIdle;



        //Attack
        else if (Input.GetMouseButtonDown(0))
        {
            animator.SetTrigger("Attack");
        }

        //Jump
        else if (Input.GetKeyDown("space") && isGrounded)
        {
            animator.SetTrigger("Jump");
            body2d.velocity = new Vector2(body2d.velocity.x, jumpForce);
        }

        //Walk
        else if (Mathf.Abs(inputX) > Mathf.Epsilon && isGrounded)
            animator.SetInteger("AnimState", 2);
        //Combat idle
        else if (combatIdle)
            animator.SetInteger("AnimState", 1);
        //Idle
        else
            animator.SetInteger("AnimState", 0);
    }

    bool IsGrounded()
    {
        return Physics2D.Raycast(transform.position, -Vector3.up, 0.03f);
    }

    public void HitboxContact(ContactData data)
    {
        throw new System.NotImplementedException();
    }
}
