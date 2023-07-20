using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour
{
    public Projectile laserPrefab;
    public float speed = 5.0f;
    public int maxLaserCount = 30;
    private int currentLaserCount;
    private bool _laserActive;
    public Text LaserCountText;
    private Vector3 initialPosition;

    public Bunker bunker1;
    public Bunker bunker2;
    public Bunker bunker3;
    public Bunker bunker4;
    
    private Invaders invaders;
    public GameObject invadersObject;
    private SignGame sign;
    

    private void Start()
    {
        currentLaserCount = maxLaserCount;
        Invaders.invaderKilled += IncrementLaserCount;
        initialPosition = transform.position;
        invaders = invadersObject.GetComponent<Invaders>();
        UpdateLaserCountText();
        SignGame.OnCorrectWordReceived += IncreaseLaserCountByAmount;
        sign = FindObjectOfType<SignGame>();
    }
 
    private void Update()
    {
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);
        
        UpdateLaserCountText();

        //Debug.Log(this.transform.position);
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)){
            if(this.transform.position.x >= -14.0f){
               this.transform.position += Vector3.left * this.speed * Time.deltaTime;
            }
        } 
        else if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)){
            if(this.transform.position.x <= 14.0f){
                this.transform.position += Vector3.right * this.speed * Time.deltaTime;
            }
        }

        if(Input.GetKey(KeyCode.F)){
            Debug.Log("entering minigame");
            sign.StartGame();
        }

        if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0)){
            if (currentLaserCount > 0 && !_laserActive)
            {
                Shoot();
                currentLaserCount--;
            }
        }
        
    }
    
    private void Shoot(){
        if(!_laserActive){
            Projectile projectile = Instantiate(this.laserPrefab, this.transform.position, Quaternion.identity);
            projectile.destroyed += LaserDestroyed;
            _laserActive = true;
        }
    }

    private void LaserDestroyed()
    {
        _laserActive = false;
    }

    private void IncreaseLaserCountByAmount(int amount)
    {
        currentLaserCount += amount;
        UnityMainThreadDispatcher.Instance().Enqueue(() =>
        {
            UpdateLaserCountText(); // Update the display to show the new value.
        });    
    }

    private void UpdateLaserCountText()
    {
        LaserCountText.text = "Total Lasers: " + currentLaserCount.ToString();
    }

    private void OnTriggerEnter2D(Collider2D other){
        if(other.gameObject.layer == LayerMask.NameToLayer("Invader") 
        || other.gameObject.layer == LayerMask.NameToLayer("Missile")){
            invaders.ResetScene();

            bunker1.ResetScene();
            bunker2.ResetScene();
            bunker3.ResetScene();
            bunker4.ResetScene();
            
            ResetScene();
        }
    }

    private void IncrementLaserCount()
    {
        currentLaserCount++;
    }

    
     private void OnDestroy()
    {
        Invaders.invaderKilled -= IncrementLaserCount; 
        //SignGame.OnCorrectWordReceived -= IncreaseLaserCountByAmount; 

    }

    private void ResetScene(){
        currentLaserCount = maxLaserCount;
        transform.position = initialPosition;
    }
}
