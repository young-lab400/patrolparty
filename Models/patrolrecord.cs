using System;

namespace FaceIDAPI.Models
{
    public class patrolrecord
    {
        /// <summary>
        /// 
        /// </summary>
        public int unid { get; set; }
        /// <summary>
        /// 駐點代號
        /// </summary>
        public string unitId { get; set; }
        /// <summary>
        /// 巡邏點代號
        /// </summary>
        public string patrolPointId { get; set; }
        /// <summary>
        /// 巡邏點名稱
        /// </summary>
        public string patrolPointName { get; set; }
        /// <summary>
        /// 巡邏時間
        /// </summary>
        public DateTime patrolTime { get; set; }
        /// <summary>
        /// 巡邏狀態
        /// </summary>
        public int patrolStatus { get; set; }
        /// <summary>
        /// 巡邏回報備註
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// 巡邏人員工號
        /// </summary>
        public string personId { get; set; }
        /// <summary>
        /// 背景上傳
        /// </summary>
        public int offline { get; set; }
        /// <summary>
        /// 最後更新時間
        /// </summary>
        public DateTime lastUpdate { get; set; }

    }
}
