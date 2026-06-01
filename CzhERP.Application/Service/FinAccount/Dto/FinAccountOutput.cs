// Admin.NET 项目的版权、商标、专利和其他相关权利均受相应法律法规的保护。使用本项目应遵守相关法律法规和许可证的要求。
//
// 本项目主要遵循 MIT 许可证和 Apache 许可证（版本 2.0）进行分发和使用。许可证位于源代码树根目录中的 LICENSE-MIT 和 LICENSE-APACHE 文件。
//
// 不得利用本项目从事危害国家安全、扰乱社会秩序、侵犯他人合法权益等法律法规禁止的活动！任何基于本项目二次开发而产生的一切法律纠纷和责任，我们不承担任何责任！
using Magicodes.ExporterAndImporter.Core;
namespace CzhERP.Application;

/// <summary>
/// 会计科目表输出参数
/// </summary>
public class FinAccountOutput
{
    /// <summary>
    /// 主键Id
    /// </summary>
    public long Id { get; set; }    
    
    /// <summary>
    /// 科目编码
    /// </summary>
    public string AccountCode { get; set; }    
    
    /// <summary>
    /// 科目名称
    /// </summary>
    public string AccountName { get; set; }    
    
    /// <summary>
    /// 科目全称
    /// </summary>
    public string? FullName { get; set; }    
    
    /// <summary>
    /// 上级科目ID
    /// </summary>
    public long? ParentId { get; set; }    
    
    /// <summary>
    /// 上级科目名称
    /// </summary>
    public string? ParentAccountName { get; set; }
    
    /// <summary>
    /// 科目级次
    /// </summary>
    public int Level { get; set; }    
    
    /// <summary>
    /// 科目类型
    /// </summary>
    public string? AccountType { get; set; }
    
    /// <summary>
    /// 科目类型（显示用）
    /// </summary>
    public string AccountTypeText => AccountType switch
    {
        "Asset" => "资产",
        "Liability" => "负债",
        "Equity" => "权益",
        "Cost" => "成本",
        "Profit" => "损益",
        _ => AccountType ?? ""
    };
    
    /// <summary>
    /// 余额方向
    /// </summary>
    public string? Direction { get; set; }
    
    /// <summary>
    /// 余额方向（显示用）
    /// </summary>
    public string DirectionText => Direction switch
    {
        "Debit" => "借方",
        "Credit" => "贷方",
        _ => Direction ?? ""
    };
    
    /// <summary>
    /// 是否明细科目
    /// </summary>
    public bool IsDetail { get; set; }
    
    /// <summary>
    /// 是否明细科目（显示用）
    /// </summary>
    public string IsDetailText => IsDetail ? "是" : "否";
    
    /// <summary>
    /// 是否辅助核算
    /// </summary>
    public bool IsAuxiliary { get; set; }
    
    /// <summary>
    /// 是否辅助核算（显示用）
    /// </summary>
    public string IsAuxiliaryText => IsAuxiliary ? "是" : "否";
    
    /// <summary>
    /// 是否现金流量科目
    /// </summary>
    public bool IsCashFlow { get; set; }
    
    /// <summary>
    /// 是否现金流量科目（显示用）
    /// </summary>
    public string IsCashFlowText => IsCashFlow ? "是" : "否";
    
    /// <summary>
    /// 部门辅助核算
    /// </summary>
    public bool AuxDept { get; set; }
    
    /// <summary>
    /// 部门辅助核算（显示用）
    /// </summary>
    public string AuxDeptText => AuxDept ? "是" : "否";
    
    /// <summary>
    /// 个人辅助核算
    /// </summary>
    public bool AuxPerson { get; set; }
    
    /// <summary>
    /// 个人辅助核算（显示用）
    /// </summary>
    public string AuxPersonText => AuxPerson ? "是" : "否";
    
    /// <summary>
    /// 项目辅助核算
    /// </summary>
    public bool AuxProject { get; set; }
    
    /// <summary>
    /// 项目辅助核算（显示用）
    /// </summary>
    public string AuxProjectText => AuxProject ? "是" : "否";
    
    /// <summary>
    /// 供应商辅助核算
    /// </summary>
    public bool AuxSupplier { get; set; }
    
    /// <summary>
    /// 供应商辅助核算（显示用）
    /// </summary>
    public string AuxSupplierText => AuxSupplier ? "是" : "否";
    
    /// <summary>
    /// 客户辅助核算
    /// </summary>
    public bool AuxCustomer { get; set; }
    
    /// <summary>
    /// 客户辅助核算（显示用）
    /// </summary>
    public string AuxCustomerText => AuxCustomer ? "是" : "否";
    
    /// <summary>
    /// 存货辅助核算
    /// </summary>
    public bool AuxInventory { get; set; }
    
    /// <summary>
    /// 存货辅助核算（显示用）
    /// </summary>
    public string AuxInventoryText => AuxInventory ? "是" : "否";
    
    /// <summary>
    /// 是否启用
    /// </summary>
    public bool IsEnabled { get; set; }
    
    /// <summary>
    /// 是否启用（显示用）
    /// </summary>
    public string IsEnabledText => IsEnabled ? "是" : "否";
    
    /// <summary>
    /// 排序号
    /// </summary>
    public int SortOrder { get; set; }    
    
    /// <summary>
    /// 创建时间
    /// </summary>
    public DateTime? CreateTime { get; set; }    
    
    /// <summary>
    /// 更新时间
    /// </summary>
    public DateTime? UpdateTime { get; set; }    
    
    /// <summary>
    /// 创建者Id
    /// </summary>
    public long? CreateUserId { get; set; }    
    
    /// <summary>
    /// 创建者姓名
    /// </summary>
    public string? CreateUserName { get; set; }    
    
    /// <summary>
    /// 修改者Id
    /// </summary>
    public long? UpdateUserId { get; set; }    
    
    /// <summary>
    /// 修改者姓名
    /// </summary>
    public string? UpdateUserName { get; set; }    
    
}

/// <summary>
/// 会计科目表下拉选择输出
/// </summary>
public class FinAccountSelectorOutput
{
    /// <summary>
    /// 科目ID
    /// </summary>
    public long Id { get; set; }
    
    /// <summary>
    /// 科目编码
    /// </summary>
    public string AccountCode { get; set; }
    
    /// <summary>
    /// 科目名称
    /// </summary>
    public string AccountName { get; set; }
    
    /// <summary>
    /// 科目级次
    /// </summary>
    public int Level { get; set; }
    
    /// <summary>
    /// 上级科目ID
    /// </summary>
    public long? ParentId { get; set; }
}

/// <summary>
/// 会计科目表数据导入模板实体
/// </summary>
public class ExportFinAccountOutput : ImportFinAccountInput
{
    [ImporterHeader(IsIgnore = true)]
    [ExporterHeader(IsIgnore = true)]
    public override string Error { get; set; }
}
