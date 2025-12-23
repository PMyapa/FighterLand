using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class targetMovement : MonoBehaviour
{
    player playerMovement;
    [SerializeField] float tankSpeed;
    [SerializeField] float movementStartDis;
    public bool tmAlive;

    private float targetSpeedRan;
    // Start is called before the first frame update
    void Start()
    {
        playerMovement = GameObject.FindObjectOfType<player>();
        targetSpeedRan = Random.Range(tankSpeed - 1f, tankSpeed + 1f);
        tmAlive = true;
    }

    // Update is called once per frame
    public void FixedUpdate()
    {
        Vector3 pp = playerMovement.transform.position;
        Vector3 tp = this.transform.position;

        float distance = DisToTank(pp, tp);

        if (distance < movementStartDis && tmAlive)
        {

            transform.position += -transform.forward * targetSpeedRan * Time.deltaTime ;
        }

        
    }


    public void tmDie()
    {
        tmAlive = false;
    }

    float DisToTank(Vector3 a, Vector3 b)
    {
        float xx = Mathf.Abs(a.x - b.x);
        float zz = Mathf.Abs(a.z - b.z);

        float sqDis = (xx * xx) + (zz * zz);
        return Mathf.Sqrt(sqDis) ;
    }

}
