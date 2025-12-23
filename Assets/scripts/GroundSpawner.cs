using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    [SerializeField] GameObject[] Tile;
    Vector3 nextSpawnPoint;

    player playerGround;

    private float mtob;
    private float ctob;


    // Start is called before the first frame update
    void Start()
    {
        playerGround = GameObject.FindObjectOfType<player>();

        for (int i = 0; i < 2; i++)
        {
            if (i < 1)
            {
                SpawnTile(false);
            }
            else
            {
                SpawnTile(true);
            }


        }

        
    }

    void Update()
    {
        mtob = playerGround.mileage;
        ctob = 1f;
        if (mtob <= 30f)
        {
            ctob = 0.072f * mtob + 1.84f;
        }
        else if (mtob > 30f)
        {
            ctob = 4f;
        }
    }

    public void SpawnTile(bool block)
    {
        int tile_index = Random.Range(0, Tile.Length);
        GameObject temp = Instantiate(Tile[tile_index], nextSpawnPoint, Quaternion.Euler(0, 0, 0));
       

        nextSpawnPoint = temp.transform.GetChild(0).transform.position;
        
        if (!block)
        {
            return;
        }
                       

        temp.GetComponent<BlockIns>().SpawnBlock(ctob , mtob);

    }

 
}
