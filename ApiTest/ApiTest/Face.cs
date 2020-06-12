using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;

namespace ApiTest
{
	public static class Face
	{
		// 调用getAccessToken()获取的 access_token建议根据expires_in 时间 设置缓存
		// 返回token示例
		public static string TOKEN = "***************";

		// 百度云中开通对应服务应用的 API Key 建议开通应用的时候多选服务
		private static string clientId = "************";
		// 百度云中开通对应服务应用的 Secret Key
		private static string clientSecret = "********";

		/// <summary>
		/// 获取Token值
		/// </summary>
		/// <returns></returns>
		public static FaceTocken getAccessToken()
		{
			FaceTocken tocken = new FaceTocken();
			String authHost = "https://aip.baidubce.com/oauth/2.0/token";
			HttpClient client = new HttpClient();
			List<KeyValuePair<string, string>> paraList = new List<KeyValuePair<string, string>>();
			paraList.Add(new KeyValuePair<string, string>("grant_type", "client_credentials"));
			paraList.Add(new KeyValuePair<string, string>("client_id", clientId));
			paraList.Add(new KeyValuePair<string, string>("client_secret", clientSecret));

			HttpResponseMessage response = client.PostAsync(authHost, new FormUrlEncodedContent(paraList)).Result;
			string result = response.Content.ReadAsStringAsync().Result;
			//Console.WriteLine(result);
			tocken = Newtonsoft.Json.JsonConvert.DeserializeObject<FaceTocken>(result);
			return tocken;
		}
		/// <summary>
		/// 将图片转为BASE64值
		/// </summary>
		/// <param name="Imagefilename">图片路径</param>
		/// <returns></returns>
		public static string ImgToBase64String(string Imagefilename)
		{
			try
			{
				Bitmap bmp = new Bitmap(Imagefilename);
				MemoryStream ms = new MemoryStream();
				bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Jpeg);
				byte[] arr = new byte[ms.Length];
				ms.Position = 0;
				ms.Read(arr, 0, (int)ms.Length);
				ms.Close();
				return Convert.ToBase64String(arr);
			}
			catch (Exception ex)
			{
				return null;
			}
		}
		/// <summary>
		/// 人脸对比
		/// </summary>
		/// <returns>返回对比值</returns>
		public static Root faceMatch()
		{
			var sdc = getAccessToken();
			string token = sdc.access_token;
			string host = "https://aip.baidubce.com/rest/2.0/face/v3/match?access_token=" + token;
			Encoding encoding = Encoding.Default;
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(host);
			request.Method = "post";
			request.KeepAlive = true;
			string str = string.Empty;
			List<FaceImage> images = new List<FaceImage>();
			FaceImage s = new FaceImage
			{
				image = ImgToBase64String(@"D:\Images\66.jpg"),
				image_type = "BASE64",
				face_type = "LIVE",
				quality_control = "LOW"
			};
			FaceImage s1 = new FaceImage
			{
				image = ImgToBase64String(@"D:\Images\44.jpg"),
				image_type = "BASE64",
				face_type = "LIVE",
				quality_control = "LOW"
			};
			images.Add(s);
			images.Add(s1);
			str = Newtonsoft.Json.JsonConvert.SerializeObject(images);
			byte[] buffer = encoding.GetBytes(str);
			request.ContentLength = buffer.Length;
			request.GetRequestStream().Write(buffer, 0, buffer.Length);
			HttpWebResponse response = (HttpWebResponse)request.GetResponse();
			StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default);
			string result = reader.ReadToEnd();
			//Console.WriteLine("人脸对比:");
			//Console.WriteLine(result);
			var FaceResult = Newtonsoft.Json.JsonConvert.DeserializeObject<Root>(result);
			return FaceResult;
		}
		/// <summary>
		/// 人脸检测与属性分析
		/// </summary>
		/// <returns>返回人脸数据信息</returns>
		public static string faceDetect()
		{
			var sdc = getAccessToken();
			string token = sdc.access_token;
			string host = "https://aip.baidubce.com/rest/2.0/face/v3/detect?access_token=" + token;
			Encoding encoding = Encoding.Default;
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(host);
			request.Method = "post";
			request.KeepAlive = true;
			FaceImp s1 = new FaceImp
			{
				image = ImgToBase64String(@"D:\Images\88.jpg"),
				image_type = "BASE64",
				face_field = ""
			};
			//String str = "{\"image\":\"027d8308a2ec665acb1bdf63e513bcb9\",\"image_type\":\"FACE_TOKEN\",\"face_field\":\"faceshape,facetype\"}";
			string str = "";
			str = Newtonsoft.Json.JsonConvert.SerializeObject(s1);
			byte[] buffer = encoding.GetBytes(str);
			request.ContentLength = buffer.Length;
			request.GetRequestStream().Write(buffer, 0, buffer.Length);
			HttpWebResponse response = (HttpWebResponse)request.GetResponse();
			StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default);
			string result = reader.ReadToEnd();
			Console.WriteLine("人脸检测与属性分析:");
			Console.WriteLine(result);
			return result;
		}
		/// <summary>
		/// 身份信息验证(需要获取权限)
		/// </summary>
		/// <returns></returns>
		public static string IDCard_Name()
		{
			var sdc = getAccessToken();
			string token = sdc.access_token;
			string host = "https://aip.baidubce.com/rest/2.0/face/v3/person/idmatch?access_token=" + token;
			Encoding encoding = Encoding.Default;
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(host);
			request.Method = "post";
			request.KeepAlive = true;
			string str = "{\"id_card_number\":\"yourIDCard\",\"name\":\"yourName\"}";
			byte[] buffer = encoding.GetBytes(str);
			request.ContentLength = buffer.Length;
			request.GetRequestStream().Write(buffer, 0, buffer.Length);
			HttpWebResponse response = (HttpWebResponse)request.GetResponse();
			StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default);
			string result = reader.ReadToEnd();
			Console.WriteLine(result);
			return result;
		}

		/// <summary>
		/// 身份验证(需要获取权限)
		/// </summary>
		/// <returns></returns>
		public static string personVerify()
		{
			var sdc = getAccessToken();
			string token = sdc.access_token;
			string host = "https://aip.baidubce.com/rest/2.0/face/v3/person/verify?access_token=" + token;
			Encoding encoding = Encoding.Default;
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(host);
			request.Method = "post";
			request.KeepAlive = true;
			FaceIDCard faceID = new FaceIDCard
			{
				image = ImgToBase64String(@"D:\Images\55.jpg"),//自己人脸图片BASE64值
				image_type = "BASE64",
				id_card_number = "******",//IDCard
				name = "******",//Name
				quality_control = "LOW",
				liveness_control = "HIGH"
			};
			//string str = "{\"image\":\"sfasq35sadvsvqwr5q...\",\"image_type\":\"BASE64\",\"id_card_number\":\"110...\",\"name\":\"张三\",\"quality_control\":\"LOW\",\"liveness_control\":\"HIGH\"}";
			string str = Newtonsoft.Json.JsonConvert.SerializeObject(faceID);
			byte[] buffer = encoding.GetBytes(str);
			request.ContentLength = buffer.Length;
			request.GetRequestStream().Write(buffer, 0, buffer.Length);
			HttpWebResponse response = (HttpWebResponse)request.GetResponse();
			StreamReader reader = new StreamReader(response.GetResponseStream(), Encoding.Default);
			string result = reader.ReadToEnd();
			Console.WriteLine("身份验证:");
			Console.WriteLine(result);
			return result;
		}

	}
}
