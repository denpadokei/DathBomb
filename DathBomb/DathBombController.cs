using DathBomb.Configuration;
using DathBomb.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using Zenject;

namespace DathBomb
{
    /// <summary>
    /// Monobehaviours (scripts) are added to GameObjects.
    /// For a full list of Messages a Monobehaviour can receive from the game, see https://docs.unity3d.com/ScriptReference/MonoBehaviour.html.
    /// </summary>
    public class DathBombController : MonoBehaviour
    {
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // プロパティ
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // コマンド
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // コマンド用メソッド
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // オーバーライドメソッド
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // パブリックメソッド
        [Inject]
        public void Constractor(IAudioTimeSource controller, BombEffectSpowner spowner, IReadonlyBeatmapData beatmapData)
        {
            this.source = controller;
            var objectData = beatmapData.allBeatmapDataItems.Where(x => x is NoteData).Select(x => x as NoteData).ToArray();
            this.noteCount = objectData.Length;
            var interval = Mathf.Floor(this.source.songEndTime / this.noteCount);
        }
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // プライベートメソッド
        private void CreateDefault()
        {
            var entites = new List<BombTextEntity>();
            var imageQueue = new ConcurrentQueue<string>();
            foreach (var imagePath in Directory.EnumerateFiles(_imageDirPath, "*.png", SearchOption.TopDirectoryOnly))
            {
                imageQueue.Enqueue(imagePath);
            }
            for (var i = 0; i < 60; i++)
            {
                _ = imageQueue.TryDequeue(out var imageName);
                for (var j = 0; j < 3; j++)
                {
                    if (j != 2 && !string.IsNullOrEmpty(imageName))
                    {
                        var entity = new BombTextEntity()
                        {
                            Index = j,
                            GroupingID = i,
                            Text = j == 0 ? "" : Path.GetFileNameWithoutExtension(imageName),
                            ImageName = j == 0 ? Path.GetFileName(imageName) : "",
                            TargetPos = new Vector3Wrapper(new Vector3(0f, 1.8f - (0.5f * j), 4f))
                        };
                        entites.Add(entity);
                    }
                    else
                    {
                        var entity = new BombTextEntity()
                        {
                            Index = j,
                            GroupingID = i,
                            Text = "この辺になんか説明とか",
                            ImageName = "",
                            TargetPos = new Vector3Wrapper(new Vector3(0f, 1.8f - (0.5f * j), 4f))
                        };
                        entites.Add(entity);
                    }
                }
            }
            var jsonEntity = new BombJsonEntity(entites);
            var defaultJson = JsonConvert.SerializeObject(jsonEntity, Formatting.Indented);
            File.WriteAllText(_jsonPath, defaultJson);
        }
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // メンバ変数
        private IAudioTimeSource source;
        private static readonly string _dirPath = Path.Combine(Environment.CurrentDirectory, "UserData", "DathBomb");
        private static readonly string _imageDirPath = Path.Combine(_dirPath, "Images");
        private static readonly string _jsonPath = Path.Combine(_dirPath, "Staff.json");
        private int noteCount;
        private float nextInterval;
        private DateTime _lastSendTime;
        private bool _enable;
        private readonly ConcurrentQueue<IGrouping<int, BombTextEntity>> _entities = new ConcurrentQueue<IGrouping<int, BombTextEntity>>();
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // 構築・破棄
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region Monobehaviour Messages
        // These methods are automatically called by Unity, you should remove any you aren't using.

        /// <summary>
        /// Only ever called once, mainly used to initialize variables.
        /// </summary>
        private void Awake()
        {
            // For this particular MonoBehaviour, we only want one instance to exist at any time, so store a reference to it in a static property
            //   and destroy any that are created while one already exists.
            Plugin.Log?.Debug($"{this.name}: Awake()");
            this._lastSendTime = DateTime.Now;
            this.nextInterval = 0f;
            while (this._entities.TryDequeue(out _))
            {
            }
            while (DummyBomb.Senders.TryDequeue(out _))
            {
            }
            this._enable = PluginConfig.Instance.IsBombEnable;
            if (!this._enable)
            {
                return;
            }
            if (!Directory.Exists(_dirPath))
            {
                _ = Directory.CreateDirectory(_dirPath);
            }
            if (!File.Exists(_jsonPath))
            {
                this.CreateDefault();
            }

            var jsonText = File.ReadAllText(_jsonPath);
            var bombJson = JsonConvert.DeserializeObject<BombJsonEntity>(jsonText);

            foreach (var eneity in bombJson.Staff.GroupBy(x => x.GroupingID).OrderBy(x => x.Key))
            {
                this._entities.Enqueue(eneity);
            }
        }

        private void Update()
        {
            if (!this._enable)
            {
                return;
            }
            if (this.source.songTime == 0)
            {
                this._lastSendTime = DateTime.Now;
                return;
            }
            if ((DateTime.Now - this._lastSendTime).Seconds < this.nextInterval)
            {
                return;
            }
            if (this._entities.TryDequeue(out var entity))
            {
                foreach (var item in entity.OrderBy(x => x.Index))
                {
                    DummyBomb.Senders.Enqueue(item);
                    this.nextInterval = item.Interval;
                    this._lastSendTime = DateTime.Now;
                }
            }
        }

        /// <summary>
        /// Called when the script is being destroyed.
        /// </summary>
        private void OnDestroy()
        {
            Plugin.Log?.Debug($"{this.name}: OnDestroy()");
        }
        #endregion
    }
}
