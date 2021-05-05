using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace WebAPIClient
{

	public class Rootobject
	{
		public bool ok { get; set; }
		public string query { get; set; }
		public Files files { get; set; }
	}

	public class Files
	{
		public int total { get; set; }
		public Pagination pagination { get; set; }
		public Paging paging { get; set; }
		public Match[] matches { get; set; }
	}

	public class Pagination
	{
		public int total_count { get; set; }
		public int page { get; set; }
		public int per_page { get; set; }
		public int page_count { get; set; }
		public int first { get; set; }
		public int last { get; set; }
	}

	public class Paging
	{
		public int count { get; set; }
		public int total { get; set; }
		public int page { get; set; }
		public int pages { get; set; }
	}

	public class Match
	{
		public string id { get; set; }
		public int created { get; set; }
		public int timestamp { get; set; }
		public string name { get; set; }
		public string title { get; set; }
		public string mimetype { get; set; }
		public string filetype { get; set; }
		public string pretty_type { get; set; }
		public string user { get; set; }
		public bool editable { get; set; }
		public int size { get; set; }
		public string mode { get; set; }
		public bool is_external { get; set; }
		public string external_type { get; set; }
		public bool is_public { get; set; }
		public bool public_url_shared { get; set; }
		public bool display_as_bot { get; set; }
		public string username { get; set; }
		public string url_private { get; set; }
		public string url_private_download { get; set; }
		public string thumb_64 { get; set; }
		public string thumb_80 { get; set; }
		public string thumb_360 { get; set; }
		public int thumb_360_w { get; set; }
		public int thumb_360_h { get; set; }
		public string thumb_480 { get; set; }
		public int thumb_480_w { get; set; }
		public int thumb_480_h { get; set; }
		public string thumb_160 { get; set; }
		public string thumb_720 { get; set; }
		public int thumb_720_w { get; set; }
		public int thumb_720_h { get; set; }
		public string thumb_800 { get; set; }
		public int thumb_800_w { get; set; }
		public int thumb_800_h { get; set; }
		public string thumb_960 { get; set; }
		public int thumb_960_w { get; set; }
		public int thumb_960_h { get; set; }
		public string thumb_1024 { get; set; }
		public int thumb_1024_w { get; set; }
		public int thumb_1024_h { get; set; }
		public int original_w { get; set; }
		public int original_h { get; set; }
		public string thumb_tiny { get; set; }
		public string permalink { get; set; }
		public string permalink_public { get; set; }
		public bool is_starred { get; set; }
		public Shares shares { get; set; }
		public object[] channels { get; set; }
		public string[] groups { get; set; }
		public object[] ims { get; set; }
		public bool has_rich_preview { get; set; }
		public string converted_pdf { get; set; }
		public string thumb_pdf { get; set; }
		public int thumb_pdf_w { get; set; }
		public int thumb_pdf_h { get; set; }
	}

	public class Shares
	{
		public Private _private { get; set; }
	}

	public class Private
	{
		public GP3G0NW0L[] GP3G0NW0L { get; set; }
		public G01HGAV7S4C[] G01HGAV7S4C { get; set; }
	}

	public class GP3G0NW0L
	{
		public object[] reply_users { get; set; }
		public int reply_users_count { get; set; }
		public int reply_count { get; set; }
		public string ts { get; set; }
		public string channel_name { get; set; }
		public string team_id { get; set; }
		public string share_user_id { get; set; }
	}

	public class G01HGAV7S4C
	{
		public object[] reply_users { get; set; }
		public int reply_users_count { get; set; }
		public int reply_count { get; set; }
		public string ts { get; set; }
		public string channel_name { get; set; }
		public string team_id { get; set; }
		public string share_user_id { get; set; }
	}

}