﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Fishing
{
    public class HookBehaviour : MonoBehaviour, IEdible
    {
        public GameObject hookedObject;

        [SerializeField] private Transform linePivotPoint;
        [SerializeField] private float resetSpeed;
        [SerializeField] private float hookHangHeight;

        [Header("Water Physics")]
        [SerializeField] private float drag;
        [SerializeField] private float waterDrag;
        [SerializeField] private float lineRotationAngle = -5f;

        private Vector3 _targetPos;
        private RodBehaviour _rod;
        private Rigidbody _rb;

        public bool playedSplash;

        private void Start()
        {
            _rod = transform.parent.GetComponent<RodBehaviour>();
            _rb = this.GetComponent<Rigidbody>();
        }

        void Update()
        {
            if (transform.position.y <= 0f)
            {
                OnSubmerged();
            }
            else
            {
                OnSurfaced();
            }

            if (hookedObject && !GameController.instance.overflowItem.activeSelf)
            {
                hookedObject.transform.position = this.transform.position;
            }

            if (!_rod.casted)
            {
                _rb.isKinematic = true;
                _targetPos = linePivotPoint.position - new Vector3(0, hookHangHeight, 0);
                transform.position = Vector3.MoveTowards(transform.position, _targetPos, resetSpeed * Time.deltaTime);
                return;
            }

            HandlePhysics();
        }

        private void HandlePhysics()
        {
            if (Vector3.Distance(transform.position, linePivotPoint.position) >= _rod.GetLineLength())
            {
                if (transform.position.y < 0f)
                {
                    _rb.useGravity = false;
                    _rb.isKinematic = false;
                    if (transform.position.x - linePivotPoint.position.x >= 0f)
                    {
                        transform.RotateAround(linePivotPoint.position, Vector3.forward, lineRotationAngle * Time.deltaTime);
                        transform.rotation = Quaternion.identity;
                    }
                }

                else
                {
                    _rb.velocity = new Vector3(0f, _rb.velocity.y, _rb.velocity.z);
                }
            }
            else
            {
                _rb.useGravity = true;
                _rb.isKinematic = false;
            }
        }

        public void Despawn()
        {
            return;
        }

        private void OnSubmerged()
        {
            if (!playedSplash)
            {
                AudioManager.instance.PlaySound("Hook Splash");
                playedSplash = true;
            }
            _rb.drag = waterDrag;
        }
        private void OnSurfaced()
        {
            _rb.drag = drag;
            playedSplash = false;
        }

        public void Cast(float _angle, float _force)
        {
            _rb.isKinematic = false;
            Quaternion rot = Quaternion.AngleAxis(_angle, Vector3.forward);
            _rb.AddForce(rot * Vector3.right * _force);
        }

        public void Reel(float _force) => _rb.AddForce(Vector3.Normalize(linePivotPoint.position - transform.position) * _force * Time.deltaTime);

        public Transform GetHookAnchorPoint() => linePivotPoint;
        private void OnTriggerEnter(Collider _other)
        {
            SetHook(_other.GetComponent<FishableItem>());
        }

        public void SetHook(FishableItem fishable)
        {
            if (hookedObject != null) return;

            fishable.OnHooked(transform);
            hookedObject = fishable.gameObject;
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

                FishableItem _item = hookedObject.GetComponent<FishableItem>();
                BucketMenuItem _overflowMenu = GameController.instance.overflowItem.GetComponent<BucketMenuItem>();

                _overflowMenu.UpdateName(_item.GetName());
                _overflowMenu.UpdateLength(_item.GetLength());
                _overflowMenu.UpdateWeight(_item.GetWeight());
                FishData newData = new FishData();
                newData.itemName = _item.GetName();
                newData.itemDescription = _item.GetDescription();
                newData.itemWeight = _item.GetWeight();
                newData.itemLength = _item.GetLength();
                _overflowMenu.UpdateReference(newData);
                Destroy(hookedObject);
                hookedObject = null;
            }
            else
            {
                AudioManager.instance.PlaySound("Add To Bucket");
                BucketBehaviour.instance.AddToBucket(hookedObject.GetComponent<FishableItem>());
                hookedObject.GetComponent<IEdible>().Despawn();
                hookedObject = null;
            }
        }
    }

}