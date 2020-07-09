using System;
using System.Threading;
using System.Timers;

namespace WPFApp_Test3.ViewModels
{
    /// <summary>
    /// MainViewウインドウに対するデータコンテキストを表します。
    /// </summary>
    internal class MainViewModel : NotificationObject
    {
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
            System.Diagnostics.Debug.WriteLine("コールバック処理をおこないます。");
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
            System.Diagnostics.Debug.WriteLine(result);
        }
        #endregion バージョン情報を表示する

        #region 現在時刻を取得する
        /// <summary>
        /// 現在時刻を更新するためのタイマー
        /// </summary>
        private System.Timers.Timer _timer;

        private DateTime _currentTime;
        public DateTime CurrentTime
        {
            get
            {
                if (this._timer == null)
                {
                    this._currentTime = DateTime.Now;

                    this._timer = new System.Timers.Timer(1000);
                    this._timer.Elapsed += (_, __) =>
                    {
                        this._currentTime = DateTime.Now;
                    };
                    this._timer.Start();
                }
                return this._currentTime;
            }
            private set { SetProperty(ref this._currentTime, value); }
        }
        #endregion 現在時刻を取得する
    }
}
