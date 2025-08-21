using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantSys.Models;
public class DishData
{
    [Display(Name = "餐點編號")]
    [Key]
    [HiddenInput]
    public int DishID { get; set; }

    [Display(Name = "餐點名稱")]
    [StringLength(20, ErrorMessage = "餐點名稱不能超過20個字")]
    [Required(ErrorMessage = "餐點名稱為必填欄位")]
    public string DishName { get; set; } = null!;

    [Display(Name = "餐點描述")]
    [StringLength(50, ErrorMessage = "餐點描述不能超過50個字")]
    [Required(ErrorMessage ="餐點描述不可為空")]
    public string Description { get; set; } = null!;

    [Display(Name = "照片")]
    [StringLength(50,ErrorMessage ="照片路徑最多50字元")]
    public string? PhotoPath { get; set; }

    [Display(Name = "價格")]
    [Required(ErrorMessage = "請設定價格")]
    public decimal DishPrice { get; set; }

    [Display(Name = "備註")]
    [StringLength(40, ErrorMessage = "備註最多40個字")]
    public string? Note { get; set; }
}


public class MemberData
{
    [Display(Name = "會員編號")]
    [StringLength(9,ErrorMessage ="會員編號不可超過9個字")]
    [Required(ErrorMessage ="會員編號為必填欄位")]
    [Key]
    public string MemberID { get; set; } = null!;

    [Display(Name = "姓名")]
    [StringLength(40,ErrorMessage ="姓名最多40個字")]
    [Required(ErrorMessage = "姓名為必填欄位")]
    public string Name { get; set; } = null!;

    [Display(Name="縣市")]
    [Required(ErrorMessage = "縣市為必填欄位")]
    [StringLength(10,ErrorMessage ="縣市欄位最多10個字")]
    public string City { get; set; } = null!;

    [Display(Name = "地址")]
    [Required(ErrorMessage = "地址為必填欄位")]
    [StringLength(100,ErrorMessage ="地址最多100個字")]
    public string Address { get; set; } = null!;

    [Display(Name = "生日")]
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
    [Required(ErrorMessage ="生日欄位不可為空")]
    public DateTime Birthday { get; set; }

    [Display(Name = "稱謂")]
    [Required(ErrorMessage = "請輸入稱謂，例如：先生、小姐、Mr.、Ms.")]
    [StringLength(10, ErrorMessage = "稱謂長度不能超過 10 個字元")]
    public string title { get; set; } = null!;


    [Display(Name = "密碼")]
    [Required(ErrorMessage = "請設定密碼")]
    [StringLength(20, ErrorMessage = "密碼長度不能超過 20 個字")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
}

public class MemberTelData
{
    [Display(Name = "序號")]
    [Required]
    [Key]
    public int SN { get; set; }

    [Display(Name ="會員電話")]
    [Required]
    [StringLength(20,ErrorMessage ="電話不可超過20碼")]
    public string MemTel { get; set; } = null!;

    [ForeignKey("Member)")]
    [HiddenInput]
    public string MemberID { get; set; } = null!;
}
public class StaffData
{
    [Display(Name = "員工編號")]
    [Required(ErrorMessage = "員工編號為必填欄位")]
    [StringLength(8,ErrorMessage ="員工編號不可超過8個字")]
    [Key]
    public string StaffID { get; set; } = null!;

    [Display(Name="姓名")]
    [Required(ErrorMessage ="姓名欄位不可為空")]
    [StringLength(40,ErrorMessage ="姓名欄位最多40個字")]
    public string Name { get; set; } = null!;

    [Display(Name="員工電話")]
    [Required(ErrorMessage ="電話欄位不可為空")]
    [StringLength(20,ErrorMessage ="電話欄位不可超過20碼")]
    public string StaffTel { get; set; } = null!;

    [Display(Name = "地址")]
    [Required(ErrorMessage = "地址為必填欄位")]
    [StringLength(100, ErrorMessage = "地址最多100個字")]
    public string Address { get; set; } = null!;

    [Display(Name = "生日")]
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
    [Required(ErrorMessage = "生日欄位不可為空")]
    public DateTime Birthday { get; set; }

    [Display(Name = "到職日")]
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}")]
    [Required(ErrorMessage = "起填寫到職日期")]
    public DateTime HireDate { get; set; }

    [Display(Name = "密碼")]
    [Required(ErrorMessage = "請設定密碼")]
    [StringLength(20, ErrorMessage = "密碼長度不能超過 20 個字")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
}

public class SupplierData
{
    [Display(Name = "供應商編號")]
    [Required]
    [Key]
    public int SupplierID { get; set; }

    [Display(Name = "供應商名稱")]
    [Required(ErrorMessage = "供應商名稱為必填欄位")]
    [StringLength(20, ErrorMessage = "供應商名稱不能超過20個字")]
    public string SupplierName { get; set; } = null!;

    [Display(Name = "聯絡人")]
    [Required(ErrorMessage = "聯絡人為必填欄位")]
    [StringLength(30, ErrorMessage = "聯絡人名稱不能超過30個字")]
    public string ContactPerson { get; set; } = null!;

    [Display(Name = "供應商電話")]
    [Required(ErrorMessage = "供應商電話為必填欄位")]
    [StringLength(20, ErrorMessage = "供應商電話不能超過20個字")]
    public string SupplierTel { get; set; } = null!;

    [Display(Name = "地址")]
    [Required(ErrorMessage = "地址為必填欄位")]
    [StringLength(100, ErrorMessage = "地址最多100個字")]
    public string Address { get; set; } = null!;
}

public class StockData
{
    [Display(Name = "物品編號")]
    [Required]
    [Key]
    public int ItemID { get; set; }

    [Display(Name = "物品名稱")]
    [Required(ErrorMessage = "物品名稱為必填欄位")]
    [StringLength(20, ErrorMessage = "物品名稱不能超過20個字")]
    public string ItemName { get; set; } = null!;

    [Display(Name ="現有庫存量")]
    [Required(ErrorMessage ="請填寫現有庫存數量")]
    public int CurrentStock { get; set; }

    [Display(Name ="安全庫存量")]
    [Required(ErrorMessage ="請填寫安全庫存數量")]
    public int SafeStock { get; set; }

    [Display(Name ="單位")]
    [StringLength(10,ErrorMessage ="請填寫單位")]
    [Required(ErrorMessage ="單位為必填欄位")]
    public string Unit { get; set; } = null!;

    [Display(Name ="單價")]
    [Required(ErrorMessage = "請填寫單價")]
    public decimal ItemPrice { get; set; }

    [Display(Name = "是否啟用")]
    public bool IsActive { get; set; } = true;

    [ForeignKey("Supplier")]
    [HiddenInput]
    public string SupplierID { get; set; } = null!;
}

public class OrderData
{
    [Display(Name = "訂單編號")]
    [Required]
    [StringLength(12)]
    [Key]
    public string OrderID { get; set; } = null!;

    [Display(Name = "訂單日期")]
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss}")]
    [Required]
    public DateTime OrderDate { get; set; }

    [Display(Name = "預計取餐時間")]
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm}")]
    public DateTime PickUpTime { get; set; }

    [Display(Name = "備註")]
    [StringLength(100, ErrorMessage = "備註不能超過100個字")]
    public string? Note { get; set; }

    [ForeignKey("Member")]
    [HiddenInput]
    public string MemberID { get; set; } = null!;

    [ForeignKey("Staff")]
    [HiddenInput]
    public string StaffID { get; set; } = null!;
}

public class OrderDetailData
{
    [Display(Name = "訂單編號")]
    [Required]
    [Key]
    [ForeignKey("Order")]
    public string OrderID { get; set; } = null!;

    [Display(Name = "餐點編號")]
    [Required]
    [Key]
    [ForeignKey("Dish")]
    public int DishID { get; set; }

    [Display(Name = "總金額")]
    public decimal Price { get; set; }

    [Display(Name = "實際取餐時間")]
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm}")]
    [Required]
    public DateTime GetTime { get; set; }
}

    public class DishIngredientData
{
    [Display(Name = "餐點編號")]
    [Key]
    [ForeignKey("Dish")]
    public long DishID { get; set; }

    [Display(Name = "物品編號")]
    [Key]
    [ForeignKey("Stock")]
    public long ItemID { get; set; }

    [Display(Name = "是否啟用")]
    [Required]
    public bool IsActive { get; set; }
}

[ModelMetadataType(typeof(DishData))]
public partial class Dish
{ 
}

[ModelMetadataType(typeof(MemberData))]
public partial class Member
{
}

[ModelMetadataType(typeof(MemberTelData))]
public partial class MemberTel
{
}

[ModelMetadataType(typeof(StaffData))]
public partial class Staff
{
}

[ModelMetadataType(typeof(SupplierData))]
public partial class Supplier
{
}

[ModelMetadataType(typeof(StockData))]
public partial class Stock
{
}

[ModelMetadataType(typeof(OrderData))]
public partial class Order
{
}

[ModelMetadataType(typeof(OrderDetailData))]
public partial class OrderDetail
{
}

[ModelMetadataType(typeof(DishIngredientData))]
public partial class DishIngredient
{
}