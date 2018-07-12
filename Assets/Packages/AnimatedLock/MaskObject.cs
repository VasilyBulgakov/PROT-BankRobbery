using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Interface
{
    public class MaskObject : MonoBehaviour
    {
        /// <summary>
        /// цвет маски
        /// </summary>
        public Color colorMask;

        /// <summary>
        /// видна ли маска после мигания
        /// </summary>
        public bool isVisible = true;

        /// <summary>
        /// знак показывающий увеличиваем или уменьшаем альфа-канал маски
        /// </summary>
        private int _signDelta = 1;

        /// <summary>
        /// значение альфа-канала цвета маски
        /// </summary>
        private float _alpha = 0;

        /// <summary>
        /// Материал маски
        /// </summary>
        private Material _material;

        /// <summary>
        /// общее количество миганий маски
        /// </summary>
        [SerializeField]
        private int _countFlash = 5;    

        /// <summary>
        /// текущее количество миганий маски
        /// </summary>
        private int _flash = 0;

        /// <summary>
        /// время миганий маски
        /// (время за которое меняется значение альфа от нижнего до верхнего значение и обратно)
        /// </summary>
        [SerializeField]
        private float _timeFlash = 1;

        /// <summary>
        /// верхнее значение альфа-канала
        /// </summary>
        [SerializeField]
        private float _upperValueAlpha = 0.9f;

        /// <summary>
        /// нижнее значение альфа-канала
        /// </summary>
        [SerializeField]
        private float _lowerValueAlpha = 0.1f;

        /// <summary>
        /// Приращение альфа за секунду
        /// </summary>
        private float _deltaAlpha;

        // Use this for initialization
        void Awake()
        {
            _material = transform.GetComponent<Renderer>().material;
            _material.SetVector("_Color", colorMask);
            _alpha = _lowerValueAlpha;
            _deltaAlpha = (_upperValueAlpha - _lowerValueAlpha) / _timeFlash * 2;
        }

        // Update is called once per frame
        void Update()
        {
            if (_flash < _countFlash)
            {
                //меняем знак если достигли граничных значений
                if (_alpha > _upperValueAlpha)
                {
                    _signDelta = -1;
                    
                }
                else
                if (_alpha < _lowerValueAlpha)
                {
                    _signDelta = 1;
                    _flash++;
                }

                _alpha += _signDelta * _deltaAlpha * Time.deltaTime;
                if (_flash != _countFlash)
                {
                    _material.SetFloat("_Alpha", _alpha);
                }
                else
                {
                    if (isVisible)
                    {
                        _material.SetFloat("_Alpha", 0.9f);
                    }
                    else
                    {
                        _material.SetFloat("_Alpha", 0);
                    }
                }
            }
        }
    }
}