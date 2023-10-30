using Dapper;
using FaceIDAPI.Models;
using FaceIDAPI.Models.Resp;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace FaceIDAPI.Repository
{
   

    public class DeviceRepository: SqlHelper
    {
        /// <summary>
        /// 連線字串
        /// </summary>
        //private readonly string _connectString = @"Data Source=DESKTOP-USMBRO1\SQLEXPRESS;Initial Catalog=CSS;Integrated Security=false;Connect Timeout=30;User ID=API_User;Password=sQW#4246dfs;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";

        /// <summary>
        /// 查詢卡片列表
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Device> GetList()
        {
            using (var conn = new SqlConnection(ConnectionStrings))
            {
                var result = conn.Query<Device>("SELECT * FROM Device");
                return result;
            }
        }

        /// <summary>
        /// 查詢設備
        /// </summary>
        /// <returns></returns>
        public Device Get(string No)
        {
            using (var conn = new SqlConnection(ConnectionStrings))
            {
                var result = conn.QueryFirstOrDefault<Device>(
                    "SELECT TOP 1 * FROM Device Where No = @No",
                    new
                    {
                        No = No
                    });
                return result;
            }
        }
        public async Task<string> getDeviceKey(Device device)
        {
            var httpClient = new HttpClient();
            string url = "http://" + device.IP + ":" + device.Port + "/getDeviceKey";
            var response = await httpClient.GetAsync(url);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }
        public async Task<string> CallDoor(Device device)
        {
            var httpClient = new HttpClient();
            var content = new MultipartFormDataContent
            {
                { new StringContent(device.Pass), "pass" }
            };
            string url = "http://" + device.IP + ":" + device.Port + "/device/openDoorControl";
            var response = await httpClient.PostAsync(url, content);


            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="No">工號</param>
        /// <param name="PicNo">圖序</param>
        /// <param name="strbase64">base64</param>
        /// <param name="device">設備</param>
        /// <returns></returns>
        public void PicUp(string No, int PicNo, string strbase64, Device device)
        {

            //var httpClient = new HttpClient();
            //var content = new MultipartFormDataContent();
            //content.Add(new StringContent(device.Pass), "pass");
            //content.Add(new StringContent(No), "personId");
            //content.Add(new StringContent(No + PicNo.ToString()), "faceId");
            //content.Add(new StringContent(strbase64), "imgBase64");
            //string url = "http://" + device.IP + ":" + device.Port + "/face/update";
            //httpClient.PostAsync(url, content);

            var httpClient = new HttpClient();
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(device.Pass), "pass");
            content.Add(new StringContent(No + PicNo.ToString()), "faceId");
            string url = "http://" + device.IP + ":" + device.Port + "/face/delete";
            httpClient.PostAsync(url, content);

            var httpClient2 = new HttpClient();
            var content2 = new MultipartFormDataContent();
            content2.Add(new StringContent(device.Pass), "pass");
            content2.Add(new StringContent(No), "personId");
            content2.Add(new StringContent(No + PicNo.ToString()), "faceId");
            content2.Add(new StringContent(strbase64), "imgBase64");
            url = "http://" + device.IP + ":" + device.Port + "/face/create";
            httpClient2.PostAsync(url, content2);
        }
        /// <summary>
        /// 人員新增
        /// </summary>
        /// <param name="one"></param>
        /// <param name="device"></param>
        /// <returns></returns>
        public async Task<string> MemberCreate(MemberRelResp one, Device device)
        {
            var httpClient = new HttpClient();
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(device.Pass), "pass");
            person P = new person();
            P.id = one.No;
            P.name = one.Name;
            string json = JsonSerializer.Serialize(P); ;
            content.Add(new StringContent(json.ToString()), "person");
            string url = "http://" + device.IP + ":" + device.Port + "/person/create";
            var response = await httpClient.PostAsync(url, content);
            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }
        


        /// <summary>
        /// 人員修改
        /// </summary>
        /// <param name="one"></param>
        /// <param name="device"></param>
        /// <returns></returns>
        public void MemberUpdate(MemberRelResp one, Device device)
        {
            var httpClient = new HttpClient();
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(device.Pass), "pass");
            person P = new person();
            P.id = one.No;
            P.name = one.Name;
            //P.idcardNum = one.CardNo;
            string json = JsonSerializer.Serialize(P); ;
            content.Add(new StringContent(json.ToString()), "person");
            string url = "http://" + device.IP + ":" + device.Port + "/person/update";
            //var response = 
                httpClient.PostAsync(url, content);
            //response.EnsureSuccessStatusCode();
            //string responseBody = await response.Content.ReadAsStringAsync();
            //return responseBody;
        }
        //public async Task<string> MemberDelete(MemberRelResp one, Device device)
        public void MemberDelete(MemberRelResp one, Device device)
        {
            var httpClient = new HttpClient();
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(device.Pass), "pass");
            content.Add(new StringContent(one.No), "id");
            content.Add(new StringContent("0"), "deleteLog");
            string url = "http://" + device.IP + ":" + device.Port + "/person/delete";
            //var response = await 
                httpClient.PostAsync(url, content);
            //response.EnsureSuccessStatusCode();
            //string responseBody = await response.Content.ReadAsStringAsync();
            //return responseBody;
        }
        
            /// <summary>
            /// 
            /// </summary>
            /// <param name="start">起日</param>
            /// <param name="end">迄日</param>
            /// <param name="member">工號</param>
            /// <param name="index">頁碼</param>
            /// <param name="length">每頁最大數量</param>
            /// <param name="device">設備</param>
            /// <returns></returns>
            public async Task<string> GetRecord(DateTime start, DateTime end,Member member,int index,int length, Device device)
        {
            var httpClient = new HttpClient();
            var content = new MultipartFormDataContent();
            content.Add(new StringContent(device.Pass), "pass");
            content.Add(new StringContent(member.No), "personId");
            content.Add(new StringContent(length.ToString()), "length");
            content.Add(new StringContent(index.ToString()), "index");
            content.Add(new StringContent(start.ToString("yyyy-mm-dd HH;mm:ss")), "startTime");
            content.Add(new StringContent(end.ToString("yyyy-mm-dd HH;mm:ss")), "endTime");
            

            string url = "http://" + device.IP + ":" + device.Port + "/findRecords";
            var response = await httpClient.PostAsync(url, content);


            response.EnsureSuccessStatusCode();
            string responseBody = await response.Content.ReadAsStringAsync();
            return responseBody;
        }
        
    }
}
