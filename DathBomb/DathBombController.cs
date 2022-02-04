using DathBomb.Configuration;
using DathBomb.Models;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        public void Constractor(IAudioTimeSource controller, BombEffectSpowner spowner, GameplayCoreSceneSetupData data)
        {
            this.source = controller;
            var diff = data.difficultyBeatmap;
            var objectData = diff.beatmapData.beatmapObjectsData.Where(x => x is NoteData).Select(x => x as NoteData).ToArray();
            noteCount = objectData.Length;
            var interval = Mathf.Floor(this.source.songEndTime / noteCount);
        }
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // プライベートメソッド
        private void CreateDefault()
        {
            var entites = new List<BombTextEntity>();
            var imageQueue = new ConcurrentQueue<string>();
            foreach (var imagePath in Directory.EnumerateFiles(_imageDirPath, "*.png", SearchOption.TopDirectoryOnly)) {
                imageQueue.Enqueue(imagePath);
            }
            for (int i = 0; i < 60; i++) {
                imageQueue.TryDequeue(out var imageName);
                for (int j = 0; j < 3; j++) {
                    if (j != 2 && !string.IsNullOrEmpty(imageName)) {
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
                    else {
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
        private ConcurrentQueue<IGrouping<int, BombTextEntity>> _entities = new ConcurrentQueue<IGrouping<int, BombTextEntity>>();
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
            Plugin.Log?.Debug($"{name}: Awake()");
            _lastSendTime = DateTime.Now;
            nextInterval = 0f;
            while (_entities.TryDequeue(out _)) {

            }
            while (DummyBomb.Senders.TryDequeue(out _)) {

            }
            if (!PluginConfig.Instance.IsBombEnable) {
                return;
            }
            if (!Directory.Exists(_dirPath)) {
                Directory.CreateDirectory(_dirPath);
            }
            if (!File.Exists(_jsonPath)) {
                this.CreateDefault();
            }

            var jsonText = File.ReadAllText(_jsonPath);
            var bombJson = JsonConvert.DeserializeObject<BombJsonEntity>(jsonText);

            foreach (var eneity in bombJson.Staff.GroupBy(x => x.GroupingID).OrderBy(x => x.Key)) {
                _entities.Enqueue(eneity);
            }
        }

        private void Update()
        {
            if (source.songTime == 0) {
                _lastSendTime = DateTime.Now;
                return;
            }
            if ((DateTime.Now - _lastSendTime).Seconds < nextInterval) {
                return;
            }
            if (_entities.TryDequeue(out var entity)) {
                foreach (var item in entity.OrderBy(x => x.Index)) {
                    DummyBomb.Senders.Enqueue(item);
                    nextInterval = item.Interval;
                    _lastSendTime = DateTime.Now;
                }
            }
        }

        /// <summary>
        /// Called when the script is being destroyed.
        /// </summary>
        private void OnDestroy()
        {
            Plugin.Log?.Debug($"{name}: OnDestroy()");
        }
        #endregion
    }
}
