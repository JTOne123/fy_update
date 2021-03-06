﻿using System.Collections.Generic;
using Microsoft;
using Update.Core.Entities;
using Update.Core.Events;
using Update.Net;

namespace Update.Core
{
    /// <summary>
    /// 更新器 by 许崇雷
    /// </summary>
    public partial class Updater : Disposable, IUpdater
    {
        private static readonly char[] PERIOD = { '.', '。' };              //句号
        private static readonly string PACKAGES = "packages.txt";           //服务端更新包列表文件
        private static readonly string PACKAGE_DELETE = "delete.txt";       //更新包要删除列表文件
        private UpdateClient m_Client;                                      //客户端
        private IEnumerator<IPackage> m_Avaliables;                         //更新枚举器

        /// <summary>
        /// 构造函数
        /// </summary>
        public Updater()
        {
            this.m_Client = new UpdateClient { Encoding = System.Text.Encoding.UTF8 };
            this.m_Client.KillProgressChanged += (sender, e) => this.OnProgress(new ProgressEventArgs(e.ProgressPercentage));
            this.m_Client.KillProcessCompleted += (sender, e) => this.ClientKillCompleted(e);
            this.m_Client.DownloadProgressChanged += (sender, e) => this.OnProgress(new ProgressEventArgs(e.ProgressPercentage));
            this.m_Client.DownloadStringCompleted += (sender, e) => this.ClientCheckCompleted(e);
            this.m_Client.DownloadDataCompleted += (sender, e) => this.ClientDownloadCompleted(e);
            this.m_Client.DecompressProgressChanged += (sender, e) => this.OnProgress(new ProgressEventArgs(e.ProgressPercentage));
            this.m_Client.DecompressDataCompleted += (sender, e) => this.ClientDecompressCompleted(e);
        }
    }
}
