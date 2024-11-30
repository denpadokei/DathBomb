using HMUI;
using IPA.Utilities;
using System;
using System.Linq;
using TMPro;
using UnityEngine;
using Zenject;

namespace DathBomb.Models
{
    public class FlyingBombNameEffect : FlyingObjectEffect
    {

        public void Initialize()
        {
            try
            {
                this.SetField<FlyingObjectEffect, AnimationCurve>("_moveAnimationCurve",
                    new AnimationCurve(new Keyframe(0f, 0f, 0f, 2f), new Keyframe(0.18f, 1f, 0f, 0.3f), new Keyframe(1f, 1f, 0f, 0f)));

                if (this._noGlow == null)
                {
                    this._noGlow = Instantiate(Resources.FindObjectsOfTypeAll<Material>().FirstOrDefault(x => x.name == "UINoGlow"));
                }

                this._rootCanvas = this.gameObject.GetComponent<Canvas>();
                this._rootCanvas.transform.localPosition = Vector3.zero;
                this._rootCanvas.transform.localRotation = Quaternion.identity;
                this._rootCanvas.renderMode = RenderMode.WorldSpace;
                this._rootCanvas.gameObject.layer = 5;
                this._text = new GameObject("text", typeof(TextMeshPro)).GetComponent<TextMeshPro>();
                this._text.rectTransform.SetParent(this._rootCanvas.transform as RectTransform, false);
                if (this._fontAssetReader.MainFont != null)
                {
                    this._text.font = this._fontAssetReader.MainFont;
                }
                this._text.alignment = TextAlignmentOptions.Center;
                this._text.fontSize = 30;

                this._image = new GameObject("image", typeof(ImageView)).GetComponent<ImageView>();
                this._image.rectTransform.SetParent(this._rootCanvas.transform as RectTransform, false);
                this._image.rectTransform.localScale = Vector3.one * 0.005f;
                this._image.rectTransform.localPosition = Vector3.zero;
                this._image.material = this._noGlow;
                this._image.enabled = false;
                this._image.color = new Color(1, 1, 1, 1);
            }
            catch (Exception e)
            {
                Logger.Error(e);
            }
        }

        public void OnDestroy()
        {
            if (this._text != null)
            {
                Destroy(this._text);
            }
            if (this._image != null)
            {
                Destroy(this._image);
            }
            if (this._noGlow != null)
            {
                Destroy(this._noGlow);
            }
        }

        public virtual void InitAndPresent(string text, float duration, Vector3 targetPos, Quaternion rotation, Color color, float fontSize, bool shake)
        {
            this.InitAndPresent(text, null, duration, targetPos, rotation, color, fontSize, 0.005f, shake);
        }

        public virtual void InitAndPresent(Sprite image, float duration, Vector3 targetPos, Quaternion rotation, float imageScale, bool shake)
        {
            this.InitAndPresent("", image, duration, targetPos, rotation, Color.white, 30, imageScale, shake);
        }

        public virtual void InitAndPresent(string text, Sprite image, float duration, Vector3 targetPos, Quaternion rotation, Color color, float fontSize, float imageScale, bool shake)
        {
            if (image != null)
            {
                this._image.rectTransform.sizeDelta = new Vector2(image.texture.width, image.texture.height);
                this._image.rectTransform.localScale = Vector3.one * imageScale;
                this._image.sprite = image;
                this._image.enabled = true;
                this._image.SetAllDirty();
            }
            else
            {
                this._image.enabled = false;
            }

            this._textcolor = color;
            this._text.text = text;
            this._text.fontSize = fontSize;
            base.InitAndPresent(duration, targetPos, rotation, shake);
        }

        protected override void ManualUpdate(float t)
        {
            this._text.color = this._textcolor.ColorWithAlpha(this._fadeAnimationCurve.Evaluate(t));
            this._image.color = Color.white.ColorWithAlpha(this._fadeAnimationCurve.Evaluate(t));
        }

        [Inject]
        public void Constractor(FontAssetReader fontAssetReader)
        {
            this._fontAssetReader = fontAssetReader;
        }

        private Canvas _rootCanvas;
        private TextMeshPro _text;
        private ImageView _image;
        private Material _noGlow;
        private Color _textcolor;
        private readonly AnimationCurve _fadeAnimationCurve = new AnimationCurve(new Keyframe(0f, 1f, 0f, 0f), new Keyframe(0.9f, 1f, 0f, -20f), new Keyframe(1f, 0f, 20f, 0f));
        private FontAssetReader _fontAssetReader;

        public class Pool : MemoryPool<FlyingBombNameEffect>
        {
            protected override void OnCreated(FlyingBombNameEffect item)
            {
                base.OnCreated(item);
                item.Initialize();

            }
        }
    }
}
