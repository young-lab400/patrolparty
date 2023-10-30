using System;

namespace FaceIDAPI.Models
{
    public class patrolerrorrecord
    {
        public int unid { get; set; }
        /// <summary>
        /// 駐點代號
        /// </summary>
        public string unitId { get; set; }
        /// <summary>
        /// 巡邏點代號
        /// </summary>
        public string pointId { get; set; }
        /// <summary>
        /// 巡邏點名稱
        /// </summary>
        public string pointName { get; set; }
        /// <summary>
        /// 應巡邏起始時間
        /// </summary>
        public DateTime startTime { get; set; }
        /// <summary>
        /// 應巡邏結束時間
        /// </summary>
        public DateTime endTime { get; set; }
        /// <summary>
        /// 紀錄建立時間
        /// </summary>
        public DateTime createdTime { get; set; }
        /// <summary>
        /// 最後修改時間
        /// </summary>
        public DateTime lastUpdate { get; set; }
    }
}
