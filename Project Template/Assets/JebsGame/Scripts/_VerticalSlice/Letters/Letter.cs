using JebsReadingGame.Pools;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Animations;

namespace JebsReadingGame.Letters
{
    [Serializable]
    public class Font
    {
        public TMP_FontAsset asset;
        public float fontSizeOffset = 0.0f;
    }

    public class Letter : MonoBehaviour, IPooledObject
    {
        public TextMeshPro tmpro;
        public Collider trigger;
        public LookAtConstraint constraint;

        char _letter;
        public char letter
        {
            get { return _letter; }
            set
            {
                _letter = value;

                tmpro.text = _letter.ToString();
            }
        }

        Color _color;
        public Color color
        {
            get { return _color; }
            set
            {
                _color = value;

                tmpro.color = _color;
            }
        }

        Font _font;
        public Font font
        {
            get { return _font; }
            set
            {
                _font = value;

                tmpro.font = _font.asset;
                tmpro.fontSize = _fontSize + _font.fontSizeOffset;
            }
        }

        float _fontSize;
        public float fontSize
        {
            get { return _fontSize; }
            set
            {
                _fontSize = value;

                tmpro.fontSize = _fontSize + _font.fontSizeOffset;
            }
        }

        Transform _lookAt;
        public Transform lookAt
        {
            get { return _lookAt; }
            set
            {
                _lookAt = value;

                ConstraintSource source = new ConstraintSource();
                source.weight = 1.0f;
                source.sourceTransform = _lookAt;

                constraint.SetSource(0, source);
            }
        }

        public void OnInstantiation()
        {
            // If not found, try to get them withing your own object
            if (!tmpro) tmpro = GetComponent<TextMeshPro>();
            if (!constraint) constraint = GetComponent<LookAtConstraint>();
            if (!trigger) trigger = GetComponent<Collider>();

            if (trigger) trigger.isTrigger = true; // Force trigger condition
        }

        public void OnRespawn()
        {
            // ...
        }
    }
}
