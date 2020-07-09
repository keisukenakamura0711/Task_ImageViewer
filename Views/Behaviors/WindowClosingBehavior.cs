using System;
using System.ComponentModel;
using System.Windows;

namespace WPFApp_Test3.Views.Behaviors
{
    /// <summary>
    /// Windowを閉じるときのビヘイビアを表します。
    /// </summary>
    internal class WindowClosingBehavior
    {
        #region Callback 添付プロパティ
        /// <summary>
        /// Func&lt;bool&gt; 型の Callback 添付プロパティを定義します。
        /// </summary>
        public static readonly DependencyProperty CallbackProperty = DependencyProperty.RegisterAttached("Callback",
                                                                                                         typeof(Func<bool>),
                                                                                                         typeof(WindowClosingBehavior),
                                                                                                         new PropertyMetadata(null, OnIsEnabledPropertyChagned));

        /// <summary>
        /// Callback添付プロパティを取得します。
        /// </summary>
        /// <Param name="target">対象とするDependencyObjectを指定します</param>
        /// <returns>取得した値を返します</returns>
        public static Func<bool> GetCallback(DependencyObject target)
        {
            return (Func<bool>)target.GetValue(CallbackProperty);
        }

        /// <summary>
        /// Callback添付プロパティを設定します。
        /// </summary>
        /// <param name="target">対象とする DependencyObject を指定します。</param>
        /// <param name="value">設定する値を指定します。</param>
        public static void SetCallback(DependencyObject target, Func<bool> value)
        {
            target.SetValue(CallbackProperty, value);
        }
        #endregion Callback 添付プロパティ

        /// <summary>
        /// Callback 添付プロパティ変更イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行元</param>
        /// <param name="e">イベント引数</param>
        private static void OnIsEnabledPropertyChagned(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var w = sender as Window;
            if (w != null)
            {
                var callback = GetCallback(w);
                if ((callback != null) && (e.OldValue == null))
                {
                    w.Closing += OnClosing;
                }
                else if (callback == null)
                {
                    w.Closing -= OnClosing;
                }
            }
        }

        /// <summary>
        /// Closing イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行元</param>
        /// <param name="e">イベント引数</param>
        private static void OnClosing(object sender, CancelEventArgs e)
        {
            var callback = GetCallback(sender as DependencyObject);
            if (callback != null)
            {
                // コールバック処理の結果がfalseのときキャンセルする
                e.Cancel = !callback();
            }
        }
    }
}
