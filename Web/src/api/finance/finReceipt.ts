import {useBaseApi} from '/@/api/base';

// 收款记录表接口服务
export const useFinReceiptApi = () => {
	const baseApi = useBaseApi("finReceipt");
	return {
		// 分页查询收款记录表
		page: baseApi.page,
		// 查看收款记录表详细
		detail: baseApi.detail,
		// 新增收款记录表
		add: baseApi.add,
		// 更新收款记录表
		update: baseApi.update,
		// 删除收款记录表
		delete: baseApi.delete,
		// 批量删除收款记录表
		batchDelete: baseApi.batchDelete,
		// 导出收款记录表数据
		exportData: baseApi.exportData,
		// 导入收款记录表数据
		importData: baseApi.importData,
		// 下载收款记录表数据导入模板
		downloadTemplate: baseApi.downloadTemplate,
		// 获取收款类型字典
		getReceiptTypes: () => baseApi.request({ url: "/api/finReceipt/getReceiptTypes" }),
		// 获取收款状态字典
		getStatuses: () => baseApi.request({ url: "/api/finReceipt/getStatuses" }),
		// 获取新收款单号
		getNewReceiptNo: () => baseApi.request({ url: "/api/finReceipt/getNewReceiptNo" }),
		// 获取客户选择器
		getCustomerSelector: () => baseApi.request({ url: "/api/finReceipt/getCustomerSelector" }),
		// 获取银行账户选择器
		getBankAccountSelector: () => baseApi.request({ url: "/api/finReceipt/getBankAccountSelector" }),
		// 收款单选择器
		selector: () => baseApi.request({ url: "/api/finReceipt/selector" }),
		// 审核收款
		approve: (data: { id: number; approvalRemark?: string }) => baseApi.request({ url: "/api/finReceipt/approve", method: "POST", data }),
		// 拒绝收款
		reject: (data: { id: number; rejectReason: string }) => baseApi.request({ url: "/api/finReceipt/reject", method: "POST", data }),
	}
}

// 收款记录表实体
export interface FinReceipt {
	// 主键Id
	id: number;
	// 收款单号
	receiptNo?: string;
	// 客户ID
	customerId?: number;
	// 客户名称
	customerName?: string;
	// 收款日期
	receiptDate?: string;
	// 收款类型
	receiptType: string;
	// 收款银行账户ID
	bankAccountId: number;
	// 收款银行账户
	bankAccountName: string;
	// 收款金额
	receiptAmount?: number;
	// 已核销金额
	receivedAmount?: number;
	// 未核销金额
	unverifyAmount?: number;
	// 核销单号
	againstNo: string;
	// 状态
	status?: string;
	// 审批人ID
	approvalUserId: number;
	// 审批时间
	approvalTime: string;
	// 审批意见
	approverRemark: string;
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