using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RubyController : MonoBehaviour
{
    private float speed;

    public int maxHealth = 5;

    public GameObject projectilePrefab;
    public GameObject DamageeffectPrefab;
    public GameObject HealthpickupeffectPrefab;

    public int health { get { return currentHealth; } }
    int currentHealth;

    public float timeInvincible = 2.0f;
    public AudioClip throwCog;
    public AudioClip Damage;
    bool isInvincible;
    float invincibleTimer;

    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;

    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);
    public Text winlose;
    public Text winlose2;
    public Text Scoreboard;
    public Text Cog_amount;
    public int Cogs = 4;
    public int Score = 0;
    bool movement = true;
    public AudioSource musicSource;
    public AudioClip background;
    public AudioClip victory;
    public AudioClip defeat;
    public AudioClip talk;
    public AudioClip AmmoCollect;

    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        Scoreboard.text = Score.ToString();
        speed = 4.0f;
        Cogs = 4;
        Cog_amount.text = Cogs.ToString();
        currentHealth = maxHealth;
        audioSource = GetComponent<AudioSource>();
        winlose.text = ("");
        winlose2.text = ("");
        musicSource.clip = background;
        musicSource.Play();

    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    // Update is called once per frame
    void Update()
    {
        if (movement == true)
        {
            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");

            Vector2 move = new Vector2(horizontal, vertical);
            if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
            {
                lookDirection.Set(move.x, move.y);
                lookDirection.Normalize();
            }
            animator.SetFloat("Look X", lookDirection.x);
            animator.SetFloat("Look Y", lookDirection.y);
            animator.SetFloat("Speed", move.magnitude);
        } 



        if (Score == 4)
        {
            winlose.text = ("You Win! Created by Gage Langlais talk to jambi to move on to the next level");
            musicSource.clip = victory;
            musicSource.Play();
        }

        if (currentHealth <= 0)
        {
            winlose.text = ("You lost! Press R to restart");
            speed = 0f;
            musicSource.clip = defeat;
            musicSource.Play();
            Cogs = 0;
            Cog_amount.text = Cogs.ToString();
            if (Input.GetKey(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                movement = false;
            }
        }

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            if (Cogs >= 1)
                Launch();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                audioSource.PlayOneShot(talk);
                if (character != null)
                {
                    character.DisplayDialog();
                }
            }
            if (Score == 4)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
                if (Score == 4)
                {
                    winlose2.text = ("You Win! Created By Gage Langlais");
                    musicSource.clip = victory;
                    musicSource.Play();
                }
            }
        }

    }

    void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;

        rigidbody2d.MovePosition(position);
    }

    public void Invincible(int amount)
        {
            if (amount < 0)
            {
                if (isInvincible)
                    return;

                isInvincible = true;
                invincibleTimer = timeInvincible;
                animator.SetTrigger("Hit");
            }
        }

    public void ChangeHealth(int amount)
    {
        if (amount < 0)
        {
            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;
            GameObject projectileObject = Instantiate(DamageeffectPrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
            animator.SetTrigger("Hit");
            audioSource.PlayOneShot(Damage);
        }

        if (amount > 0)
        {
            GameObject projectileObject = Instantiate(HealthpickupeffectPrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);
        }

        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth);
    }

    public void SpeedHazard(float amount)
    {
        speed = 0.5f;
    }

    public void NormalSpeed(float amount)
    {
        speed = 4.0f;
    }

        public void FastSpeed(float amount)
    {
        speed = 7.0f;
    }

    public void addscore()
    {
        Score++;
        Scoreboard.text = Score.ToString();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Cogs")
        {
            Cogs += 4;
            Cog_amount.text = Cogs.ToString();
            Destroy(collision.collider.gameObject);
            audioSource.PlayOneShot(AmmoCollect);
        }

    }

    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);
        audioSource.PlayOneShot(throwCog);
        animator.SetTrigger("Launch");
        Cogs -= 1;
        Cog_amount.text = Cogs.ToString();
    }

}