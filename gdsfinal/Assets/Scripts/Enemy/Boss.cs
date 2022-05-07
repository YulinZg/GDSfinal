using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Boss : Enemy
{
    public GameObject stopBulletLauncher;
    public GameObject[] normalBulletLauncher;
    public GameObject arcBulletLauncher;
    // Start is called before the first frame update
    void Start()
    {
        //InvokeRepeating("LaunchStopBullet", 15f, 15f);
        //间隔时间加5才是真正的间隔时间
        //InvokeRepeating("LaunchNormalBullet", 8f, 13f);
        InvokeRepeating("LaunchArcBullet", 8f, 13f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void LaunchArcBullet()
    {
        arcBulletLauncher.GetComponent<ArcBulletLauncher>().enabled = true;
        Invoke("SetNotLaunchArcBullet", 5.0f);
        //stopBulletLauncher.GetComponent<StopBulletLauncher>().enabled = false;
    }

    private void SetNotLaunchArcBullet()
    {
        //stopBulletLauncher.GetComponent<StopBulletLauncher>().enabled = true;
        arcBulletLauncher.GetComponent<ArcBulletLauncher>().enabled = false;
    }
    private void LaunchStopBullet()
    {
        stopBulletLauncher.GetComponent<StopBulletLauncher>().enabled = true;
        Invoke("SetNotLaunchStopBullet", 2.0f);
        //stopBulletLauncher.GetComponent<StopBulletLauncher>().enabled = false;
    }
    private void SetNotLaunchStopBullet()
    {
        //stopBulletLauncher.GetComponent<StopBulletLauncher>().enabled = true;
        stopBulletLauncher.GetComponent<StopBulletLauncher>().enabled = false;
    }
    IEnumerator AppearMagic(float duration, SpriteRenderer magic)
    {
        for (int i = 0; i <= 255; i++)
        {
            magic.color = new Color(magic.color.r, magic.color.g, magic.color.b, (float)i / 255f);
            yield return new WaitForSeconds(duration / 255f);
        }
        
        //parent.setSpeed(parent.getCurrentSpeed());
    }

    IEnumerator DisappearMagic(float duration, SpriteRenderer magic)
    {
        for (int i = 255; i >= 0; i--)
        {
            magic.color = new Color(magic.color.r, magic.color.g, magic.color.b, (float)i / 255f);
            yield return new WaitForSeconds(duration / 255f);
        }

        //parent.setSpeed(parent.getCurrentSpeed());
    }
    private void LaunchNormalBullet()
    {
        foreach (GameObject item in normalBulletLauncher)
        {
            StartCoroutine(AppearMagic(0.5f, item.GetComponent<SpriteRenderer>()));
        }
        
        foreach (GameObject item in normalBulletLauncher)
        {
            item.GetComponent<NormalBulletLauncher>().enabled = true;
        }
        Invoke("SetNotLaunchNormalBullet", 5.0f);
    }

    private void SetNotLaunchNormalBullet()
    {
        foreach (GameObject item in normalBulletLauncher)
        {
            StartCoroutine(DisappearMagic(0.5f, item.GetComponent<SpriteRenderer>()));
        }
        foreach (GameObject item in normalBulletLauncher)
        {
            item.GetComponent<NormalBulletLauncher>().enabled = false;
        }
    }
    public override void UpdateState()
    {
        throw new System.NotImplementedException();
    }

    public override void Move()
    {
        throw new System.NotImplementedException();
    }
}
