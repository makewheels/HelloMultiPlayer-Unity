using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class Health : NetworkBehaviour
{
    public int maxHealth = 100;
    [SyncVar(hook = "onChangeHealth")] public int currentHealth = 100;
    public Slider healthSlider;


    public void takeDamage(int damage)
    {
        //血量处理只在服务器执行
        if (isServer == false)
        {
            return;
        }

        currentHealth -= damage;
        if (currentHealth <= 0)
        {
            currentHealth = 0;
            RpcRespawn();
        }
    }

    private void onChangeHealth(int health)
    {
        //更新血条
        healthSlider.value = health * 1.0f / maxHealth;
    }

    [ClientRpc]
    private void RpcRespawn()
    {
        if (isLocalPlayer == false)
        {
            return;
        }

        transform.position = new Vector3(0, 1.017199f, 0);
        currentHealth = maxHealth;
    }

    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        healthSlider.transform.LookAt(Camera.main.transform);
    }
}