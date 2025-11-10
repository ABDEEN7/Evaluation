namespace Evaluation.SharedHelper.Models.Admin
{
    public class GeneralListDTO
	{
		public int ID { get; set; }
		public string Value { get; set; } = null!;

        public string Title { get; set; } = null!;
        public string TextAr { get; set; } = null!;
        public string TextEn { get; set; } = null!;
        public int ParentID { get; set; }
		public int OrderNo { get; set; }
		public List<GeneralListDTO>? ParentChild { get; set; }



		
	}
}
