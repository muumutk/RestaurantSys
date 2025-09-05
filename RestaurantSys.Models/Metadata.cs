using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantSys.Models;
public class DishData
{
    [Display(Name = "餐點編號")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
    [StringLength(300,ErrorMessage ="照片路徑最多300字元")]
    public string? PhotoPath { get; set; }

    [Display(Name = "價格")]
    [Required(ErrorMessage = "請設定價格")]
    public decimal DishPrice { get; set; }

    [Display(Name = "備註")]
    [StringLength(40, ErrorMessage = "備註最多40個字")]
    public string? Note { get; set; }

    [Display(Name ="是否啟用")]
    public bool IsActive { get; set; } = true;

    [Display(Name = "類別編號")]
    [ForeignKey("DishCategory")]
    [HiddenInput]
    public int DishCategoryID { get; set; }
}


public class DishCategoryData
{
    [Display(Name = "類別編號")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int DishCategoryID { get; set; }

    [Display(Name = "類別名稱")]
    [StringLength(20, ErrorMessage = "類別名稱最多20個字")]
    [Required(ErrorMessage = "類別名稱為必填欄位")]
    public string DishCategoryName { get; set; } = null!;
}

public class MemberData
{
    [Display(Name = "會員編號")]
    [StringLength(9)]
    [HiddenInput]
    public string? MemberID { get; set; } = null!;

    [Display(Name = "姓名")]
    [StringLength(40,ErrorMessage ="姓名最多40個字")]
    [Required(ErrorMessage = "姓名為必填欄位")]
    public string Name { get; set; } = null!;

    [Display(Name = "會員電話")]
    [Required(ErrorMessage = "電話欄位不可為空")]
    [StringLength(20,ErrorMessage ="電話欄位不可超過20碼")]
    public string MemberTel { get; set; } = null!;

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
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
    public DateTime? Birthday { get; set; }

    [Display(Name = "稱謂")]
    [Required(ErrorMessage = "請選擇稱謂")]
    public string title { get; set; } = null!;

    [Display(Name = "電子郵件")]
    [EmailAddress(ErrorMessage = "請輸入有效的電子郵件地址")]
    [StringLength(40, ErrorMessage = "電子郵件最多40個字元")]
    public string? MEmail { get; set; }

    [Display(Name = "密碼")]
    [Required(ErrorMessage = "請設定密碼")]
    [StringLength(20, ErrorMessage = "密碼長度不能超過 20 個字")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;

    [Display(Name = "確認密碼")]
    [DataType(DataType.Password)]
    [Compare("Password", ErrorMessage = "密碼不同！")]
    public string ConfirmPassword { get; set; } = null!;
}

public class EmployeeData
{
    [Display(Name = "員工編號")]
    [Required(ErrorMessage = "員工編號為必填欄位")]
    [StringLength(8,ErrorMessage ="員工編號不可超過8個字")]
    [Key]
    public string EmployeeID { get; set; } = null!;

    [Display(Name="員工姓名")]
    [Required(ErrorMessage ="姓名欄位不可為空")]
    [StringLength(40,ErrorMessage ="姓名欄位最多40個字")]
    public string EName { get; set; } = null!;

    [Display(Name="員工電話")]
    [Required(ErrorMessage ="電話欄位不可為空")]
    [StringLength(20,ErrorMessage ="電話欄位不可超過20碼")]
    public string EmployeeTel { get; set; } = null!;

    [Display(Name = "地址")]
    [Required(ErrorMessage = "地址為必填欄位")]
    [StringLength(100, ErrorMessage = "地址最多100個字")]
    public string Address { get; set; } = null!;

    [Display(Name = "生日")]
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
    [Required(ErrorMessage = "生日欄位不可為空")]
    public DateTime Birthday { get; set; }

    [Display(Name = "到職日")]
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
    [Required(ErrorMessage = "請填寫到職日期")]
    public DateTime HireDate { get; set; }

    [Display(Name = "是否在職")]
    public bool IsEmployed { get; set; } = true;

    [Display(Name = "電子郵件")]
    [EmailAddress(ErrorMessage = "請輸入有效的電子郵件地址")]
    [StringLength(40, ErrorMessage = "電子郵件最多40個字元")]
    public string? EEmail { get; set; }

    [Display(Name = "密碼")]
    [Required(ErrorMessage = "請設定密碼")]
    [StringLength(20, ErrorMessage = "密碼長度不能超過 20 個字")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = null!;
}

public class SupplierData
{
    [Display(Name = "供應商編號")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
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
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int ItemID { get; set; }

    [Display(Name = "物品名稱")]
    [Required(ErrorMessage = "物品名稱為必填欄位")]
    [StringLength(20, ErrorMessage = "物品名稱不能超過20個字")]
    public string ItemName { get; set; } = null!;

    [Display(Name ="現有庫存量")]
    [Required(ErrorMessage ="請填寫現有庫存數量")]
    public int? CurrentStock { get; set; }

    [Display(Name ="安全庫存量")]
    [Required(ErrorMessage ="請填寫安全庫存數量")]
    public int? SafeStock { get; set; }

    [Display(Name ="單位")]
    [StringLength(10,ErrorMessage ="請填寫單位")]
    [Required(ErrorMessage ="單位為必填欄位")]
    public string Unit { get; set; } = null!;

    [Display(Name ="單價")]
    [Required(ErrorMessage = "請填寫單價")]
    public decimal ItemPrice { get; set; }

    [Display(Name = "是否啟用")]
    public bool IsActive { get; set; } = true;

    [Display(Name = "供應商編號")]
    [ForeignKey("Supplier")]
    [HiddenInput]
    public int SupplierID { get; set; }
}

public class StockBatchData
{
    [Display(Name = "批次序號")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int BatchID { get; set; }

    [Display(Name = "批次編號")]
    [StringLength(15, ErrorMessage = "批次編號不能超過15個字元")]
    public string BatchNo { get; set; } = null!;

    [Display(Name = "數量")]
    [Required(ErrorMessage = "請輸入數字")]
    public int Quantity { get; set; }

    [Display(Name = "進貨單價")]
    [Required(ErrorMessage = "請輸入此批貨的單價")]
    public decimal ItemPrice { get; set; }

    [Display(Name = "到貨日期")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
    [Required(ErrorMessage = "請選擇到貨日期")]
    public DateTime ArrivalDate { get; set; }

    [Display(Name = "有效日期")]
    [DataType(DataType.Date)]
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
    [Required(ErrorMessage = "請選擇有效日期")]
    public DateTime ExpiryDate { get; set; }

    [Display(Name = "員工")]
    [ForeignKey("Employee")]
    [Required(ErrorMessage = "請選擇員工")]
    public string EmployeeID { get; set; } = null!;

    [Display(Name = "物品編號")]
    [Required(ErrorMessage = "請選擇物品")]
    [ForeignKey("Stock")]
    public int ItemID { get; set; }
}

public class StockBatchWarningLogData
{
    [Display(Name = "警示紀錄編號")]
    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    [Key]
    public int StockBatchWarningLogID { get; set; }

    [Display(Name = "警示發送日期")]
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss}", ApplyFormatInEditMode = true)]
    public DateTime WarningSentDate { get; set; }

    [Display(Name = "批次序號")]
    [ForeignKey("StockBatch")]
    public int? BatchID { get; set; }


    [Display(Name ="員工編號")]
    [ForeignKey("Employee")]
    public string? EmployeeID { get; set; } = null!;
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
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm:ss}", ApplyFormatInEditMode = true)]
    [Required]
    public DateTime OrderDate { get; set; }

    [Display(Name = "預計取餐時間")]
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm}", ApplyFormatInEditMode = true)]
    public DateTime PickUpTime { get; set; }

    [Display(Name = "備註")]
    [StringLength(100, ErrorMessage = "備註不能超過100個字")]
    public string? Note { get; set; }

    [Display(Name = "會員編號")]
    [ForeignKey("Member")]
    [HiddenInput]
    public string MemberID { get; set; } = null!;

    [Display(Name = "員工編號")]
    [ForeignKey("Employee")]
    [HiddenInput]
    public string EmployeeID { get; set; } = null!;
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

    [Display(Name = "餐點數量")]
    [Required]
    public int Quantity { get; set; }

    [Display(Name = "餐點金額")]
    [Required]
    public decimal UnitPrice { get; set; }

    [Display(Name = "實際取餐時間")]
    [DataType(DataType.DateTime)]
    [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd hh:mm}", ApplyFormatInEditMode = true)]
    public DateTime? GetTime { get; set; }
}

    public class DishIngredientData
{
    [Display(Name = "餐點編號")]
    [Key]
    [ForeignKey("Dish")]
    public int DishID { get; set; }

    [Display(Name = "物品編號")]
    [Key]
    [ForeignKey("Stock")]
    public int ItemID { get; set; }

    [Display(Name = "是否啟用")]
    [Required]
    public bool IsActive { get; set; }
}

[ModelMetadataType(typeof(DishData))]
public partial class Dish
{ 
}

[ModelMetadataType(typeof(DishCategoryData))]
public partial class DishCategory
{
}

[ModelMetadataType(typeof(MemberData))]
public partial class Member
{
}


[ModelMetadataType(typeof(EmployeeData))]
public partial class Employee
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

[ModelMetadataType(typeof(StockBatchData))]
public partial class StockBatch
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