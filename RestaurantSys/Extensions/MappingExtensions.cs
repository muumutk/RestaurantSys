using RestaurantSys.DTOs;
using RestaurantSys.Models;

public static class MappingExtensions
{
    // 擴充方法必須是 public static，並且第一個參數前面要加 this

    // Dish -> DishDTO 的擴充方法
    public static DishDTO ToDto(this Dish dish)
    {
        return new DishDTO
        {
            DishID = dish.DishID,
            DishCategoryID = dish.DishCategoryID,
            DishName = dish.DishName,
            Description = dish.Description,
            PhotoPath = dish.PhotoPath,
            DishPrice = dish.DishPrice,
            Note = dish.Note,
            IsActive = dish.IsActive
        };
    }


    // DishDTO -> Dish 的擴充方法
    public static Dish ToModel(this DishDTO dishDTO)
    {
        return new Dish
        {
            DishID = dishDTO.DishID,
            DishCategoryID = dishDTO.DishCategoryID,
            DishName = dishDTO.DishName,
            Description = dishDTO.Description,
            PhotoPath = dishDTO.PhotoPath,
            DishPrice = dishDTO.DishPrice,
            Note = dishDTO.Note,
            IsActive = dishDTO.IsActive
        };
    }
}