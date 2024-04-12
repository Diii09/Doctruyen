namespace AppDocTruyen.Models
{
    public class Account
    {
        public int IdAcc { get; set; }
        public string Ten { get; set; }
        public string Username { get; set; }
        public string PwAccount { get; set; }
        public string TrangThai { get; set; }
        public int Role { get; set; }
    }
    public class UpdateAccountRequest
    {
        public string Ten { get; set; }
        public string Username { get; set; }
        public string PwAccount { get; set; }
        public string TrangThai { get; set; }
        public int Role { get; set; }

    }

}