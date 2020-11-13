using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;
using UnityEngine.Networking;

public class PlayerController : NetworkBehaviour
{
    public GameObject bulletPrefab;
    public Transform muzzle;

    private float rotateSpeed = 120;
    private float moveSpeed = 4;
    private float bulletSpeed = 10;


    // Use this for initialization
    void Start()
    {
        transform.position = new Vector3(transform.position.x, 1.017199f, transform.position.z);
    }

    // Update is called once per frame
    void Update()
    {
        //本地player
        if (isLocalPlayer == false)
        {
            return;
        }

        //移动旋转
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        transform.Rotate(Vector3.up * horizontal * rotateSpeed * Time.deltaTime);
        transform.Translate(Vector3.right * vertical * moveSpeed * Time.deltaTime);

        //开火
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CmdFire();
        }
    }

    public override void OnStartLocalPlayer()
    {
        GetComponent<MeshRenderer>().material.color = Color.blue;
    }

    [Command]
    private void CmdFire()
    {
        GameObject bullet = Instantiate(bulletPrefab, muzzle.position, muzzle.rotation);
        bullet.GetComponent<Rigidbody>().velocity = bullet.transform.right * bulletSpeed;
        Destroy(bullet, 3);
        NetworkServer.Spawn(bullet);
    }
}