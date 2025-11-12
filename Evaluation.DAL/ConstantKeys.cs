namespace Evaluation.DAL;

public static class ConstantKeys
{
    public static class PlanStatus
    {
        public static readonly Guid Draft = Guid.Parse("");
        public static readonly Guid ApproveFromHead = Guid.Parse("");
        public static readonly Guid Approve = Guid.Parse("");


    }

	public static class FormGroupTypeKeyIds
	{
		public static readonly Guid FormGroup = Guid.Parse("b8c6d1b0-d915-442f-a7b4-18f9cd8d6a10");
		public static readonly Guid List = Guid.Parse("e4c923d1-b9fa-4f59-94bc-dae7c90bcf4f");
	}
	public static class TransactionTypeGuidIds
	{
		public static readonly Guid INSERT = Guid.Parse("550e8400-e29b-41d4-a716-446655440001");
		public static readonly Guid APPROVE = Guid.Parse("550e8400-e29b-41d4-a716-446655440002");
		public static readonly Guid REJECT = Guid.Parse("550e8400-e29b-41d4-a716-446655440003");
		public static readonly Guid UPDATE = Guid.Parse("550e8400-e29b-41d4-a716-446655440004");
		public static readonly Guid REQUESTMISSING = Guid.Parse("550e8400-e29b-41d4-a716-446655440005");
	}
	public static class UserGenderIds
	{
		public static readonly Guid MALE = Guid.Parse("550e8400-e29b-41d4-a716-446655440001");
		public static readonly Guid FEMALE = Guid.Parse("550e8400-e29b-41d4-a716-446655440002");
	}
	public static class ActionTypeIds
	{
		public static readonly Guid Draft = Guid.Parse("f328d31b-86ca-4994-b2ae-4c2d6b3dfec3");
		public static readonly Guid Info = Guid.Parse("7227bc28-d11b-4885-a6c0-ad402471d9e3");
		public static readonly Guid INFO_Override_Approve = Guid.Parse("2a9e6657-2122-46da-8790-7dd41a391276");
		public static readonly Guid Edit = Guid.Parse("b8c6d1b0-d915-442f-a7b4-18f9cd8d6a10");
		public static readonly Guid Approve = Guid.Parse("e4c923d1-b9fa-4f59-94bc-dae7c90bcf4f");
		public static readonly Guid Reject = Guid.Parse("9c7a6f74-bb19-41d1-929d-15a12372ff1d");
		public static readonly Guid Assign = Guid.Parse("b163ef2d-fdf4-43c3-8488-22544f5643ae");
		public static readonly Guid ApproveAndAssign = Guid.Parse("8ab3257e-748f-4207-89f7-407e4175b9a7");
		public static readonly Guid RETURNBACK = Guid.Parse("d71bba64-9e7f-4e19-948f-03f08d0243f7");
		public static readonly Guid SubmitMissingData = Guid.Parse("6d697ec3-cc8c-485a-bdf3-ef2e9c24c512");
		public static readonly Guid RequestDataChange = Guid.Parse("cfeb4c71-d1a7-4d35-9a77-5f90e1f8768b");
		public static readonly Guid CreatePlan = Guid.Parse("3e6d8fd3-76f9-479f-94f9-14b8b4a7d589");
		public static readonly Guid ReserveVacancy = Guid.Parse("423dbe7b-3e9d-4f10-bcf4-6d232de5ec39");
		public static readonly Guid INFO_WITH_DRAFT = Guid.Parse("d71bba64-9e7f-4e19-948f-03f08d024311");

	}
}
