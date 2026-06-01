import {useBaseApi} from '/@/api/base';

// 资金账户表接口服务
export const useFinCashAccountApi = () => {
	const baseApi = useBaseApi("finCashAccount");
	return {
		// 分页查询资金账户表
		page: baseApi.page,
		// 查看资金账户表详细
		detail: baseApi.detail,
		// 新增资金账户表
		add: baseApi.add,
		// 更新资金账户表
		update: baseApi.update,
		// 删除资金账户表
		delete: baseApi.delete,
		// 批量删除资金账户表
		batchDelete: baseApi.batchDelete,
		// 导出资金账户表数据
		exportData: baseApi.exportData,
		// 导入资金账户表数据
		importData: baseApi.importData,
		// 下载资金账户表数据导入模板
		downloadTemplate: baseApi.downloadTemplate,
		// 获取账户类型字典
		getAccountTypes: () => baseApi.request({ url: "/api/finCashAccount/getAccountTypes" }),
		// 获取币种字典
		getCurrencies: () => baseApi.request({ url: "/api/finCashAccount/getCurrencies" }),
		// 获取新账户编码
		getNewAccountCode: () => baseApi.request({ url: "/api/finCashAccount/getNewAccountCode" }),
		// 资金账户选择器
		selector: () => baseApi.request({ url: "/api/finCashAccount/selector" }),
	}
}

// 资金账户表实体
export interface FinCashAccount {
	// 主键Id
	id: number;
	// 账户编码
	accountCode?: string;
	// 账户名称
	accountName?: string;
	// 账户类型
	accountType: string;
	// 开户银行
	bankName: string;
	// 银行账号
	bankAccount: string;
	// 期初余额
	openingBalance?: number;
	// 当前余额
	currentBalance?: number;
	// 币种
	currency?: string;
	// 是否启用
	isEnabled?: boolean;
	// 是否默认账户
	isDefault?: boolean;
	// 备注
	remark: string;
	// 创建时间
	createTime: string;
	// 更新时间
	updateTime: string;
	// 创建者Id
	createUserId: number;
	// 创建者姓名
	createUserName: string;
	// 修改者Id
	updateUserId: number;
	// 修改者姓名
	updateUserName: string;
}