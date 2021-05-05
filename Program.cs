using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using WebAPIClient;

namespace ConsoleApp1
{
	class Program
	{
		private static readonly HttpClient client = new HttpClient();

		public static async Task Main(string[] args)
		{
			// 設定ファイル読込
			var query_param_dic = ReadSetting();
			if (query_param_dic == null) return;
			var download_dir = query_param_dic["download_dir"].ToString();
			query_param_dic.Remove("download_dir");
			var authkey = query_param_dic["authkey"].ToString();
			query_param_dic.Remove("authkey");

			// 検索条件作成
			var query_param_txt = await new FormUrlEncodedContent(query_param_dic).ReadAsStringAsync();


			// Headerの設定
			client.DefaultRequestHeaders.Accept.Clear();
			client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/x-www-form-urlencoded+json"));
			client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authkey);

			// Slackからデータ取得
			var message = new HttpRequestMessage(HttpMethod.Post, $"https://slack.com/api/search.files?{query_param_txt}");
			var slack_response1 = await client.SendAsync(message, HttpCompletionOption.ResponseHeadersRead);
			var m = await slack_response1.Content.ReadAsStreamAsync();

			// Jsonからオブジェクトに変換
			var slack = await JsonSerializer.DeserializeAsync<Rootobject>(m);

			// 一致したデータに対する処理
			foreach (var matche in slack.files.matches)
			{
				Console.WriteLine(matche.name);
				Console.WriteLine(matche.url_private_download);
				if(matche.url_private_download==null)
				{
					Console.WriteLine("ダウンロード対象が存在しません。スキップします。");
					continue;
				}
				Console.WriteLine("ダウンロード中...");
				using (var request = new HttpRequestMessage(HttpMethod.Get, new Uri(matche.url_private_download)))
				using (var response = await client.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
				{
					if (response.StatusCode == System.Net.HttpStatusCode.OK)
					{
						var fullpath = System.IO.Path.Combine(download_dir, matche.name);
						using (var content = response.Content)
						using (var stream = await content.ReadAsStreamAsync())
						using (var fileStream = new FileStream(fullpath, FileMode.Create, FileAccess.Write, FileShare.None))
						{
							stream.CopyTo(fileStream);
						}
					}
				}
				Console.WriteLine("ダウンロード完了");
				Console.WriteLine();
			}
		}

		private static Dictionary<string, string> ReadSetting()
		{
			IConfiguration configuration = new ConfigurationBuilder()
			  .SetBasePath(Directory.GetCurrentDirectory())
			  .AddJsonFile("appsettings.json", true, true)
			  .Build();

			IConfigurationSection section = configuration.GetSection("search");

			Console.WriteLine("※appsettings.jsonはUTF-8で保存してください。");
			Console.WriteLine();
			string query = section["query"];
			Console.WriteLine("【検索クエリ】");
			Console.WriteLine(string.IsNullOrWhiteSpace(query)?"指定してください。": query);
			Console.WriteLine();
			
			var sort = section["sort"];
			Console.WriteLine("【検索結果ソートキー】");
			Console.WriteLine(string.IsNullOrWhiteSpace(sort) ? "指定してください。" : sort);
			Console.WriteLine();

			var sort_dir = section["sort_dir"];
			Console.WriteLine("【検索結果並び順】");
			Console.WriteLine(string.IsNullOrWhiteSpace(sort_dir) ? "指定してください。" : sort_dir);
			Console.WriteLine();

			var download_dir = section["download_dir"];
			Console.WriteLine("【ダウンロードフォルダ】");
			Console.WriteLine(string.IsNullOrWhiteSpace(download_dir) ? "指定してください。" : download_dir);
			if (!string.IsNullOrWhiteSpace(download_dir) && !Directory.Exists(download_dir))
			{
				Console.WriteLine("※ダウンロードフォルダは自動的に作成されます。");
			}
			Console.WriteLine("※Windowsの場合、異なるドライブには保存できません。");
			Console.WriteLine();

			var authkey = section["authkey"];
			Console.WriteLine("【authkey】");
			Console.WriteLine(string.IsNullOrWhiteSpace(authkey) ? "指定してください。" : authkey);

			Console.WriteLine();
			Console.WriteLine();
			if (!string.IsNullOrWhiteSpace(query) 
				&& !string.IsNullOrWhiteSpace(sort_dir)
				&& !string.IsNullOrWhiteSpace(sort)
				&& !string.IsNullOrWhiteSpace(download_dir)
				&& !string.IsNullOrWhiteSpace(authkey))
			{
				Console.WriteLine("ダウンロード開始しますか？ [y] + Enter");
				Console.WriteLine("キャンセルはEnter押下。");
				Console.WriteLine();
			}
			else
			{
				Console.WriteLine("設定ファイル（appsettings.json）が不正です");
				Console.WriteLine("終了はEnter押下。");
				Console.WriteLine();
				Console.ReadLine();
				return null;
			}

			var input = Console.ReadLine();
			if(input == null || 0 == input.Length || "y" != input.ToString().ToLower())
			{
				return null;
			}

			if (!Directory.Exists(download_dir))
			{
				Directory.CreateDirectory(download_dir);
			}


			return new Dictionary<string, string>()
			{
				{ "query", query},
				{ "sort", sort },
				{ "sort_dir", sort_dir },
				{ "pretty", "1" },
				{ "download_dir", download_dir },
				{ "authkey", authkey },
			};

		}
	}

}
