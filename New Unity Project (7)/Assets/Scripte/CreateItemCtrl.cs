using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateItemCtrl : MonoBehaviour
{
    public GameObject Coin;

    // Start is called before the first frame update
    void OnCollisionEnter(Collision coll)
    {
        Debug.Log("ItemCollision");
        if (coll.collider.tag == "Player")
        {
            Debug.Log("destroy");
            GameObject coin = (GameObject)Instantiate(Coin, this.transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
