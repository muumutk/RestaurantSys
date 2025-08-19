namespace RestaurantSys.Models
{
    public class MemberTel
    {
        public int SN { get; set; }

        public string MemTel { get; set; } = null!;

        public string MemberID { get; set; } = null!;

        public virtual Member Member { get; set; } = null!;

    }
}
