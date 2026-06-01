import {useBaseApi} from '/@/api/base';

// 银行对账单接口服务
export const useFinBankStatementApi = () => {
	const baseApi = useBaseApi("finBankStatement");
	return {
		// 分页查询银行对账单
		page: baseApi.page,
		// 查看银行对账单详细
		detail: baseApi.detail,
		// 新增银行对账单
		add: baseApi.add,
		// 更新银行对账单
		update: baseApi.update,
		// 删除银行对账单
		delete: baseApi.delete,
		// 批量删除银行对账单
		batchDelete: baseApi.batchDelete,
		// 导出银行对账单数据
		exportData: baseApi.exportData,
		// 导入银行对账单数据
		importData: baseApi.importData,
		// 下载银行对账单数据导入模板
		downloadTemplate: baseApi.downloadTemplate,
		// 获取银行账户选择器
		getBankAccountSelector: () => baseApi.request({ url: "/api/finBankStatement/getBankAccountSelector" }),
	}
}

// 银行对账单实体
export interface FinBankStatement {
	// 主键Id
	id: number;
	// 银行账户ID
	bankAccountId?: number;
	// 银行账号
	bankAccountNo?: string;
	// 对账日期
	statementDate?: string;
	// 交易日期
	transactionDate?: string;
	// 交易类型
	transactionType: string;
	// 交易金额
	amount?: number;
	// 余额
	balance?: number;
	// 对方单位
	counterparty: string;
	// 交易描述
	description: string;
	// 是否已匹配
	isMatched?: boolean;
	// 匹配凭证ID
	matchedVoucherId: number;
	// 是否已对账
	isReconciled?: boolean;
	// 对账人ID
	reconcileUserId: number;
	// 对账时间
	reconcileTime: string;
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