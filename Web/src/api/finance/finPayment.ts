import {useBaseApi} from '/@/api/base';

// 付款记录表接口服务
export const useFinPaymentApi = () => {
	const baseApi = useBaseApi("finPayment");
	return {
		// 分页查询付款记录表
		page: baseApi.page,
		// 查看付款记录表详细
		detail: baseApi.detail,
		// 新增付款记录表
		add: baseApi.add,
		// 更新付款记录表
		update: baseApi.update,
		// 删除付款记录表
		delete: baseApi.delete,
		// 批量删除付款记录表
		batchDelete: baseApi.batchDelete,
		// 导出付款记录表数据
		exportData: baseApi.exportData,
		// 导入付款记录表数据
		importData: baseApi.importData,
		// 下载付款记录表数据导入模板
		downloadTemplate: baseApi.downloadTemplate,
		// 获取付款类型字典
		getPaymentTypes: () => baseApi.request({ url: "/api/finPayment/getPaymentTypes" }),
		// 获取付款方式字典
		getPaymentMethods: () => baseApi.request({ url: "/api/finPayment/getPaymentMethods" }),
		// 获取付款状态字典
		getStatuses: () => baseApi.request({ url: "/api/finPayment/getStatuses" }),
		// 获取新付款单号
		getNewPaymentNo: () => baseApi.request({ url: "/api/finPayment/getNewPaymentNo" }),
		// 获取供应商选择器
		getSupplierSelector: () => baseApi.request({ url: "/api/finPayment/getSupplierSelector" }),
		// 获取银行账户选择器
		getBankAccountSelector: () => baseApi.request({ url: "/api/finPayment/getBankAccountSelector" }),
		// 付款单选择器
		selector: () => baseApi.request({ url: "/api/finPayment/selector" }),
		// 审核付款
		approve: (data: { id: number; approvalRemark?: string }) => baseApi.request({ url: "/api/finPayment/approve", method: "POST", data }),
		// 拒绝付款
		reject: (data: { id: number; rejectReason: string }) => baseApi.request({ url: "/api/finPayment/reject", method: "POST", data }),
	}
}

// 付款记录表实体
export interface FinPayment {
	// 主键Id
	id: number;
	// 付款单号
	paymentNo?: string;
	// 供应商ID
	supplierId?: number;
	// 供应商名称
	supplierName?: string;
	// 付款日期
	paymentDate?: string;
	// 付款类型
	paymentType: string;
	// 付款银行账户ID
	bankAccountId?: number;
	// 付款银行账户
	bankAccountName?: string;
	// 付款金额
	paymentAmount?: number;
	// 已核销金额
	paidAmount?: number;
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
	// 付款方式
	paymentMethod: string;
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