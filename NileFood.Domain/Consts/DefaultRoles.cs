namespace NileFood.Domain.Consts;

public static class DefaultRoles
{
    public partial class Admin
    {
        public const string Name = nameof(Admin);
        public const string Id = "0191d311-2918-7f76-bd8a-0bded8535075";
        public const string ConcurrencyStamp = "0191d311-2918-7f76-bd8a-0be08dd83078";
    }


    public partial class Restaurant
    {
        public const string Name = nameof(Restaurant);
        public const string Id = "0191d311-2918-7f76-bd8a-0bdf740adff8";
        public const string ConcurrencyStamp = "0191d311-2918-7f76-bd8a-0be100f11384";
    }

    public partial class Branch
    {
        public const string Name = nameof(Branch);
        public const string Id = "feb07bce-2808-40bb-8561-9b95ed1d2816";
        public const string ConcurrencyStamp = "644f048f-1dcf-4b6c-b21c-55a24018046e";
    }

    public partial class User
    {
        public const string Name = nameof(User);
        public const string Id = "ce2fd704-7a3c-4a03-846e-c5479a8b921d";
        public const string ConcurrencyStamp = "aacecacd-28e1-43cc-92da-decb1f9b32c4";
    }

}
