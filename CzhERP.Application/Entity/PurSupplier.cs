namespace CzhERP.Application.Entity;

[SugarTable("Pur_Supplier", TableDescription = "供应商主表")]
[SugarIndex("index_{table}_SC", nameof(SupplierCode), OrderByType.Asc, IsUnique = true)]
public class PurSupplier : EntityBase
{
    [SugarColumn(ColumnDescription = "供应商编码", Length = 50, IsNullable = false)]
    public string SupplierCode { get; set; }

    [SugarColumn(ColumnDescription = "供应商名称", Length = 100, IsNullable = false)]
    public string SupplierName { get; set; }

    [SugarColumn(ColumnDescription = "简称", Length = 50)]
    public string? ShortName { get; set; }

    [SugarColumn(ColumnDescription = "分类ID")]
    public long? CategoryId { get; set; }

    [SugarColumn(ColumnDescription = "联系人", Length = 50)]
    public string? ContactName { get; set; }

    [SugarColumn(ColumnDescription = "联系电话", Length = 20)]
    public string? Phone { get; set; }

    [SugarColumn(ColumnDescription = "手机", Length = 20)]
    public string? Mobile { get; set; }

    [SugarColumn(ColumnDescription = "邮箱", Length = 100)]
    public string? Email { get; set; }

    [SugarColumn(ColumnDescription = "地址", Length = 500)]
    public string? Address { get; set; }

    [SugarColumn(ColumnDescription = "开户银行", Length = 100)]
    public string? BankName { get; set; }

    [SugarColumn(ColumnDescription = "银行账号", Length = 50)]
    public string? BankAccount { get; set; }

    [SugarColumn(ColumnDescription = "税号", Length = 50)]
    public string? TaxNo { get; set; }

    [SugarColumn(ColumnDescription = "信用等级(1-5)", DefaultValue = "0")]
    public int CreditRating { get; set; } = 0;

    [SugarColumn(ColumnDescription = "状态(0禁用/1启用)", DefaultValue = "1")]
    public int Status { get; set; } = 1;

    [SugarColumn(ColumnDescription = "是否黑名单", DefaultValue = "false")]
    public bool IsBlacklist { get; set; } = false;

    [SugarColumn(ColumnDescription = "备注", Length = 500)]
    public string? Remark { get; set; }

    [Navigate(NavigateType.OneToOne, nameof(CategoryId))]
    public PurSupplierCategory? Category { get; set; }
}