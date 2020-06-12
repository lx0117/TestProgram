using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ApiTest
{
	public class FaceIDCard
	{
		public string image { get; set; }
		public string image_type { get; set; }
		public string id_card_number { get; set; }
		public string name { get; set; }
		public string quality_control { get; set; }
		public string liveness_control { get; set; }
	}
}
