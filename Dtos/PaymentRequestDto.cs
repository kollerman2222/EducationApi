namespace FgssrApi.Dtos
{
    public class PaymentRequestDto
    {
        public int amount { get; set; }
        public int amount_cents { get; set; }
        public string? currency { get; set; }
        public int[]? payment_methods { get; set; }
        public Items[]? items { get; set; }
        public billing_data? billing_data { get; set; }
        public Customer? customer { get; set; }
        public bool is_live { get; set; }
        public string? transaction_id { get; set; }

        public string? name { get; set; }
        public string? email { get; set; }
        public string? full_name { get; set; }
        public string? phone_number { get; set; }
        public string? token { get; set; }

    }

    public class Items
    {
        public string? name { get; set; }
        public string? description { get; set; }
        public int amount { get; set; }
    }


    public class Customer
    {
        public string? first_name { get; set; }
        public string? last_name { get; set; }
        public string? email { get; set; }
    }



    public class billing_data
    {
        public string? first_name { get; set; }
        public string? last_name { get; set; }
        public string? email { get; set; }
        public string? phone_number { get; set; }
        //public string street { get; set;}
        //public string apartment { get; set;}
        //public string city { get; set;}
        //public string country { get; set;}
        //public string floor { get; set;}
        //public string state { get; set;}
        //public string building { get; set;}

    }
}
