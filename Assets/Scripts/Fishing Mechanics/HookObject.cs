using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HookObject : MonoBehaviour
{
    public GameObject hookedObject;

    public static HookObject instance;

    private void Awake() => instance = this;

    private void Update()
    {
        if (hookedObject && !GameController.instance.overflowItem.activeSelf)
        {
            hookedObject.transform.position = this.transform.position;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.GetComponent<FishableItem>()) return;

        if (hookedObject != null) return;

        hookedObject = other.gameObject;
        hookedObject.transform.parent.GetComponent<SpawnZone>().spawnList.Remove(hookedObject);
        hookedObject.GetComponent<FishableItem>().isHooked = true;
        hookedObject.transform.parent = this.transform;
    }

    public void AddToBucket()
    {
        AudioManager.instance.StopPlaying("Reel");

        if (hookedObject == null) return;

        AudioManager.instance.PlaySound("Catch Fish");

        if (BucketBehaviour.instance.bucketList.Count >= BucketBehaviour.instance.maxItems)
        {
            BucketMenu.instance.ShowBucketMenu();
            GameController.instance.overflowItem.SetActive(true);

            FishableItem item = hookedObject.GetComponent<FishableItem>();
            BucketMenuItem overflowMenu = GameController.instance.overflowItem.GetComponent<BucketMenuItem>();

            overflowMenu.UpdateName(item.GetName());
            overflowMenu.UpdateLength(item.GetLength());
            overflowMenu.UpdateWeight(item.GetWeight());
            FishData newData = new FishData();
            newData.itemName = item.GetName();
            newData.itemDescription = item.GetDescription();
            newData.itemWeight = item.GetWeight();
            newData.itemLength = item.GetLength();
            overflowMenu.UpdateReference(newData);
            Destroy(hookedObject);
            hookedObject = null;
        }
        else
        {
            AudioManager.instance.PlaySound("Add To Bucket");
            BucketBehaviour.instance.AddToBucket(hookedObject.GetComponent<FishableItem>());
            Destroy(hookedObject);
            hookedObject = null;
        }
    }
}
