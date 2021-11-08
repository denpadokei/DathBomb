using HMUI;
using IPA.Utilities;
using System;
using TMPro;
using UnityEngine;

namespace DathBomb.Models
{
    public class FlyingBombNameEffect : FlyingObjectEffect
    {
        private void Awake()
        {
            try {
                this.SetField<FlyingObjectEffect, AnimationCurve>("_moveAnimationCurve",
                    new AnimationCurve(new Keyframe(0f, 0f, 0f, 2f), new Keyframe(0.18f, 0.9f, 0f, 0.3f), new Keyframe(1f, 1f, 0f, 0f)));


                this._text = this.gameObject.AddComponent<TextMeshPro>();
                if (FontAssetReader.instance.MainFont != null) {
                    this._text.font = FontAssetReader.instance.MainFont;
                }
                this._text.alignment = TextAlignmentOptions.Center;
                this._text.fontSize = 30;
                this.gameObject.layer = 5;
            }
            catch (Exception e) {
                Logger.Error(e);
            }
        }
        public virtual void InitAndPresent(string text, float duration, Vector3 targetPos, Quaternion rotation, Color color, float fontSize, bool shake)
        {
            this.InitAndPresent(text, null, duration, targetPos, rotation, color, fontSize, shake);
        }

        public virtual void InitAndPresent(ImageView image, float duration, Vector3 targetPos, Quaternion rotation, bool shake)
        {
            this.InitAndPresent("", image, duration, targetPos, rotation, Color.white, 30, shake);
        }

        public virtual void InitAndPresent(string text, ImageView image, float duration, Vector3 targetPos, Quaternion rotation, Color color, float fontSize, bool shake)
        {
            this._image = image;
            this._color = color;
            this._text.text = text;
            this._text.fontSize = fontSize;
            base.InitAndPresent(duration, targetPos, rotation, shake);
        }

        protected override void ManualUpdate(float t) => this._text.color = this._color.ColorWithAlpha(this._fadeAnimationCurve.Evaluate(t));

        private TextMeshPro _text;
        private ImageView _image;
        private Color _color;
        private readonly AnimationCurve _fadeAnimationCurve = new AnimationCurve(new Keyframe(0f, 0f, 0f, 0f), new Keyframe(0.9f, 0.9f, -20f, 20f), new Keyframe(1f, 1f, 0f, 0f));
    }
}
