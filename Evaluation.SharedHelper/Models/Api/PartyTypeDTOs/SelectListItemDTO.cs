using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Evaluation.SharedHelper.Models.Api.PartyTypeDTOs
{
	public class SelectListItemDTO
	{

		public string Value { get; set; } = null!;
		public string TextAr { get; set; } = null!;
		public string TextEn { get; set; } = null!;
		public string Text { get; set; } = null!;
		public Guid? Parent { get; set; }

	}
}
