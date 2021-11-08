using HMUI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace DathBomb.Utilities
{
    public class ImageLoader : PersistentSingleton<ImageLoader>
    {
        private static readonly string _dirPath = Path.Combine(Environment.CurrentDirectory, "UserData", "DathBomb");
        private static readonly string _imageDirPath = Path.Combine(_dirPath, "Images");
        public ConcurrentDictionary<string, Texture2D> Images { get; } = new ConcurrentDictionary<string, Texture2D>();

        private Texture2D CreateTextuer2D(byte[] datas, ImageExtention extention)
        {
            this.GetImageSize(datas, extention, out var width, out var height);
            var result = new Texture2D(width, height, TextureFormat.ARGB32, false, true);
            result.LoadImage(datas);
            return result;
        }

        private void GetImageSize(byte[] datas, ImageExtention extention, out int width, out int height)
        {
            width = 0;
            height = 0;

            switch (extention) {
                case ImageExtention.JPEG:
                    for (var i = 0; i < datas.Length; i++) {
                        if (datas[i] == 0xff) {
                            if (datas[i + 1] == 0xc0) {
                                height = datas[i + 5] * 256 + datas[i + 6];
                                width = datas[i + 7] * 256 + datas[i + 8];
                                break;
                            }
                        }
                    }
                    break;
                case ImageExtention.PNG:
                    var buf = new byte[8];
                    using (var ms = new MemoryStream(datas)) {
                        ms.Seek(16, SeekOrigin.Begin);
                        ms.Read(buf, 0, 8);
                    }
                    width = (buf[0] << 24) | (buf[1] << 16) | (buf[2] << 8) | buf[3];
                    height = (buf[4] << 24) | (buf[5] << 16) | (buf[6] << 8) | buf[7];
                    break;
                default:
                    break;
            }
        }

        public void Awake()
        {
            this.Images.Clear();
            if (!Directory.Exists(_imageDirPath)) {
                Directory.CreateDirectory(_imageDirPath);
            }
            foreach (var imagePath in Directory.EnumerateFiles(_imageDirPath, "*.png", SearchOption.TopDirectoryOnly)) {
                byte[] datas = null;
                datas = File.ReadAllBytes(imagePath);
                var extention = Path.GetExtension(imagePath);
                var extentionType = ImageExtention.PNG;
                var textuer = this.CreateTextuer2D(datas, extentionType);
                this.Images.TryAdd(Path.GetFileName(imagePath), textuer);
            }
        }

        protected override void OnDestroy()
        {
            base.OnDestroy();
            Images.Clear();
        }

        public enum ImageExtention
        {
            JPEG,
            PNG
        }
    }
}
