using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
//using UnityEngine.InputSystem;
using Fishing.IO;
////using UnityEngine.EventSystems;

namespace Fishing.UI
{
    public class MenuNavigation : MonoBehaviour
    {
//        public List<Selectable> menuOptions;
//        public List<Vector3> menuCoordinates;
//        public bool isSubMenu;
//        public int _currentIndex = 0;
//        public Vector3 _currentCoordinates;

//        //private bool usingMouse = false;
//        ////private Controls _controls;

//        ////private EventSystem eventSystem;

//        private void Awake()
//        {
//            //eventSystem = FindObjectOfType<EventSystem>();

//            //_controls = new Controls();
//            //_controls.TitleMenuInput.Enable();

//            //if (menuOptions.Count <= 0) return;
//            //_controls.TitleMenuInput.Navigate.performed += Navigate;
//            //_controls.TitleMenuInput.MouseMovement.performed += MouseMovement;

//            _currentIndex = 0;
//            _currentCoordinates = menuCoordinates[0];
//            menuOptions[_currentIndex].Select();
//        }

//        private void Update()
//        {
//            // Putting code in update because of a Unity events bug that takes at least one frame to update selected buttons
//            //if (usingMouse) return;
//            //else
//            //{
//            //    GameObject _go = eventSystem.currentSelectedGameObject;
//            //    if (_go != menuOptions[_currentIndex].gameObject) eventSystem.SetSelectedGameObject(null);
//            //    menuOptions[_currentIndex].Select();
//            //}
//        }

//        //public void MouseMovement()
//        //{
//        //    Vector2 _mouseVector = _context.ReadValue<Vector2>();
//        //    if (_mouseVector != Vector2.zero) usingMouse = true;
//        //}

//        //public void Navigate()
//        //{
//        //    Vector2 _inputVector = _context.ReadValue<Vector2>();
//        //    if (_inputVector.y != 0)
//        //    {
//        //        Vector2 _columnBounds = GetColumnBounds((int)_currentCoordinates.y);
//        //        if (_currentCoordinates.z + _inputVector.y < _columnBounds.x)
//        //        {
//        //            SetSelectedMenuOptionFromCoordinates((int)_currentCoordinates.y, (int)_columnBounds.y);
//        //        }
//        //        else if (_currentCoordinates.z + _inputVector.y > _columnBounds.y)
//        //        {
//        //            SetSelectedMenuOptionFromCoordinates((int)_currentCoordinates.y, (int)_columnBounds.x);
//        //        }
//        //        else
//        //        {
//        //            SetSelectedMenuOptionFromCoordinates((int)_currentCoordinates.y, (int)(_currentCoordinates.z + _inputVector.y));
//        //        }
//        //    }
//        //    else if (_inputVector.x != 0)
//        //    {
//        //        int _newX = (int)(_currentCoordinates.y + _inputVector.x);
//        //        if (_newX >= 0 && _newX <= GetFurthestRow())
//        //        {
//        //            int _newY = GetColumnTransferY((int)_currentCoordinates.y, (int)_inputVector.x);
//        //            SetSelectedMenuOptionFromCoordinates((int)(_currentCoordinates.y + _inputVector.x), _newY);
//        //        }
//        //    }

//        //    usingMouse = false;
//        //}

//        private int GetIndexFromCoordinates(int _x, int _y)
//        {
//            int _index = -1;
//            foreach (Vector3 _v in menuCoordinates)
//            {
//                if (_v.y != _x) continue;
//                if (_v.z != _y) continue;
//                _index = (int)_v.x;
//            }
//            return _index;
//        }

//        private Vector2 GetColumnBounds(int _col)
//        {
//            int _top = -1;
//            int _bottom = -1;
//            foreach (Vector3 _v in menuCoordinates)
//            {
//                if (_v.y != _col) continue;

//                if (_top == -1)
//                {
//                    _top = (int)_v.z;
//                    _bottom = (int)_v.z;
//                    continue;
//                }

//                if (_v.z < _top) _top = (int)_v.z;
//                if (_v.z > _bottom) _bottom = (int)_v.z;
//            }
//            return new Vector2(_top, _bottom);
//        }

//        private int GetColumnTransferY(int _col, int _dir)
//        {
//            foreach (Vector3 _v in menuCoordinates)
//            {
//                if (_v.y != _col + _dir) continue;

//                if (_currentCoordinates.z == _v.z)
//                {
//                    return (int)_currentCoordinates.z;
//                }
//            }
//            Vector2 _columnBounds = GetColumnBounds(_col + _dir);
//            if (menuCoordinates[_currentIndex].z > _columnBounds.y)
//            {
//                return (int)_columnBounds.y;
//            }
//            return (int)_columnBounds.x;
//        }

//        private int GetFurthestRow()
//        {
//            int _furthestRow = 0;
//            foreach (Vector3 _v in menuCoordinates)
//            {
//                if (_v.y > _furthestRow) _furthestRow = (int)_v.y;
//            }
//            return _furthestRow;
//        }

//        private void SetSelectedMenuOptionFromCoordinates(int _x, int _y)
//        {
//            _currentIndex = GetIndexFromCoordinates(_x, _y);
//            _currentCoordinates = menuCoordinates[_currentIndex];
//        }

//        //private void OnEnable()
//        //{
//        //    _controls.TitleMenuInput.Navigate.performed += Navigate;
//        //    _controls.TitleMenuInput.MouseMovement.performed += MouseMovement;
//        //}

//        private void OnDisable()
//        {
//            _currentIndex = 0;
//            if (menuOptions.Count <= 0) return;
//            _currentCoordinates = menuCoordinates[0];
//            //_controls.TitleMenuInput.Navigate.performed -= Navigate;
//            //_controls.TitleMenuInput.MouseMovement.performed -= MouseMovement;
//        }
    }
}
