using System;
using System.Windows;

namespace Task_ImageViewer.Views.Behaviors
{
    /// <summary>
    /// ダイアログを開くためのビヘイビアを表します。
    /// </summary>
    internal class OpenDialogBehavior
    {
        #region DataContext 添付プロパティ
        /// <summary>
        /// object 型の DataContext 添付プロパティを定義します。
        /// </summary>
        public static readonly DependencyProperty DataContextProperty = DependencyProperty.RegisterAttached("DataContext",
                                                                                                            typeof(object),
                                                                                                            typeof(OpenDialogBehavior),
                                                                                                            new PropertyMetadata(null));

        /// <summary>
        /// DataContext 添付プロパティを取得します。
        /// </summary>
        /// <Param name="target">対象とするDependencyObjectを指定します</param>
        /// <returns>取得した値を返します</returns>
        public static object GetDataContext(DependencyObject target)
        {
            return (object)target.GetValue(DataContextProperty);
        }

        /// <summary>
        /// DataContext 添付プロパティを設定します。
        /// </summary>
        /// <param name="target">対象とする DependencyObject を指定します。</param>
        /// <param name="value">設定する値を指定します。</param>
        public static void SetDataContext(DependencyObject target, object value)
        {
            target.SetValue(DataContextProperty, value);
        }
        #endregion DataContext 添付プロパティ

        #region WindowType 添付プロパティ
        /// <summary>
        /// Type 型の WindowType 添付プロパティを定義します。
        /// </summary>
        public static readonly DependencyProperty WindowTypeProperty = DependencyProperty.RegisterAttached("WindowType",
                                                                                                           typeof(Type),
                                                                                                           typeof(OpenDialogBehavior),
                                                                                                           new PropertyMetadata(null));

        /// <summary>
        /// WindowType 添付プロパティを取得します。
        /// </summary>
        /// <Param name="target">対象とするDependencyObjectを指定します</param>
        /// <returns>取得した値を返します</returns>
        public static Type GetWindowType(DependencyObject target)
        {
            return (Type)target.GetValue(WindowTypeProperty);
        }

        /// <summary>
        /// WindowType 添付プロパティを設定します。
        /// </summary>
        /// <param name="target">対象とする DependencyObject を指定します。</param>
        /// <param name="value">設定する値を指定します。</param>
        public static void SetWindowType(DependencyObject target, Type value)
        {
            target.SetValue(WindowTypeProperty, value);
        }
        #endregion WindowType 添付プロパティ

        #region Callback 添付プロパティ
        /// <summary>
        /// Func&lt;bool&gt; 型の Callback 添付プロパティを定義します。
        /// </summary>
        public static readonly DependencyProperty CallbackProperty = DependencyProperty.RegisterAttached("Callback",
                                                                                                         typeof(Action<bool>),
                                                                                                         typeof(OpenDialogBehavior),
                                                                                                         new PropertyMetadata(null, OnIsEnabledPropertyChagned));

        /// <summary>
        /// Callback添付プロパティを取得します。
        /// </summary>
        /// <Param name="target">対象とするDependencyObjectを指定します</param>
        /// <returns>取得した値を返します</returns>
        public static Action<bool> GetCallback(DependencyObject target)
        {
            return (Action<bool>)target.GetValue(CallbackProperty);
        }

        /// <summary>
        /// Callback添付プロパティを設定します。
        /// </summary>
        /// <param name="target">対象とする DependencyObject を指定します。</param>
        /// <param name="value">設定する値を指定します。</param>
        public static void SetCallback(DependencyObject target, Action<bool> value)
        {
            target.SetValue(CallbackProperty, value);
        }
        
        /// <summary>
        /// Callback 添付プロパティ変更イベントハンドラ
        /// </summary>
        /// <param name="sender">イベント発行元</param>
        /// <param name="e">イベント引数</param>
        private static void OnIsEnabledPropertyChagned(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var callback = GetCallback(sender);
            if (callback != null)
            {
                var type = GetWindowType(sender);
                var obj = type.InvokeMember(null, System.Reflection.BindingFlags.CreateInstance, null, null, null);
                var child = obj as Window;
                if (child != null)
                {
                    child.DataContext = GetDataContext(sender);
                    var result = child.ShowDialog();
                    callback(result.Value);
                }
            }
        }
        #endregion Callback 添付プロパティ
    }
}
