using IPA.Utilities;
using DathBomb.HarmonyPathches;
using DathBomb.Utilities;
using System.Collections.Concurrent;
using UnityEngine;

namespace DathBomb.Models
{
    public class DummyBomb : MonoBehaviour, INoteControllerDidInitEvent
    {
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // プロパティ
        public GameNoteController Controller { get; private set; }
        public BombTextEntity BombInfo { get; set; }
        public static ConcurrentQueue<BombTextEntity> Senders { get; } = new ConcurrentQueue<BombTextEntity>();
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // イベント
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // コマンド用メソッド
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // オーバーライドメソッド
        protected void Awake()
        {
            if (CustomNoteUtil.TryGetGameNoteController(this.gameObject, out var component)) {
                this.Controller = component;
                this.Controller.didInitEvent.Add(this);
            }
            this._noteCube = this.gameObject.transform.Find("NoteCube");
            if (CustomNoteUtil.TryGetColorNoteVisuals(this.gameObject, out var visuals)) {
                this._colorManager = visuals.GetField<ColorManager, ColorNoteVisuals>("_colorManager");
            }
            this._noteMesh = this.GetComponentInChildren<MeshRenderer>();
        }
        protected void OnDestroy()
        {
            if (this.Controller != null) {
                this.Controller.didInitEvent.Remove(this);
            }
            if (this._bombMesh != null) {
                Destroy(this._bombMesh);
            }
        }
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // パブリックメソッド
        public void HandleNoteControllerDidInit(NoteControllerBase noteController)
        {
            if (this.Controller.gameNoteType == GameNoteController.GameNoteType.Ghost) {
                return;
            }
            // ここで置き換えるノーツの設定をする（ボムにするとかなんとか）
            // ふぁっきんかすたむのーつ
            if (CustomNoteUtil.IsInstallCustomNote && 1 <= CustomNoteUtil.SelectedNoteIndex) {
                if (Senders.TryDequeue(out var sender)) {
                    this.BombInfo = sender;
                    if (this._bombMesh == null && BombMeshGetter.BombMesh != null) {
                        this._bombMesh = Instantiate(BombMeshGetter.BombMesh);
                        this._bombMesh.gameObject.transform.SetParent(this._noteCube, false);
                    }
                    if (this._bombMesh != null) {
                        this._bombMesh.enabled = true;
                        var color = this._colorManager.ColorForType(noteController.noteData.colorType);
                        this._bombMesh.material.SetColor("_SimpleColor", color);
                    }
                }
                else {
                    this.BombInfo = null;
                    if (this._bombMesh != null) {
                        this._bombMesh.enabled = false;
                    }
                }
            }
            else {
                if (this._bombMesh == null && BombMeshGetter.BombMesh != null) {
                    this._bombMesh = Instantiate(BombMeshGetter.BombMesh);
                    this._bombMesh.gameObject.transform.SetParent(this._noteCube, false);
                }
                if (Senders.TryDequeue(out var sender)) {
                    this.BombInfo = sender;
                    this._noteMesh.forceRenderingOff = true;
                    if (this._bombMesh != null) {
                        this._bombMesh.enabled = true;
                    }
                }
                else {
                    this._noteMesh.forceRenderingOff = false;
                    if (this._bombMesh != null) {
                        this._bombMesh.enabled = false;
                    }
                }
                var color = this._colorManager.ColorForType(noteController.noteData.colorType);
                if (this._bombMesh != null) {
                    this._bombMesh.material.SetColor("_SimpleColor", color);
                }
            }
        }
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // プライベートメソッド
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // メンバ変数
        private Transform _noteCube;
        [DoesNotRequireDomainReloadInit]
        protected static readonly int _noteColorId = Shader.PropertyToID("_Color");
        protected static readonly int _bombColorId = Shader.PropertyToID("_SimpleColor");
        private MeshRenderer _bombMesh;
        private MeshRenderer _noteMesh;
        private ColorManager _colorManager;
        #endregion
        //ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*ﾟ+｡｡+ﾟ*｡+ﾟ ﾟ+｡*
        #region // 構築・破棄
        #endregion
    }
    public delegate void NoteWasCutEventHandler(GameNoteController controller, in NoteCutInfo noteCutInfo);
}
