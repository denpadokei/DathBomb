using DathBomb.Utilities;
using System;
using UnityEngine;
using Zenject;

namespace DathBomb.Models
{
    public class BombEffectSpowner : MonoBehaviour, IFlyingObjectEffectDidFinishEvent
    {
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // プロパティ
        public int TextViewTime { get; set; }
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
        public void Start()
        {
            this._beatmapObjectManager.noteWasCutEvent += this.OnNoteWasCutEvent;
            this._beatmapObjectManager.noteWasMissedEvent += this.OnNoteWasMissedEvent;
        }
        public void OnDestroy()
        {
            this._beatmapObjectManager.noteWasCutEvent -= this.OnNoteWasCutEvent;
            this._beatmapObjectManager.noteWasMissedEvent -= this.OnNoteWasMissedEvent;
        }

        public void HandleFlyingObjectEffectDidFinish(FlyingObjectEffect flyingObjectEffect)
        {
            flyingObjectEffect.didFinishEvent.Remove(this);
            this._flyingBombNameEffectPool.Free(flyingObjectEffect as FlyingBombNameEffect);
        }
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // プライベートメソッド
        private void OnNoteWasCutEvent(NoteController noteController, in NoteCutInfo noteCutInfo)
        {
            var dummyBomb = noteController.gameObject.GetComponent<DummyBomb>();
            if (dummyBomb == null) {
                return;
            }
            if (dummyBomb.BombInfo == null) {
                return;
            }
            //this._dummyBombExprosionEffect.SpawnExplosion(noteCutInfo.cutPoint);
            var effect = this._flyingBombNameEffectPool.Alloc();
            effect.transform.localPosition = noteCutInfo.cutPoint;
            effect.didFinishEvent.Add(this);
            var targetpos = noteController.worldRotation * (new Vector3(0, 1.7f, 10f));
            try {
                if (ImageLoader.instance.Images.TryGetValue(dummyBomb.BombInfo.ImageName, out var texture)) {
                    effect.InitAndPresent(dummyBomb.BombInfo.Text, texture, dummyBomb.BombInfo.ViewTime, dummyBomb.BombInfo.TargetPos.ConvertToVector3(), noteController.worldRotation, Color.white, dummyBomb.BombInfo.FontSize, dummyBomb.BombInfo.ImageScale, false);
                }
                else {
                    effect.InitAndPresent(dummyBomb.BombInfo.Text, dummyBomb.BombInfo.ViewTime, dummyBomb.BombInfo.TargetPos.ConvertToVector3(), noteController.worldRotation, Color.white, dummyBomb.BombInfo.FontSize, false);
                }
            }
            catch (Exception e) {
                Logger.Error(e);
            }
            dummyBomb.BombInfo = null;
        }
        private void OnNoteWasMissedEvent(NoteController noteController)
        {
            var dummyBomb = noteController.gameObject.GetComponent<DummyBomb>();
            if (dummyBomb == null) {
                return;
            }
            if (dummyBomb.BombInfo == null) {
                return;
            }
            var effect = this._flyingBombNameEffectPool.Alloc();
            effect.transform.localPosition = Vector3.zero;
            effect.didFinishEvent.Add(this);
            var targetpos = noteController.worldRotation * (new Vector3(0, 1.7f, 10f));
            effect.InitAndPresent(dummyBomb.BombInfo.Text, dummyBomb.BombInfo.ViewTime, dummyBomb.BombInfo.TargetPos.ConvertToVector3(), noteController.worldRotation, Color.white, dummyBomb.BombInfo.FontSize, false);
            dummyBomb.BombInfo = null;
        }
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // メンバ変数
        private BeatmapObjectManager _beatmapObjectManager;
        //private DummyBombExprosionEffect _dummyBombExprosionEffect;
        private ObjectMemoryPool<FlyingBombNameEffect> _flyingBombNameEffectPool;
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // 構築・破棄
        [Inject]
        public void Constractor(BeatmapObjectManager manager)
        {
            this._beatmapObjectManager = manager;
            this._flyingBombNameEffectPool = new ObjectMemoryPool<FlyingBombNameEffect>(8);
        }
        #endregion
    }
}