namespace HRR.Model
{
    public class User
    {
        public long Id { get; set; }
        public string UserName{ get; set; }
        public string HashedPassword{ get; set; }//"Admin@123" // # rfew231423DSdfv
        public string IsAdmin{ get; set; }
    }
}
