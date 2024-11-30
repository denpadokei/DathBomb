using BeatSaberMarkupLanguage;
using System;
using System.Collections;
using System.IO;
using TMPro;
using UnityEngine;

namespace DathBomb.Models
{
    public class FontAssetReader : MonoBehaviour
    {
        private static readonly string FontAssetPath = Path.Combine(Environment.CurrentDirectory, "UserData", "DathBombFontAssets");
        private static readonly string MainFontPath = Path.Combine(FontAssetPath, "Main");

        private static Shader _tmpNoGlowFontShader;
        public static Shader TMPNoGlowFontShader => _tmpNoGlowFontShader ?? (_tmpNoGlowFontShader = BeatSaberUI.MainTextFont?.material.shader);

        public bool IsInitialized { get; private set; } = false;

        private TMP_FontAsset _mainFont = null;

        public TMP_FontAsset MainFont
        {
            get
            {
                if (!this._mainFont)
                {
                    return null;
                }
                if (this._mainFont.material.shader != TMPNoGlowFontShader)
                {
                    this._mainFont.material.shader = TMPNoGlowFontShader;
                }
                return this._mainFont;
            }
            private set => this._mainFont = value;
        }
        public void Awake()
        {
            _ = this.StartCoroutine(this.CreateChatFont());
        }

        public IEnumerator CreateChatFont()
        {
            this.IsInitialized = false;
            yield return new WaitWhile(() => TMPNoGlowFontShader == null);
            if (this.MainFont != null)
            {
                Destroy(this.MainFont);
            }
            if (!Directory.Exists(MainFontPath))
            {
                _ = Directory.CreateDirectory(MainFontPath);
            }
            TMP_FontAsset asset = null;
            AssetBundle bundle = null;
            foreach (var filename in Directory.EnumerateFiles(MainFontPath, "*.assets", SearchOption.TopDirectoryOnly))
            {
                using (var fs = File.OpenRead(filename))
                {
                    bundle = AssetBundle.LoadFromStream(fs);
                }
                if (bundle != null)
                {
                    break;
                }
            }
            if (bundle != null)
            {
                foreach (var bundleItem in bundle.GetAllAssetNames())
                {
                    asset = bundle.LoadAsset<TMP_FontAsset>(Path.GetFileNameWithoutExtension(bundleItem));
                    if (asset != null)
                    {
                        this.MainFont = asset;
                        bundle.Unload(false);
                        break;
                    }
                }
            }

            this.IsInitialized = true;
        }
    }
}
