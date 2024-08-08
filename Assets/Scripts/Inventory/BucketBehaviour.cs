using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fishing.Fishables;
using Fishing.UI;
using Fishing.IO;
using Fishing.Fishables.Fish;

namespace Fishing.Inventory
{
    public class BucketBehaviour : MonoBehaviour
    {
        [SerializeField] public List<FishData> bucketList = new List<FishData>();
        public int maxItems;

        private PlayerData playerData;

        public static BucketBehaviour instance;

        private void Awake() => instance = this;

        private void Start()
        {
            playerData = PlayerData.instance;

            for (int i = 0; i < playerData.bucketItemSaveData.Count; i++) bucketList.Add(FishDataFromPlayerData(i));
        }

        public void AddToBucket(Fishable _item)
        {
            AudioManager.instance.PlaySound("Catch Fish");

            FishData _fishableData = FishDataFromFishable(_item);

            if (bucketList.Count >= maxItems)
            {
                OnBucketOverflow(_fishableData);
                return;
            }

            AudioManager.instance.PlaySound("Add To Bucket");
            TooltipSystem.instance.NewTooltip(5f, "You caught a " + _fishableData.itemName + " worth $" + _fishableData.itemValue.ToString("F2"));

            _item.DisableMinimapIndicator();
            bucketList.Add(_fishableData);

            UpdatePlayerData(_fishableData);

            _item.GetComponent<IEdible>().Despawn();
        }

        private FishData FishDataFromFishable(Fishable _item)
        {
            FishData _newItem = new FishData();
            {
                _newItem.itemName = _item.GetName();
                _newItem.itemDescription = _item.GetDescription();
                _newItem.itemWeight = _item.GetWeight();
                _newItem.itemLength = _item.GetLength();
                _newItem.itemValue = _item.GetValue();
            }

            return _newItem;
        }

        private FishData FishDataFromPlayerData(int _playerDataIndex)
        {
            FishData _newItem = new FishData();
            _newItem.itemName = playerData.bucketItemSaveData[_playerDataIndex].itemName;
            _newItem.itemDescription = playerData.bucketItemSaveData[_playerDataIndex].description;
            _newItem.itemWeight = playerData.bucketItemSaveData[_playerDataIndex].weight;
            _newItem.itemLength = playerData.bucketItemSaveData[_playerDataIndex].length;
            _newItem.itemValue = playerData.bucketItemSaveData[_playerDataIndex].value;

            return _newItem;
        }

        private void OnBucketOverflow(FishData _fishableData)
        {
            TooltipSystem.instance.NewTooltip(5f, "You've filled your bucket! Pick something to throw away to make room!");
            BucketMenu.instance.ToggleBucketMenu();
            UIManager.instance.overflowItem.SetActive(true);
            BucketMenuItem _overflowMenu = UIManager.instance.overflowItem.GetComponent<BucketMenuItem>();

            _overflowMenu.UpdateName(_fishableData.itemName);
            _overflowMenu.UpdateLength(_fishableData.itemLength);
            _overflowMenu.UpdateWeight(_fishableData.itemWeight);
            _overflowMenu.UpdateValue(_fishableData.itemValue);
            _overflowMenu.UpdateReference(_fishableData);
        }

        private void UpdatePlayerData(FishData _fishableData)
        {
            PopulateNewPlayerData(_fishableData);

            playerData.UpdateFishRecordData(_fishableData.itemName, _fishableData.itemLength, _fishableData.itemWeight);
        }

        private void PopulateNewPlayerData(FishData _fishableData)
        {
            BucketItemSaveData _newItem = new BucketItemSaveData(_fishableData.itemName, _fishableData.itemDescription, _fishableData.itemWeight, _fishableData.itemLength, _fishableData.itemValue);
            playerData.bucketItemSaveData.Add(_newItem);
        }
    }

}