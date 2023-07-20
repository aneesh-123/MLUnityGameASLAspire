using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Invaders : MonoBehaviour
{

    public Invader[] prefabs;

    public int rows = 5;

    public int columns = 11;

    public AnimationCurve speed;
    public Projectile missilePrefab;
    
    public Bunker bunker1;
    public Bunker bunker2;
    public Bunker bunker3;
    public Bunker bunker4;

    public float missileAttackRate = 1.0f;

    public int amountKilled {get; private set;}
    public int amountAlive => this.totalInvaders - this.amountKilled;
    public int totalInvaders => this.rows*this.columns;
    public float percentKilled => (float) this.amountKilled / (float) totalInvaders;

    public static event System.Action invaderKilled;

    public Text amountKilledText;

    private Vector3 _direction = Vector2.right;

    private void Awake(){
        float width = 2.0f * (this.columns - 1);
        float height = 2.0f * (this.rows - 1);
        Vector2 centering = new Vector2(-width/2, -height/2);
        Debug.Log("centering " + centering);

        for(int row = 0; row < this.rows; row++){
            Vector3 rowPosition = new Vector3(centering.x, centering.y + (row * 2.0f), 0.0f);
            Debug.Log("row position" + rowPosition);
            for(int col = 0; col < this.columns; col++){
                Invader invader = Instantiate(this.prefabs[row], this.transform);
                invader.killed += InvaderKilled;

                Vector3 position = rowPosition;
                position.x += col * 2.0f;
                position.y += 7.0f;
                Debug.Log("position " + position);
                invader.transform.position = position;
            }
        }
    }

    private void Start(){
        InvokeRepeating(nameof(MissileAttack), this.missileAttackRate, this.missileAttackRate);
    }

    private void Update(){
        this.transform.position += _direction * this.speed.Evaluate(this.percentKilled) * Time.deltaTime;
        
        Vector3 leftEdge = Camera.main.ViewportToWorldPoint(Vector3.zero);
        Vector3 rightEdge = Camera.main.ViewportToWorldPoint(Vector3.right);

        foreach(Transform invader in this.transform)
        {
            if(!invader.gameObject.activeInHierarchy){
                continue;
            }
            if(_direction == Vector3.right && invader.position.x >= 14.0f){
                AdvanceRow();
            }
            else if(_direction == Vector3.left && invader.position.x <= -14.0f){
                AdvanceRow();
            }
        }
        
        UpdateAmountKilledText();
    }

    private void UpdateAmountKilledText()
    {
        amountKilledText.text = "Amount Killed: " + amountKilled.ToString();
    }

    private void AdvanceRow(){
        _direction.x *= -1.0f;

        Vector3 position = this.transform.position;
        position.y -= 1.0f;
        this.transform.position = position;
    }

    private void MissileAttack(){
        foreach(Transform invader in this.transform)
        {
         if(!invader.gameObject.activeInHierarchy){
                continue;
            }
         if(Random.value < (1.0f / (float)this.amountAlive))  {
            Instantiate(this.missilePrefab, invader.position, Quaternion.identity);
            break;
         }
        }
    }

    private void InvaderKilled(){
        this.amountKilled++;
        if(this.amountKilled >= totalInvaders){
            bunker1.ResetScene();
            bunker2.ResetScene();
            bunker3.ResetScene();
            bunker4.ResetScene();
            ResetScene();
        }
        invaderKilled?.Invoke();
        UpdateAmountKilledText();
    }

    public void ResetScene()
    {

    foreach (Transform invader in transform)
        {
            Destroy(invader.gameObject);
        }

    Debug.Log("running reset");
    missileAttackRate = 1.0f;
    amountKilled = 0;
    _direction = Vector2.right;

    float width = 2.0f * (this.columns - 1);
    float height = 2.0f * (this.rows - 1);
    Vector2 centering = new Vector2(-width/2, -height/2);
    Debug.Log("centering " + centering);

    for(int row = 0; row < this.rows; row++){
        Vector3 rowPosition = new Vector3(centering.x, centering.y + (row * 2.0f), 0.0f);
        Debug.Log("row position" + rowPosition);
        for(int col = 0; col < this.columns; col++){
            Invader invader = Instantiate(this.prefabs[row], this.transform);
            invader.killed += InvaderKilled;

            Vector3 position = rowPosition;
            position.x += col * 2.0f;
            position.y += 7.0f;
            Debug.Log("position " + position);
            invader.transform.position = position;
        }
    }
    }
}
