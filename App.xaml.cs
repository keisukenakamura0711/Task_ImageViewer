using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WPFApp_Test3.ViewModels;
using WPFApp_Test3.Views;

namespace WPFApp_Test3
{
    /// <summary>
    /// App.xaml の相互作用ロジック
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // ウィンドウをインスタンス化します
            var w = new MainView();

            // ウィンドウに対するViewModelをインスタンス化します
            var vm = new MainViewModel();

            // ウィンドウに対するViewModelをデータコンテキストに指定します
            w.DataContext = vm;

            // ウィンドウを表示します
            w.Show();
        }
    }
}
