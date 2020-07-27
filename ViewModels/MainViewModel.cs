using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Windows.Input;
using System.Windows.Controls;
using System.Linq;
using System.Collections.Specialized;
using System.Windows.Media.Imaging;

namespace Task_ImageViewer.ViewModels
{
    class ImageData
    {
        public string FullPath { get; set; }
        public BitmapImage Thumbnail { get; set; }
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
        /// データパスの設定
        /// </summry>
        public string ImagePath
        {
            get { return this._imagePath; }
            set
            {
                SetProperty(ref this._imagePath, value);
                if (this.UpdateImageList())
                {
                    this.ImagePathHistory.Add(this._imagePath);
                }
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

        private StringCollection _imagePathHistory;
        public StringCollection ImagePathHistory
        {
            get { return this._imagePathHistory; }
            set { SetProperty(ref this._imagePathHistory, value); }
        }

        #endregion データパスの入力

        #region 画像リスト表示
        private ObservableCollection<ImageData> _ImageDataList = new ObservableCollection<ImageData>();
        public ObservableCollection<ImageData> ImageDataList
        {
            get { return this._ImageDataList; }
            set { SetProperty(ref this._ImageDataList, value); }
        }
        /// <summary>
        /// 画像リスト更新
        /// </summary>
        /// <returns></returns>
        public bool UpdateImageList()
        {
            this.ImageDataList.Clear();

            if (IsImagePathInputImageData())
            {
                var imageData = new ImageData
                {
                    FullPath = this.ImagePath,
                    Thumbnail = createThumbnail(this.ImagePath),
                };
                this.ImageDataList.Add(imageData);

                return true;
            }
            
            if (Directory.Exists(this.ImagePath))
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
                        FullPath = imageFullPath,
                        Thumbnail = createThumbnail(imageFullPath),
                    };
                    this.ImageDataList.Add(imageData);
                }
                if (tempFolderImage.Count > 0){ return true; }
            }

            return false;
        }
        /// <summary>
        /// サムネイルを作成します。
        /// </summary>
        /// <param name="filePath">サムネイルを作成するファイルパス</param>
        /// <returns>BitmapImage</returns>
        BitmapImage createThumbnail(String filePath)
        {
            BitmapImage bmpImg = new BitmapImage();
            FileStream fileStream = File.OpenRead(filePath);

            bmpImg.BeginInit();
            bmpImg.CacheOption = BitmapCacheOption.OnLoad;
            bmpImg.StreamSource = fileStream;
            bmpImg.EndInit();
            fileStream.Close();

            return bmpImg;
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
                    this.FileDialogCallback = OnFileDialogCallback;
                }));
            }
        }

        private Action<bool, string> _fileDialogCallback;
        /// <summary>
        /// ダイアログに対するコールバックを取得します。
        /// </summary>
        public Action<bool, string> FileDialogCallback
        {
            get { return this._fileDialogCallback; }
            private set { SetProperty(ref this._fileDialogCallback, value); }
        }

        private void OnFileDialogCallback(bool isOk, string filePath)
        {
            this.FileDialogCallback = null;
            if (isOk)
            {
                this.ImagePath = filePath;
            }
        }
        #endregion ファイルを開く

        #region フォルダを開く
        private DelegateCommand _openFolderCommand;
        /// <summary>
        /// ファイルを開くコマンドを取得します。
        /// </summary>
        public DelegateCommand OpenFolderCommand
        {
            get
            {
                return this._openFolderCommand ?? (this._openFolderCommand = new DelegateCommand(_ =>
                {
                    this.FolderDialogCallback = OnFolderDialogCallback;
                }));
            }
        }

        private Action<bool, string> _folderDialogCallback;
        /// <summary>
        /// ダイアログに対するコールバックを取得します。
        /// </summary>
        public Action<bool, string> FolderDialogCallback
        {
            get { return this._folderDialogCallback; }
            private set { SetProperty(ref this._folderDialogCallback, value); }
        }

        private void OnFolderDialogCallback(bool isOk, string filePath)
        {
            this.FolderDialogCallback = null;
            if (isOk)
            {
                this.ImagePath = filePath;
            }
        }
        #endregion フォルダを開く

        #region ウィンドウ読み込み
        private DelegateCommand _lodingCommand;
        /// <summary>
        /// ウィンドウ読み込みコマンドを取得します。
        /// </summary>
        public DelegateCommand LodingCommand
        {
            get
            {
                return this._lodingCommand ?? (this._lodingCommand = new DelegateCommand(_ => { OnLoding(); }));
            }
        }

        /// <summary>
        /// ウィンドウ読み込みを行います。
        /// </summary>
        private bool OnLoding()
        {
            if (Properties.Settings.Default.ImagePathHistory == null)
            {
                this.ImagePathHistory = new StringCollection();
            }
            else
            {
                this.ImagePathHistory = Properties.Settings.Default.ImagePathHistory;
            }
            return true;
        }
        #endregion ウィンドウ読み込み

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
            Properties.Settings.Default.ImagePathHistory = this.ImagePathHistory;
            Properties.Settings.Default.Save();
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
