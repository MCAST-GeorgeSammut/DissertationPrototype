using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyTracker : MonoBehaviour
{
    List<KeyObject.KeyType> keyList;

    void Awake()
    {
        keyList = new List<KeyObject.KeyType>();
    }

    public void AddKey(KeyObject.KeyType keyType)
    {
        Debug.Log("Added Key: " + keyType);
        keyList.Add(keyType);
    }

    public void RemoveKey(KeyObject.KeyType keyType)
    {
        keyList.Remove(keyType);
    }

    public bool ContainsKey(KeyObject.KeyType keyType)
    {
        return keyList.Contains(keyType);
    }

    private void OnTriggerEnter2D(Collider2D collider)
    {
        //if collider is a key
        KeyObject key = collider.GetComponent<KeyObject>();
        if ( key != null)
        {
            AddKey(key.GetKeyType());
            Destroy(key.gameObject);
        }

        //if collider is a door
        DoorObject door = collider.GetComponent<DoorObject>();
        if (door != null)
        {
            if (ContainsKey(door.GetKeyType()))
            {
                //Holding key that matches door, remove key and open door
                RemoveKey(door.GetKeyType());
                Destroy(door.gameObject);
            }
        }
    }
}
