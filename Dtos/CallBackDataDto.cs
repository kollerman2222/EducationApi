namespace FgssrApi.Dtos
{
    public class CallBackDataDto
    {
        public Obj obj { get; set; }

    }

    public class Obj
    {
        public int amount_cents { get; set; }
        public string created_at { get; set; }
        public string currency { get; set; }
        public bool error_occured { get; set; }
        public bool has_parent_transaction { get; set; }
        public int id { get; set; }
        public int integration_id { get; set; }
        public bool is_3d_secure { get; set; }
        public bool is_auth { get; set; }
        public bool is_capture { get; set; }
        public bool is_refunded { get; set; }
        public bool is_standalone_payment { get; set; }
        public bool is_voided { get; set; }
        public bool pending { get; set; }
        public bool success { get; set; }
        public int owner { get; set; }

        public source_data source_data { get; set; }
        public order order { get; set; }
    }


  

    public class order
    {
        public int id { get; set; }

    }

    public class source_data
    {
        public string pan { get; set; }
        public string type { get; set; }
        public string sub_type { get; set; }
    }


}
