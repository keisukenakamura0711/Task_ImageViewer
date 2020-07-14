using System;
using System.Threading;
using System.Timers;

namespace Task_ImageViewer.ViewModels
{
    /// <summary>
    /// MainViewウインドウに対するデータコンテキストを表します。
    /// </summary>
    internal class MainViewModel : NotificationObject
    {
        private string _FilePath;
        /// <summry>
        /// 割られる数に指定される文字列を取得または設定します
        /// </summry>
        public string FilePath
        {
            get { return this._FilePath; }
            set { SetProperty(ref this._FilePath, value); }
        }

        private DelegateCommand _openFileCommand;
        /// <summary>
        /// ファイルを開くコマンドを取得します。
        /// </summary>
        public DelegateCommand OpenFileCommand
        {
            get
            {
                return this._openFileCommand ?? (this._openFileCommand = new DelegateCommand(_ =>
                {
                    this.DialogCallback = OnDialogCallback;
                }));
            }
        }

        private Action<bool, string> _dialogCallback;
        /// <summary>
        /// ダイアログに対するコールバックを取得します。
        /// </summary>
        public Action<bool, string> DialogCallback
        {
            get { return this._dialogCallback; }
            private set { SetProperty(ref this._dialogCallback, value); }
        }

        private void OnDialogCallback(bool isOk, string filePath)
        {
            this.DialogCallback = null;
            this.FilePath = filePath;
        }

        #region アプリケーションを終了する
        public Func<bool> ClosingCallback
        {
            get { return OnExit; }
        }

        private DelegateCommand _exitCommand;
        /// <summary>
        /// アプリケーション終了コマンドを取得します。
        /// </summary>
        public DelegateCommand ExitCommand
        {
            get
            {
                return this._exitCommand ?? (this._exitCommand = new DelegateCommand(_ => { OnExit(); }));
            }
        }

        /// <summary>
        /// アプリケーションを終了します。
        /// </summary>
        private bool OnExit()
        {
            App.Current.Shutdown();
            return true;
        }
        #endregion アプリケーションを終了する

        #region イメージを表示する
        private ImageViewModel _ImageViewModel = new ImageViewModel();
        public ImageViewModel ImageViewModel
        {
            get { return this._ImageViewModel; }
        }

        private DelegateCommand _ImageDialogCommand;
        /// <summary>
        /// イメージ情報表示コマンドを取得します。
        /// </summary>
        public DelegateCommand ImageDialogCommand
        {
            get
            {
                return this._ImageDialogCommand ?? (this._ImageDialogCommand = new DelegateCommand(_ =>
                {
                    ImageViewModel.ImageViewFile = this.FilePath;
                    this.ImageDialogCallback = this.OnImageCallback;
                }));
            }
        }

        private Action<bool> _ImageDialogCallback;
        /// <summary>
        /// イメージ情報表示コールバックを取得します。
        /// </summary>
        public Action<bool> ImageDialogCallback
        {
            get { return this._ImageDialogCallback; }
            private set { SetProperty(ref this._ImageDialogCallback, value); }
        }

        /// <summary>
        /// イメージ情報表示コールバック処理をおこないます。
        /// </summary>
        /// <param name="result"></param>
        private void OnImageCallback(bool result)
        {
            this.ImageDialogCallback = null;
        }
        #endregion イメージ情報を表示する

        #region バージョン情報を表示する
        private VersionViewModel _versionViewModel = new VersionViewModel();
        public VersionViewModel VersionViewModel
        {
            get { return this._versionViewModel; }
        }

        private DelegateCommand _versionDialogCommand;
        /// <summary>
        /// バージョン情報表示コマンドを取得します。
        /// </summary>
        public DelegateCommand VersionDialogCommand
        {
            get
            {
                return this._versionDialogCommand ?? (this._versionDialogCommand = new DelegateCommand(_ =>
                {
                    this.VersionDialogCallback = OnVersionCallback;
                }));
            }
        }

        private Action<bool> _versionDialogCallback;
        /// <summary>
        /// バージョン情報表示コールバックを取得します。
        /// </summary>
        public Action<bool> VersionDialogCallback
        {
            get { return this._versionDialogCallback; }
            private set { SetProperty(ref this._versionDialogCallback, value); }
        }

        /// <summary>
        /// バージョン情報表示コールバック処理をおこないます。
        /// </summary>
        /// <param name="result"></param>
        private void OnVersionCallback(bool result)
        {
            this.VersionDialogCallback = null;
        }
        #endregion バージョン情報を表示する
    }
}
