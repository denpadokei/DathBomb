using System;
using System.Collections.Concurrent;
using System.IO;
using UnityEngine;
using Zenject;

namespace DathBomb.Utilities
{
    public class ImageLoader : MonoBehaviour, IInitializable
    {
        private static readonly string _dirPath = Path.Combine(Environment.CurrentDirectory, "UserData", "DathBomb");
        private static readonly string _imageDirPath = Path.Combine(_dirPath, "Images");
        public ConcurrentDictionary<string, Sprite> Images { get; } = new ConcurrentDictionary<string, Sprite>();

        private Texture2D CreateTextuer2D(byte[] datas, ImageExtention extention)
        {
            this.GetImageSize(datas, extention, out var width, out var height);
            var result = new Texture2D(width, height, TextureFormat.ARGB32, false, true);
            _ = result.LoadImage(datas);
            return result;
        }

        private void GetImageSize(byte[] datas, ImageExtention extention, out int width, out int height)
        {
            width = 0;
            height = 0;

            switch (extention)
            {
                case ImageExtention.JPEG:
                    for (var i = 0; i < datas.Length; i++)
                    {
                        if (datas[i] == 0xff)
                        {
                            if (datas[i + 1] == 0xc0)
                            {
                                height = (datas[i + 5] * 256) + datas[i + 6];
                                width = (datas[i + 7] * 256) + datas[i + 8];
                                break;
                            }
                        }
                    }
                    break;
                case ImageExtention.PNG:
                    var buf = new byte[8];
                    using (var ms = new MemoryStream(datas))
                    {
                        _ = ms.Seek(16, SeekOrigin.Begin);
                        _ = ms.Read(buf, 0, 8);
                    }
                    width = (buf[0] << 24) | (buf[1] << 16) | (buf[2] << 8) | buf[3];
                    height = (buf[4] << 24) | (buf[5] << 16) | (buf[6] << 8) | buf[7];
                    break;
                default:
                    break;
            }
        }

        public void OnDestroy()
        {
            this.Images.Clear();
        }

        public void Initialize()
        {
            this.Images.Clear();
            if (!Directory.Exists(_imageDirPath))
            {
                _ = Directory.CreateDirectory(_imageDirPath);
            }
            foreach (var imagePath in Directory.EnumerateFiles(_imageDirPath, "*.png", SearchOption.TopDirectoryOnly))
            {
                var datas = File.ReadAllBytes(imagePath);
                _ = Path.GetExtension(imagePath);
                var extentionType = ImageExtention.PNG;
                var textuer = this.CreateTextuer2D(datas, extentionType);
                var createdSprite = Sprite.Create(textuer, new Rect(0, 0, textuer.width, textuer.height), Vector2.one * 0.5f);
                _ = this.Images.TryAdd(Path.GetFileName(imagePath), createdSprite);
            }
        }

        public enum ImageExtention
        {
            JPEG,
            PNG
        }
    }
}
