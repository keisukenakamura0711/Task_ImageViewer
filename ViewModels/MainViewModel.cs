using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using System.Windows.Controls;
using System.Linq;

namespace Task_ImageViewer.ViewModels
{
    class ImageData
    {
        public string FullPath { get; set; }
        public bool Selected { get; set; }
    }

    /// <summary>
    /// MainViewウインドウに対するデータコンテキストを表します。
    /// </summary>
    internal class MainViewModel : NotificationObject
    {
        private readonly string[] IMAGE_EXTENSION = { ".bmp", ".jpg", ".png" };

        #region データパスの入力
        private string _imagePath = "";
        /// <summry>
        /// データパスの設定、または
        /// </summry>
        public string ImagePath
        {
            get { return this._imagePath; }
            set
            {
                SetProperty(ref this._imagePath, value);
                this.UpdateImageList();
            }
        }

        /// <summary>
        /// 画像ファイルがデータパスに指定されているか
        /// </summary>
        /// <returns></returns>
        public bool IsImagePathInputImageData()
        {
            foreach (string extension in IMAGE_EXTENSION)
            {
                if (this.ImagePath.Contains(extension)) { return true; }
            }
            return false;
        }

        #endregion データパスの入力

        #region 画像リスト表示
        private ObservableCollection<ImageData> _ImageDataList = new ObservableCollection<ImageData>();
        public ObservableCollection<ImageData> ImageDataList
        {
            get { return this._ImageDataList; }
            set { SetProperty(ref this._ImageDataList, value); }
        }
        public void UpdateImageList()
        {
            this.ImageDataList.Clear();

            if (IsImagePathInputImageData())
            {
                var imageData = new ImageData
                {
                    FullPath = this.ImagePath
                };
                this.ImageDataList.Add(imageData);
            }
            else if (Directory.Exists(this.ImagePath))
            {
                var tempFolderImage = new List<string>();
                foreach (string extension in IMAGE_EXTENSION)
                {
                    tempFolderImage.AddRange(System.IO.Directory.GetFiles(this.ImagePath, "*" + extension));
                }
                tempFolderImage.Sort();

                foreach (string imageFullPath in tempFolderImage)
                {
                    var imageData = new ImageData
                    {
                        FullPath = imageFullPath
                    };
                    this.ImageDataList.Add(imageData);
                }
            }
        }
        /// <summary>
        /// イメージを表示するリスト番号を取得します。
        /// </summary>
        /// <returns>リスト番号</returns>
        /// 
        /// 現在マルチセレクト不可だが後にマルチセレクトを実施する予定
        public int GetOpenImageDialogListNo()
        {
            // 画像指定の場合は選択されていなくても指定されていることとし番号を返す
            if (this.ImageDataList.Count == 1) { return 0; }

            for (var i = 0; i < this.ImageDataList.Count; i++)
            {
                if (this.ImageDataList[i].Selected) { return i; }
            }
            return -1;
        }

        #endregion 画像リスト表示

        #region ファイルを開く
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
            this.ImagePath = filePath;
        }
        #endregion ファイルを開く

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
                return this._ImageDialogCommand ?? (this._ImageDialogCommand = new DelegateCommand(
                _ =>
                {
                    ImageViewModel.ImageViewFile = ImageDataList[this.GetOpenImageDialogListNo()].FullPath;
                    this.ImageDialogCallback = this.OnImageCallback;
                },
                _ => this.GetOpenImageDialogListNo() >= 0));
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
