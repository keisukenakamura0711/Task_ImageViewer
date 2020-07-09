using Microsoft.Win32;
using System;
using System.Windows;

namespace WPFApp_Test3.Views.Behaviors
{
    /// <summary>
    /// コモンダイアログに関するビヘイビアを表します。
    /// </summary>

    internal class CommonDialogBehavior
    {
        #region Callback 添付プロパティ
        /// <summary>
        /// Action&lt;bool, string&gt; 型のCallback添付プロパティを定義します。
        /// </summary>
        public static readonly DependencyProperty CallbackProperty = DependencyProperty.RegisterAttached("Callback",
                                                                                                         typeof(Action<bool, string>),
                                                                                                         typeof(CommonDialogBehavior),
                                                                                                         new PropertyMetadata(null, OnCallbackPropertyChanged));

        /// <summary>
        /// Callback添付プロパティを取得します。
        /// </summary>
        /// <Param name="target">対象とするDependencyObjectを指定します</param>
        /// <returns>取得した値を返します</returns>
        public static Action<bool, string> GetCallback(DependencyObject target)
        {
            return (Action<bool, string>)target.GetValue(CallbackProperty);
        }

        /// <summary>
        /// Callback添付プロパティを設定します。
        /// </summary>
        /// <param name="target">対象とする DependencyObject を指定します。</param>
        /// <param name="value">設定する値を指定します。</param>
        public static void SetCallback(DependencyObject target, Action<bool, string> value)
        {
            target.SetValue(CallbackProperty, value);
        }

        /// <summary>
        /// string 型の Title 添付プロパティを定義します。
        /// </summary>
        public static readonly DependencyProperty TitleProperty = DependencyProperty.RegisterAttached("Title",
                                                                                                      typeof(string),
                                                                                                      typeof(CommonDialogBehavior),
                                                                                                      new PropertyMetadata("ファイルを開く"));

        /// <summary>
        /// Title添付プロパティを取得します。
        /// </summary>
        /// <Param name="target">対象とするDependencyObjectを指定します</param>
        /// <returns>取得した値を返します</returns>
        public static string GetTitle(DependencyObject target)
        {
            return (string)target.GetValue(TitleProperty);
        }

        /// <summary>
        /// Title添付プロパティを設定します。
        /// </summary>
        /// <param name="target">対象とする DependencyObject を指定します。</param>
        /// <param name="value">設定する値を指定します。</param>
        public static void SetTitle(DependencyObject target, string value)
        {
            target.SetValue(TitleProperty, value);
        }

        /// <summary>
        /// string 型の Filter 添付プロパティを定義します。
        /// </summary>
        public static readonly DependencyProperty FilterProperty = DependencyProperty.RegisterAttached("Filter",
                                                                                                       typeof(string),
                                                                                                       typeof(CommonDialogBehavior),
                                                                                                       new PropertyMetadata("すべてのファイル(*.*) | *.* "));

        /// <summary>
        /// Filter添付プロパティを取得します。
        /// </summary>
        /// <Param name="target">対象とするDependencyObjectを指定します</param>
        /// <returns>取得した値を返します</returns>
        public static string GetFilter(DependencyObject target)
        {
            return (string)target.GetValue(FilterProperty);
        }

        /// <summary>
        /// Filter添付プロパティを設定します。
        /// </summary>
        /// <param name="target">対象とする DependencyObject を指定します。</param>
        /// <param name="value">設定する値を指定します。</param>
        public static void SetFilter(DependencyObject target, string value)
        {
            target.SetValue(FilterProperty, value);
        }

        /// <summary>
        /// string 型の Multiselect 添付プロパティを定義します。
        /// </summary>
        public static readonly DependencyProperty MultiselectProperty = DependencyProperty.RegisterAttached("Multiselect",
                                                                                                            typeof(bool),
                                                                                                            typeof(CommonDialogBehavior),
                                                                                                            new PropertyMetadata(false));

        /// <summary>
        /// Multiselect添付プロパティを取得します。
        /// </summary>
        /// <Param name="target">対象とするDependencyObjectを指定します</param>
        /// <returns>取得した値を返します</returns>
        public static bool GetMultiselect(DependencyObject target)
        {
            return (bool)target.GetValue(MultiselectProperty);
        }

        /// <summary>
        /// Filter添付プロパティを設定します。
        /// </summary>
        /// <param name="target">対象とする DependencyObject を指定します。</param>
        /// <param name="value">設定する値を指定します。</param>
        public static void SetMultiselect(DependencyObject target, bool value)
        {
            target.SetValue(MultiselectProperty, value);
        }

        /// <summary>
        /// Callback添付プロパティ変更イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行元</param>
        /// <param name="e">イベント引数</param>
        private static void OnCallbackPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var callback = GetCallback(sender);
            if(callback != null)
            {
                var dlg = new OpenFileDialog()
                {
                    Title = GetTitle(sender),
                    Filter = GetFilter(sender),
                    Multiselect = GetMultiselect(sender),
                };

                var owner = Window.GetWindow(sender);
                var result = dlg.ShowDialog(owner);
                callback(result.Value, dlg.FileName);
            }
        }
        #endregion Callback 添付プロパティ
    }
}
