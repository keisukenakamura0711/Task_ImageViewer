using Microsoft.Win32;
using System;
using System.Windows;
using Microsoft.WindowsAPICodePack.Dialogs;

namespace Task_ImageViewer.Views.Behaviors
{
    /// <summary>
    /// コモンダイアログに関するビヘイビアを表します。
    /// </summary>

    internal class CommonDialogBehavior
    {
        #region ファイル選択ダイアログ
        /// <summary>
        /// Action&lt;bool, string&gt; 型の Callback 添付プロパティを定義します。
        /// </summary>
        public static readonly DependencyProperty FileCallbackProperty = DependencyProperty.RegisterAttached("FileCallback",
                                                                                                             typeof(Action<bool, string>),
                                                                                                             typeof(CommonDialogBehavior),
                                                                                                             new PropertyMetadata(null, OnFileCallbackPropertyChanged));

        /// <summary>
        /// FileCallback 添付プロパティを取得します。
        /// </summary>
        /// <Param name="target">対象とするDependencyObjectを指定します</param>
        /// <returns>取得した値を返します</returns>
        public static Action<bool, string> GetFileCallback(DependencyObject target)
        {
            return (Action<bool, string>)target.GetValue(FileCallbackProperty);
        }

        /// <summary>
        /// FileCallback 添付プロパティを設定します。
        /// </summary>
        /// <param name="target">対象とする DependencyObject を指定します。</param>
        /// <param name="value">設定する値を指定します。</param>
        public static void SetFileCallback(DependencyObject target, Action<bool, string> value)
        {
            target.SetValue(FileCallbackProperty, value);
        }

        /// <summary>
        /// string 型の FileTitle 添付プロパティを定義します。
        /// </summary>
        public static readonly DependencyProperty FileTitleProperty = DependencyProperty.RegisterAttached("FileTitle",
                                                                                                          typeof(string),
                                                                                                          typeof(CommonDialogBehavior),
                                                                                                          new PropertyMetadata("ファイルを開く"));

        /// <summary>
        /// FileTitle 添付プロパティを取得します。
        /// </summary>
        /// <Param name="target">対象とするDependencyObjectを指定します</param>
        /// <returns>取得した値を返します</returns>
        public static string GetFileTitle(DependencyObject target)
        {
            return (string)target.GetValue(FileTitleProperty);
        }

        /// <summary>
        /// FileTitle 添付プロパティを設定します。
        /// </summary>
        /// <param name="target">対象とする DependencyObject を指定します。</param>
        /// <param name="value">設定する値を指定します。</param>
        public static void SetFileTitle(DependencyObject target, string value)
        {
            target.SetValue(FileTitleProperty, value);
        }

        /// <summary>
        /// string 型の FileFilter 添付プロパティを定義します。
        /// </summary>
        public static readonly DependencyProperty FileFilterProperty = DependencyProperty.RegisterAttached("FileFilter",
                                                                                                           typeof(string),
                                                                                                           typeof(CommonDialogBehavior),
                                                                                                           new PropertyMetadata("すべてのファイル(*.*)|*.*"));

        /// <summary>
        /// FileFilter 添付プロパティを取得します。
        /// </summary>
        /// <Param name="target">対象とするDependencyObjectを指定します</param>
        /// <returns>取得した値を返します</returns>
        public static string GetFileFilter(DependencyObject target)
        {
            return (string)target.GetValue(FileFilterProperty);
        }

        /// <summary>
        /// FileFilter 添付プロパティを設定します。
        /// </summary>
        /// <param name="target">対象とする DependencyObject を指定します。</param>
        /// <param name="value">設定する値を指定します。</param>
        public static void SetFileFilter(DependencyObject target, string value)
        {
            target.SetValue(FileFilterProperty, value);
        }

        /// <summary>
        /// string 型の FileMultiselect 添付プロパティを定義します。
        /// </summary>
        public static readonly DependencyProperty FileMultiselectProperty = DependencyProperty.RegisterAttached("FileMultiselect",
                                                                                                                typeof(bool),
                                                                                                                typeof(CommonDialogBehavior),
                                                                                                                new PropertyMetadata(false));

        /// <summary>
        /// FileMultiselect 添付プロパティを取得します。
        /// </summary>
        /// <Param name="target">対象とするDependencyObjectを指定します</param>
        /// <returns>取得した値を返します</returns>
        public static bool GetFileMultiselect(DependencyObject target)
        {
            return (bool)target.GetValue(FileMultiselectProperty);
        }

        /// <summary>
        ///File Multiselect 添付プロパティを設定します。
        /// </summary>
        /// <param name="target">対象とする DependencyObject を指定します。</param>
        /// <param name="value">設定する値を指定します。</param>
        public static void SetFileMultiselect(DependencyObject target, bool value)
        {
            target.SetValue(FileMultiselectProperty, value);
        }

        private static void OnFileCallbackPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var callback = GetFileCallback(sender);
            if(callback != null)
            {
                var dlg = new CommonOpenFileDialog()
                {
                    Title = GetFileTitle(sender),
                    Multiselect = GetFileMultiselect(sender),
                    IsFolderPicker = false,
                };

                string rawDisplayName = "";
                foreach (string str in GetFileFilter(sender).Split('|'))
                {
                    if (rawDisplayName == "")
                    {
                        rawDisplayName = str;
                    }
                    else
                    {
                        dlg.Filters.Add (new CommonFileDialogFilter(rawDisplayName, str));
                        rawDisplayName = "";
                    }
                }

                var result = dlg.ShowDialog();
                if (result == CommonFileDialogResult.Ok)
                {
                    callback(true, dlg.FileName);
                }
                else
                {
                    callback(false, "");
                }
            }
        }
        #endregion ファイル選択ダイアログ

        #region フォルダ選択ダイアログ
        /// <summary>
        /// Action&lt;bool, string&gt; 型の FolderCallback 添付プロパティを定義します。
        /// </summary>
        public static readonly DependencyProperty FolderCallbackProperty = DependencyProperty.RegisterAttached("FolderCallback",
                                                                                                               typeof(Action<bool, string>),
                                                                                                               typeof(CommonDialogBehavior),
                                                                                                               new PropertyMetadata(null, OnFolderCallbackPropertyChanged));

        /// <summary>
        /// FolderCallback 添付プロパティを取得します。
        /// </summary>
        /// <Param name="target">対象とするDependencyObjectを指定します</param>
        /// <returns>取得した値を返します</returns>
        public static Action<bool, string> GetFolderCallback(DependencyObject target)
        {
            return (Action<bool, string>)target.GetValue(FolderCallbackProperty);
        }

        /// <summary>
        /// FolderCallback 添付プロパティを設定します。
        /// </summary>
        /// <param name="target">対象とする DependencyObject を指定します。</param>
        /// <param name="value">設定する値を指定します。</param>
        public static void SetFolderCallback(DependencyObject target, Action<bool, string> value)
        {
            target.SetValue(FolderCallbackProperty, value);
        }

        /// <summary>
        /// string 型の FolderTitle 添付プロパティを定義します。
        /// </summary>
        public static readonly DependencyProperty FolderTitleProperty = DependencyProperty.RegisterAttached("FolderTitle",
                                                                                                            typeof(string),
                                                                                                            typeof(CommonDialogBehavior),
                                                                                                            new PropertyMetadata("ファイルを開く"));

        /// <summary>
        /// FolderTitle 添付プロパティを取得します。
        /// </summary>
        /// <Param name="target">対象とするDependencyObjectを指定します</param>
        /// <returns>取得した値を返します</returns>
        public static string GetFolderTitle(DependencyObject target)
        {
            return (string)target.GetValue(FolderTitleProperty);
        }

        /// <summary>
        /// FolderTitle 添付プロパティを設定します。
        /// </summary>
        /// <param name="target">対象とする DependencyObject を指定します。</param>
        /// <param name="value">設定する値を指定します。</param>
        public static void SetFolderTitle(DependencyObject target, string value)
        {
            target.SetValue(FolderTitleProperty, value);
        }

        /// <summary>
        /// string 型の FolderMultiselect 添付プロパティを定義します。
        /// </summary>
        public static readonly DependencyProperty FolderMultiselectProperty = DependencyProperty.RegisterAttached("FolderMultiselect",
                                                                                                                  typeof(bool),
                                                                                                                  typeof(CommonDialogBehavior),
                                                                                                                  new PropertyMetadata(false));

        /// <summary>
        /// FolderMultiselect 添付プロパティを取得します。
        /// </summary>
        /// <Param name="target">対象とするDependencyObjectを指定します</param>
        /// <returns>取得した値を返します</returns>
        public static bool GetFolderMultiselect(DependencyObject target)
        {
            return (bool)target.GetValue(FolderMultiselectProperty);
        }

        /// <summary>
        /// FolderMultiselect 添付プロパティを設定します。
        /// </summary>
        /// <param name="target">対象とする DependencyObject を指定します。</param>
        /// <param name="value">設定する値を指定します。</param>
        public static void SetFolderMultiselect(DependencyObject target, bool value)
        {
            target.SetValue(FolderMultiselectProperty, value);
        }

        /// <summary>
        /// FolderCallback 添付プロパティ変更イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行元</param>
        /// <param name="e">イベント引数</param>
        private static void OnFolderCallbackPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var callback = GetFolderCallback(sender);
            if (callback != null)
            {
                var dlg = new CommonOpenFileDialog()
                {
                    Title = GetFolderTitle(sender),
                    Multiselect = GetFolderMultiselect(sender),
                    IsFolderPicker = true,
                };

                var result = dlg.ShowDialog();
                if (result == CommonFileDialogResult.Ok)
                {
                    callback(true, dlg.FileName);
                }
                else
                {
                    callback(false, "");
                }
            }
        }
        #endregion ファイル選択ダイアログ
    }
}
