using UnityEngine;
using TMPro;
public class FruitSpawner : MonoBehaviour
{
    public SpriteRenderer fruitRenderer;
    public Sprite[] fruitPrefabs; // array of fruit prefabs
    public Transform spawnPoint;      // place in your maze where fruit appears
    public float spawnInterval = 10f; // spawn every 10 seconds
    public int fruitScore = 100;      // points for collecting fruit
    public TMP_Text fruitScoreText; 
    public float timer { get; private set; } = 0f;

    void Update()
    {
        timer += Time.deltaTime;
        if(timer>1.0f)
        {
            fruitScoreText.text="";
            if (timer >= spawnInterval)
            {
                SpawnRandomFruit();
                timer = 0;
            }
        }
       
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.layer == LayerMask.NameToLayer("Pacman"))
        {
            FindObjectOfType<GameManager>().FruitEaten();
            fruitRenderer.sprite = null;
            fruitScoreText.text="100";
            GetComponent<Collider2D>().enabled = false; // disable collider until next spawn
            timer = 0f; // reset timer to spawn next fruit
        }
    }
    void SpawnRandomFruit()
    {   
        GetComponent<Collider2D>().enabled = true;
        int index = Random.Range(0, fruitPrefabs.Length); // choose random fruit
        fruitRenderer.sprite = fruitPrefabs[index];
    }
}
