using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FaceIDAPI.Models
{
    public class Device
    {
        /// <summary>
        /// Pkey
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 序號
        /// </summary>
        public string No { get; set; }
        
        /// <summary>
        /// 設備名稱
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// 設備IP
        /// </summary>
        public string IP { get; set; }
        /// <summary>
        /// 設備IP Port
        /// </summary>
        public string Port { get; set; }

        /// <summary>
        /// 設備密碼
        /// </summary>
        public string Pass { get; set; }
        /// <summary>
        /// 人臉識別距離
        /// </summary>
        public int identifyDistance { get; set; }
        /// <summary>
        /// 識別間隔，單位秒
        /// </summary>
        public int saveIdentifyTime { get; set; }
        /// <summary>
        /// 0：識別成功後開繼電器（識別成功包括所有識別方式：人臉、卡、卡+人臉，人證比對等）（默認0）
        ///1：不開繼電器
        /// </summary>
        public int openout { get; set; }
        /// <summary>
        /// 設備進出方向：0為出， 1為進， 2無進出狀態
        /// </summary>
        public int inOutType { get; set; }
        

    }
}
