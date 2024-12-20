﻿using MiniExcelLibs.Attributes;
using System.Globalization;

namespace MiniExcelLibs
{
    public interface IConfiguration { }
    public abstract class Configuration : IConfiguration
    {
        public CultureInfo Culture { get; set; } = CultureInfo.InvariantCulture;
        public DynamicExcelColumn[] DynamicColumns { get; set; }
        public int BufferSize { get; set; } = 1024 * 512;
        public bool FastMode { get; set; } = false;
        
        /// <summary>
        ///     When exporting using DataReader, the data not in DynamicColumn will be filtered.
        /// </summary>
        public bool DynamicColumnFirst { get; set; } = false;
    }
}
