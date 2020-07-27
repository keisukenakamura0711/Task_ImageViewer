using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using System.Windows;

namespace Task_ImageViewer.Views.Behaviors
{
    /// <summary>
    /// Windowロード時のビヘイビアを表します。
    /// </summary>
    class WindowLoadingBehavior
    {
        #region Callback 添付プロパティ
        /// <summary>
        /// Func&lt;bool&gt; 型の Callback 添付プロパティを定義します。
        /// </summary>
        public static readonly DependencyProperty CallbackProperty = DependencyProperty.RegisterAttached("Callback",
                                                                                                         typeof(DelegateCommand),
                                                                                                         typeof(WindowLoadingBehavior),
                                                                                                         new PropertyMetadata(null, OnIsEnabledPropertyChagned));

        /// <summary>
        /// Callback添付プロパティを取得します。
        /// </summary>
        /// <Param name="target">対象とするDependencyObjectを指定します</param>
        /// <returns>取得した値を返します</returns>
        public static DelegateCommand GetCallback(DependencyObject target)
        {
            return (DelegateCommand)target.GetValue(CallbackProperty);
        }

        /// <summary>
        /// Callback添付プロパティを設定します。
        /// </summary>
        /// <param name="target">対象とする DependencyObject を指定します。</param>
        /// <param name="value">設定する値を指定します。</param>
        public static void SetCallback(DependencyObject target, DelegateCommand value)
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
            var w = sender as FrameworkElement;
            if (w != null && e.NewValue != null)
            {
                if (!w.IsLoaded)
                {
                    w.Loaded += new RoutedEventHandler(OnLoding);
                }
                else
                {
                    OnLoding(w, new RoutedEventArgs
                    {
                        Source = w,
                        Handled = false
                    });
                }
            }
        }

        /// <summary>
        /// Closing イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行元</param>
        /// <param name="e">イベント引数</param>
        private static void OnLoding(object sender, RoutedEventArgs e)
        {
            var w = sender as FrameworkElement;
            w.Loaded -= new RoutedEventHandler(OnLoding);

            var command = GetCallback(sender as DependencyObject);

            //実行可能であれば、コマンドを実行する。
            if (command != null && command.CanExecute(e))
            {
                command.Execute(e);
            }

            return;
        }
    }
}
