﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Aquirrel.Logger.File.Internal
{
    public class LoggerOptionsModel
    {
        /// <summary>
        /// 最小记录级别
        /// </summary>
        public LogLevel MinLevel { get; set; }
        /// <summary>
        /// 日志文件路径
        /// </summary>
        public string FileDiretoryPath { get; set; }
        /// <summary>
        /// 日志文件名称
        /// </summary>
        public string FileNameTemplate { get; set; }
        /// <summary>
        /// 单文件最大大小。单位M
        /// </summary>

        public int MaxSize_Bytes { get; set; }
        /// <summary>
        /// 命名空间类别
        /// </summary>

        public string CategoryName { get; set; }
        /// <summary>
        /// 是否启用scopes
        /// </summary>
        public bool IncludeScopes { get; set; }

        public override int GetHashCode()
        {
            return this.MinLevel.GetHashCode() + this.FileDiretoryPath.GetHashCode() + this.FileNameTemplate.GetHashCode();
        }
        public override bool Equals(object obj)
        {
            var b = obj as LoggerOptionsModel;
            if (b == null)
                return false;
            return this.MinLevel == b.MinLevel && this.FileDiretoryPath == b.FileDiretoryPath && this.FileNameTemplate == b.FileNameTemplate;
        }

    }
}
