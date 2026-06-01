import {useBaseApi} from '/@/api/base';

// 应收账款表接口服务
export const useFinReceivableApi = () => {
	const baseApi = useBaseApi("finReceivable");
	return {
		// 分页查询应收账款表
		page: baseApi.page,
		// 查看应收账款表详细
		detail: baseApi.detail,
		// 新增应收账款表
		add: baseApi.add,
		// 更新应收账款表
		update: baseApi.update,
		// 删除应收账款表
		delete: baseApi.delete,
		// 批量删除应收账款表
		batchDelete: baseApi.batchDelete,
		// 导出应收账款表数据
		exportData: baseApi.exportData,
		// 导入应收账款表数据
		importData: baseApi.importData,
		// 下载应收账款表数据导入模板
		downloadTemplate: baseApi.downloadTemplate,
		// 获取状态字典
		getStatuses: () => baseApi.request({ url: "/api/finReceivable/getStatuses" }),
		// 获取新应收单号
		getNewReceivableNo: () => baseApi.request({ url: "/api/finReceivable/getNewReceivableNo" }),
		// 获取客户选择器
		getCustomerSelector: () => baseApi.request({ url: "/api/finReceivable/getCustomerSelector" }),
		// 应收单选择器
		selector: () => baseApi.request({ url: "/api/finReceivable/selector" }),
		// 审核应收款
		approve: (data: { id: number; approvalRemark?: string }) => baseApi.request({ url: "/api/finReceivable/approve", method: "POST", data }),
		// 拒绝应收款
		reject: (data: { id: number; rejectReason: string }) => baseApi.request({ url: "/api/finReceivable/reject", method: "POST", data }),
	}
}

// 应收账款表实体
export interface FinReceivable {
	// 主键Id
	id: number;
	// 应收单号
	receivableNo?: string;
	// 客户ID
	customerId?: number;
	// 客户编码
	customerCode?: string;
	// 客户名称
	customerName?: string;
	// 来源单据类型
	sourceType: string;
	// 来源单据ID
	sourceId: number;
	// 来源单据号
	sourceNo: string;
	// 单据日期
	billDate?: string;
	// 到期日期
	dueDate: string;
	// 应收金额
	amount?: number;
	// 已收金额
	receivedAmount?: number;
	// 未收金额
	unreceivedAmount?: number;
	// 逾期天数
	overdueDays?: number;
	// 状态
	status?: string;
	// 业务员ID
	salesmanId: number;
	// 业务员姓名
	salesmanName: string;
	// 部门ID
	departmentId: number;
	// 部门名称
	departmentName: string;
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