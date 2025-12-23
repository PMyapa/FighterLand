using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camFollow : MonoBehaviour
{
    public Transform targetObject;
    public Vector3 cameraOffset;
    [SerializeField] float Camangle;
    bool playerAlive;
    bool playerFuel;
    float playerSpeed;
    float playerBasicSpeed;

    private float camVibraion;
    private float tvar;

    player playerMovement;

    // Start is called before the first frame update
    private void Start()
    {
       
        playerMovement = GameObject.FindObjectOfType<player>();
       

    }

   

    // Update is called once per frame
    private void FixedUpdate()
    {
        playerAlive = playerMovement.alive;
        playerFuel = playerMovement.fueled;
        playerSpeed = playerMovement.speed;


        if (playerAlive )
        {

            Vector3 targetpos;

            targetpos.y = transform.position.y;
            if (playerFuel)
            {
                targetpos.y = targetObject.position.y + 1f;
            }
            else
            {
                targetpos.y = transform.position.y;
            }

            targetpos.z = targetObject.position.z - 1.2f;
                  
            targetpos.x = targetObject.position.x;


            Vector3 spos = Vector3.Lerp(transform.position, targetpos, 50f * Time.fixedDeltaTime);
            spos.x = targetpos.x;
            spos.y = Mathf.Lerp(transform.position.y, targetpos.y, 20f * Time.fixedDeltaTime);
            transform.position = spos;


            var targetAngle = targetObject.eulerAngles.y;
            var currentAngle = Mathf.LerpAngle(transform.eulerAngles.y, targetAngle, 5f * Time.deltaTime);

            if (playerMovement.gettingShot)
            {
                tvar += 20 * Time.deltaTime;
                camVibraion = (tvar % 2f) - 1f;
            }
            else
            {
                camVibraion = 0;
                tvar = 0;
            }

            transform.rotation = Quaternion.Euler(Camangle, 0, 0 + camVibraion);

        }
        else
        {
            
            transform.position += new Vector3(0, 0,playerSpeed*0.01f);
        }
    }
}
