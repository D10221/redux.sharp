namespace server.Database
{
    public class Contact
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Tel { get; set; }
        public string Email { get; set; }
        public string Address { get; set; }
    }

    public class Account
    {
        public int ID { get; set; }
        public string Code { get; set; }
    }

    public class WhareHouse
    {
        public int ID { get; set; }
        public string Code { get; set; }
    }

    public class WhareHouseStock
    {
        public int ID { get; set; }
        public string StockID { get; set; }
        public double Qty { get; set; }
    }

    public class Stock
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public int PriceListID { get; set; }
        public int Value { get; set; }
    }

    public class PriceList
    {
        public int ID { get; set; }
        public string Code { get; set; }
    }

    public class Price
    {
        public int ID { get; set; }
        public string Code { get; set; }
        public int PriceList { get; set; }
        public int StockID { get; set; }
        public double Value { get; set; }
    }

    public class Ledger
    {
        public int ID { get; set; }
        public int DocID { get; set; }
        public double Debit { get; set; }
        public double Crebit { get; set; }
        public int From { get; set; }
        public int To { get; set; }
    }

    public class Document
    {
        public int ID { get; set; }
        public string Code { get; set; }
        // from account
        public int From { get; set; }
        // to account
        public int To { get; set; }

        public int Type { get; set; }
    }

    public class DocumentLine
    {
        public int ID { get; set; }
        public int StockID { get; set; }
    }
}