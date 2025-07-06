using System.Collections;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public ItemData data;
    public int id;
    public int prefabId;
    public float damage;
    public int count;
    public float speed;

    public Gear gear;
    float timer;
    Player player;

    public void SetGear(Gear gear)
    {
        this.gear = gear;
    }
    void Awake()
    {
        player = GameManager.instance.player;
    }

    void Update()
    {
        if (!GameManager.instance.isLive)
            return;
        Vector3 dir = player.transform.forward; // 플레이어의 앞쪽 방향으로 발사
        switch (id)
        {
            case 0:
                transform.Rotate(Vector3.forward * speed * Time.deltaTime);
                break;
            case 5:
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;
            case 7:  // 새로운 무기의 ID를 7로 가정
                // z축을 중심으로 회전
                transform.Rotate(Vector3.back * speed * Time.deltaTime);
                break;
            default:
                timer += Time.deltaTime;
                if (timer > speed)
                {
                    timer = 0f;
                    Fire(data);
                }
                break;
        }
    }

    public void LevelUp(float damage, int count, ItemData data)
    {
        this.damage = damage * Character.Damage;
        this.count += count;
        Debug.Log(damage);
        if (id == 0 || id == 5 || id == 7)
            Batch(data);

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }

    public void Init(ItemData data)
    {
        //Basic Set
        name = "Weapon" + data.itemId;
        transform.parent = player.transform;
        transform.localPosition = Vector3.zero;

        //Property Set
        id = data.itemId;
        damage = data.baseDamage * Character.Damage;
        count = data.baseCount + Character.Count;

        for (int index = 0; index < GameManager.instance.pool.prefabs.Length; index++)
        {
            if (data.projectile == GameManager.instance.pool.prefabs[index])
            {
                prefabId = index;
                break;
            }
        }

        switch (id)
        {
            case 0:
                speed = 180 * Character.WeaponSpeed;
                Batch(data);
                break;
            case 5:
                speed = 210 * Character.WeaponSpeed;
                Batch(data);
                break;
            case 6:
                speed = 1.1f * Character.WeaponRate;
                break;
            case 7:
                speed = 75 * Character.WeaponSpeed;
                Batch(data);
                break;
            case 8:
                speed = 0.1125f * Character.WeaponRate;
                Batch(data);
                break;
            default:
                speed = 0.3f * Character.WeaponRate;
                break;
        }

        // Hand Set
        if ((int)data.itemType == 5)
        {
            Hand hand = player.hands[2];
            hand.spriter.sprite = data.hand;
            hand.gameObject.SetActive(true);
        }
        else if ((int)data.itemType == 6)
        {
            Hand hand = player.hands[3];
            hand.spriter.sprite = data.hand;
            hand.gameObject.SetActive(true);
        }
        else if ((int)data.itemType == 7)
        {
            Hand hand = player.hands[4];
            hand.spriter.sprite = data.hand;
            hand.gameObject.SetActive(true);
        }
        else if ((int)data.itemType == 8)
        {
            Hand hand = player.hands[5];
            hand.spriter.sprite = data.hand;
            hand.gameObject.SetActive(true);
        }
        else
        {
            Hand hand = player.hands[(int)data.itemType];
            hand.spriter.sprite = data.hand;
            hand.gameObject.SetActive(true);
        }

        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver);
    }
    void Batch(ItemData data)
    {
        switch (id)
        {
            case 0:
                for (int index = 0; index < count; index++)
                {
                    Transform bullet1;
                    if (index < transform.childCount)
                    {
                        bullet1 = transform.GetChild(index);
                    }
                    else
                    {
                        bullet1 = GameManager.instance.pool.Get(prefabId).transform;
                        bullet1.parent = transform;
                    }

                    bullet1.localPosition = Vector3.zero;
                    bullet1.localRotation = Quaternion.identity;

                    Vector3 rotVec = Vector3.forward * 360 * index / count;
                    bullet1.Rotate(rotVec);
                    // 무기가 배치될 Y좌표
                    bullet1.Translate(bullet1.up * 1.5f, Space.World);
                    bullet1.GetComponent<Bullet>().Init(damage, -100, Vector3.zero); // -100 is infinity Per.
                }
                break;
            case 5:
                float radius = 3.75f; // 무기가 배치될 원의 반지름 설정 (원하는 값으로 변경)
                for (int index = 0; index < count; index++)
                {
                    Transform bullet2;
                    if (index < transform.childCount)
                    {
                        bullet2 = transform.GetChild(index);
                    }
                    else
                    {
                        bullet2 = GameManager.instance.pool.Get(prefabId).transform;
                        bullet2.parent = transform;
                    }

                    // 무기가 배치될 각도 계산 (원을 그리며 배치)
                    float angle = 360.0f * index / count;
                    Vector3 position = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * radius;

                    // 무기 위치 설정
                    bullet2.localPosition = position;

                    // 무기의 방향을 이동 방향으로 조절
                    Vector3 weaponDirection = -position.normalized;
                    float weaponRotation = Mathf.Atan2(weaponDirection.y, weaponDirection.x) * Mathf.Rad2Deg;

                    // 무기 회전 설정
                    bullet2.localRotation = Quaternion.Euler(0, 0, weaponRotation);
                    bullet2.GetComponent<Bullet>().Init(damage, -100, Vector3.zero); // -100 is infinity Per.
                }
                break;
            case 7:  // 새로운 무기의 ID를 7로 가정
                radius = 2.5f;  // 이 값은 발사체가 배치될 원의 반지름을 결정합니다.

                for (int index = 0; index < count; index++)
                {
                    Transform bullet;
                    if (index < transform.childCount)
                    {
                        bullet = transform.GetChild(index);
                    }
                    else
                    {
                        bullet = GameManager.instance.pool.Get(prefabId).transform;
                        bullet.parent = transform;
                    }

                    // 발사체가 배치될 각도 계산 (원을 그리며 배치)
                    float angle = 360.0f * index / count;
                    Vector3 position = new Vector3(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)) * radius;

                    // 발사체 위치 설정
                    bullet.localPosition = position;

                    // 발사체의 방향을 이동 방향으로 조절
                    Vector3 bulletDirection = -position.normalized;
                    float bulletRotation = Mathf.Atan2(bulletDirection.y, bulletDirection.x) * Mathf.Rad2Deg;

                    // 발사체 회전 설정
                    bullet.localRotation = Quaternion.Euler(0, 0, bulletRotation);
                    bullet.GetComponent<Bullet>().Init(damage, -100, Vector3.zero); // -100 is infinity Per.
                }
                break;
            default:
                break;
        }
    }

    void Fire(ItemData data)
    {
        if (!player.scanner.nearestTarget)
            return;

        Vector3 targetPos = player.scanner.nearestTarget.position;
        Vector3 dir = targetPos - transform.position;
        dir = dir.normalized;

        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = transform.position;
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        Debug.Log(damage);
        bullet.GetComponent<Bullet>().Init(damage, count, dir); // -1 is infinity Per.

        AudioManager.instance.PlaySfx(AudioManager.Sfx.Range);
    }
}
