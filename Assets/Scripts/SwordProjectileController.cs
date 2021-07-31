using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordProjectileController : MonoBehaviour
{
    public int projectileAmount = 1;
    public int spreadAmount = 1;
    public float spawnDistance = 5;

    float damage;

    public GameObject projectileObject;

    public GameObject swordPlacementParent;
    public Camera camera;

    void Start()
    {
        
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            damage = this.GetComponent<PlayerSwordController>().damage;
            StartCoroutine(MultiShot());
        }
    }

    IEnumerator MultiShot()
    {
        for (int i = 0; i < projectileAmount; i++)
        {
            for (int y = 0; y < spreadAmount; y++)
            {
                float curAngle = 90;

                if (spreadAmount == 3)
                {
                    if (y == 0) curAngle = 90;
                    else if (y == 1) curAngle = 80;
                    else curAngle = 100;
                }
                else if (spreadAmount == 2)
                {
                    if (y == 0) curAngle = 80;
                    else if (y == 1) curAngle = 100;
                }

                float radian = curAngle * Mathf.Deg2Rad;

                Vector3 newPos = new Vector3(Mathf.Cos(radian), Mathf.Sin(radian), 0);
                newPos *= spawnDistance;

                GameObject curProjectile = Instantiate(projectileObject, Vector3.zero, swordPlacementParent.transform.rotation);
                curProjectile.transform.parent = swordPlacementParent.transform;
                curProjectile.transform.localPosition = new Vector3(newPos.x, 0, newPos.y);
                curProjectile.GetComponent<SwordProjectileController1>().damge = damage;

                if (curAngle == 80) curProjectile.transform.Rotate(0, 20, 0);
                else if (curAngle == 100) curProjectile.transform.Rotate(0, -20, 0);
                curProjectile.transform.parent = null;
            }

            yield return new WaitForSeconds(.3f);
        }

        yield return null;
    }
}
