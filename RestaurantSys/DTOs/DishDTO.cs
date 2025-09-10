namespace RestaurantSys.DTOs
{
    public class DishDTO
    {
        public int DishID { get; set; }
        public string DishName { get; set; }
        public string Description { get; set; }
        public string PhotoPath { get; set; }
        public decimal DishPrice { get; set; }
        public string Note { get; set; }
    }
}
