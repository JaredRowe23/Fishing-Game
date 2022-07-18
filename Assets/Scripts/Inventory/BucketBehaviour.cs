using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.Fishables;
using Fishing.UI;
using Fishing.IO;

namespace Fishing.Inventory
{
    public class BucketBehaviour : MonoBehaviour
    {
        [SerializeField] public List<FishData> bucketList = new List<FishData>();
        public int maxItems;

        public static BucketBehaviour instance;

        private void Awake() => instance = this;

        private void Start()
        {
            int i = 0;
            foreach(string _fish in PlayerData.instance.bucketFish)
            {
                FishData _newItem = new FishData();
                _newItem.itemName = PlayerData.instance.bucketFish[i];
                _newItem.itemDescription = PlayerData.instance.bucketFishDescription[i];
                _newItem.itemWeight = PlayerData.instance.bucketFishWeight[i];
                _newItem.itemLength = PlayerData.instance.bucketFishLength[i];
                _newItem.itemValue = PlayerData.instance.bucketFishValue[i];
                bucketList.Add(_newItem);
                i++;
            }
        }

        public void AddToBucket(Fishable _item)
        {
            AudioManager.instance.PlaySound("Catch Fish");

            FishData _newItem = new FishData();
            {
                _newItem.itemName = _item.GetName();
                _newItem.itemDescription = _item.GetDescription();
                _newItem.itemWeight = _item.GetWeight();
                _newItem.itemLength = _item.GetLength();
                _newItem.itemValue = _item.GetValue();
            }

            if (bucketList.Count >= maxItems)
            {
                BucketMenu.instance.ShowBucketMenu();
                UIManager.instance.overflowItem.SetActive(true);
                BucketMenuItem _overflowMenu = UIManager.instance.overflowItem.GetComponent<BucketMenuItem>();

                _overflowMenu.UpdateName(_item.GetName());
                _overflowMenu.UpdateLength(_newItem.itemLength);
                _overflowMenu.UpdateWeight(_newItem.itemWeight);

                _overflowMenu.UpdateReference(_newItem);

                return;
            }

            AudioManager.instance.PlaySound("Add To Bucket");

            _item.DisableMinimapIndicator();
            bucketList.Add(_newItem);
            bool _emptySpace = false;
            for(int i = 0; i < PlayerData.instance.bucketFish.Count; i++)
            {
                if (PlayerData.instance.bucketFish[i] != null)
                {
                    i++;
                    continue;
                }
                if (PlayerData.instance.bucketFishDescription[i] != null)
                {
                    i++;
                    continue;
                }
                if (PlayerData.instance.bucketFishWeight[i] != 0)
                {
                    i++;
                    continue;
                }
                if (PlayerData.instance.bucketFishLength[i] != 0)
                {
                    i++;
                    continue;
                }
                if (PlayerData.instance.bucketFishValue[i] != 0)
                {
                    i++;
                    continue;
                }

                PlayerData.instance.bucketFish[i] = _newItem.itemName;
                PlayerData.instance.bucketFishDescription[i] = _newItem.itemDescription;
                PlayerData.instance.bucketFishWeight[i] = _newItem.itemWeight;
                PlayerData.instance.bucketFishLength[i] = _newItem.itemLength;
                PlayerData.instance.bucketFishValue[i] = _newItem.itemValue;
                _emptySpace = true;
            }

            if (!_emptySpace)
            {
                PlayerData.instance.bucketFish.Add(_newItem.itemName);
                PlayerData.instance.bucketFishDescription.Add(_newItem.itemDescription);
                PlayerData.instance.bucketFishWeight.Add(_newItem.itemWeight);
                PlayerData.instance.bucketFishLength.Add(_newItem.itemLength);
                PlayerData.instance.bucketFishValue.Add(_newItem.itemValue);
            }

            _item.GetComponent<IEdible>().Despawn();
        }
    }

}