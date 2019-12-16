#pragma warning disable CS1591,CS1572
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceCG.HPSocket
{
    public class ErrorInfo
    {
        /// <summary>
        /// 错误码
        /// </summary>
        public int ErrorCode { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string ErrorMsg { get; set; }
    }
}
