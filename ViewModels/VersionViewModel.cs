using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task_ImageViewer.Models;

namespace Task_ImageViewer.ViewModels
{
    /// <summary>
    /// VersionView ウィンドウに対するデータコンテキストを表します。
    /// </summary>
    internal class VersionViewModel : NotificationObject
    {
        /// <summary>
        /// アプリケーションの正式名称を取得します。
        /// </summary>
        public string ProductName
        {
            get { return ProductInfo.Product; }
        }

        /// <summary>
        /// アプリケーション名を取得します。
        /// </summary>
        public string Title
        {
            get { return ProductInfo.Title; }
        }

        /// <summary>
        /// バージョン番号を取得します。
        /// </summary>
        public string Version
        {
            get { return "Ver." + ProductInfo.VersionString; }
        }

        /// <summary>
        /// 著作権情報を取得します。
        /// </summary>
        public string Copyright
        {
            get { return ProductInfo.Copyright; }
        }
    }
}
