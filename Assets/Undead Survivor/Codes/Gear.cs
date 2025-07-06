using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gear : MonoBehaviour
{
    public ItemData.ItemType type;
    public Weapon weapon;
    public float rate;

    public void Init(ItemData data, Weapon weapon)
    {
        name = "Gear" + data.itemId;
        transform.parent = GameManager.instance.player.transform;
        transform.localPosition = Vector3.zero;
        type = data.itemType;
        rate = data.damages[0];
        this.weapon = weapon;
        ApplyGear(data);
    }

    public void LevelUp(ItemData data, float rate)
    {
        this.rate = rate;
        ApplyGear(data);
    }

    void ApplyGear(ItemData data)
    {
        switch (type)
        {
            case ItemData.ItemType.Glove:
                RateUp();
                break;
            case ItemData.ItemType.Shoe:
                SpeedUp();
                break;
        }
    }

    void RateUp()
    {
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();
        foreach (Weapon weapon in weapons)
        {
            switch (weapon.id)
            {
                case 0:
                    float speed = 180 * Character.WeaponSpeed;
                    weapon.speed = speed + (speed * rate);
                    break;
                case 5:
                    speed = 210 * Character.WeaponSpeed;
                    weapon.speed = speed + (speed * rate);
                    break;
                case 1:
                    speed = 0.3f * Character.WeaponRate;
                    weapon.speed = speed * (1f - rate);
                    break;
                case 6:
                    speed = 1.1f * Character.WeaponRate;
                    weapon.speed = speed * (1f - rate);
                    break;
                case 7:
                    speed = 75 * Character.WeaponSpeed;
                    weapon.speed = speed + (speed * rate);
                    break;
                case 8:
                    speed = 0.1125f * Character.WeaponRate;
                    weapon.speed = speed + (speed * rate);
                    break;
                default:
                    break;
            }
        }
    }

    void SpeedUp()
    {
        float speed = 3 * Character.Speed;
        GameManager.instance.player.speed = speed + speed * rate;
    }
}