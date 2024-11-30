using System;
using System.Collections;
using System.IO;
using System.Linq;
using UnityEngine;

namespace DathBomb.Models
{
    public class ParticleAssetLoader : MonoBehaviour
    {
        private static readonly string FontAssetPath = Path.Combine(Environment.CurrentDirectory, "UserData", "SPCParticleAssets");

        private static Material _default;

        private static Material Default => _default ?? (_default = Resources.FindObjectsOfTypeAll<Material>().FirstOrDefault(x => x.name == "FireworkExplosion"));

        public bool IsInitialized { get; private set; } = false;

        public ParticleSystem Particle { get; private set; } = null;
        public void Awake()
        {
            _ = this.StartCoroutine(this.LoadParticle());
        }

        public IEnumerator LoadParticle()
        {
            this.IsInitialized = false;
            yield return new WaitWhile(() => Default == null);
            if (this.Particle != null)
            {
                Destroy(this.Particle);
            }
            if (!Directory.Exists(FontAssetPath))
            {
                _ = Directory.CreateDirectory(FontAssetPath);
            }
            AssetBundle bundle = null;
            foreach (var filename in Directory.EnumerateFiles(FontAssetPath, "*.particle", SearchOption.TopDirectoryOnly))
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
                    var asset = bundle.LoadAsset<GameObject>(Path.GetFileNameWithoutExtension(bundleItem));
                    if (asset != null)
                    {
                        this.Particle = asset.GetComponent<ParticleSystem>();
                        var renderer = this.Particle.GetComponent<ParticleSystemRenderer>();
                        renderer.material = Instantiate(Default);
                        bundle.Unload(false);
                        break;
                    }
                }
            }
            this.IsInitialized = true;
        }
    }
}
