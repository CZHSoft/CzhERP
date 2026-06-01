import {useBaseApi} from '/@/api/base';

// 应付账款表接口服务
export const useFinPayableApi = () => {
	const baseApi = useBaseApi("finPayable");
	return {
		// 分页查询应付账款表
		page: baseApi.page,
		// 查看应付账款表详细
		detail: baseApi.detail,
		// 新增应付账款表
		add: baseApi.add,
		// 更新应付账款表
		update: baseApi.update,
		// 删除应付账款表
		delete: baseApi.delete,
		// 批量删除应付账款表
		batchDelete: baseApi.batchDelete,
		// 导出应付账款表数据
		exportData: baseApi.exportData,
		// 导入应付账款表数据
		importData: baseApi.importData,
		// 下载应付账款表数据导入模板
		downloadTemplate: baseApi.downloadTemplate,
		// 获取状态字典
		getStatuses: () => baseApi.request({ url: "/api/finPayable/getStatuses" }),
		// 获取新应付单号
		getNewPayableNo: () => baseApi.request({ url: "/api/finPayable/getNewPayableNo" }),
		// 获取供应商选择器
		getSupplierSelector: () => baseApi.request({ url: "/api/finPayable/getSupplierSelector" }),
		// 应付单选择器
		selector: () => baseApi.request({ url: "/api/finPayable/selector" }),
		// 审核应付款
		approve: (data: { id: number; approvalRemark?: string }) => baseApi.request({ url: "/api/finPayable/approve", method: "POST", data }),
		// 拒绝应付款
		reject: (data: { id: number; rejectReason: string }) => baseApi.request({ url: "/api/finPayable/reject", method: "POST", data }),
	}
}

// 应付账款表实体
export interface FinPayable {
	// 主键Id
	id: number;
	// 应付单号
	payableNo?: string;
	// 供应商ID
	supplierId?: number;
	// 供应商编码
	supplierCode?: string;
	// 供应商名称
	supplierName?: string;
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
	// 应付金额
	amount?: number;
	// 已付金额
	paidAmount?: number;
	// 未付金额
	unpaidAmount?: number;
	// 逾期天数
	overdueDays?: number;
	// 状态
	status?: string;
	// 采购员ID
	purchaserId: number;
	// 采购员姓名
	purchaserName: string;
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