import {useBaseApi} from '/@/api/base';
import request from '/@/utils/request';

// 销售订单接口服务
export const useSalOrderApi = () => {
	const baseApi = useBaseApi("salOrder");
	return {
		// 分页查询销售订单
		page: baseApi.page,
		// 查看销售订单详细
		detail: baseApi.detail,
		// 新增销售订单
		add: baseApi.add,
		// 更新销售订单
		update: baseApi.update,
		// 删除销售订单
		delete: baseApi.delete,
		// 批量删除销售订单
		batchDelete: baseApi.batchDelete,
		// 导出销售订单数据
		exportData: baseApi.exportData,
		// 导入销售订单数据
		importData: baseApi.importData,
		// 下载销售订单数据导入模板
		downloadTemplate: baseApi.downloadTemplate,
		// 获取订单状态选项
		getStatusOptions: () => request({
			url: "/api/salOrder/StatusOptions",
			method: "GET",
		}),
		// 订单审核
		approve: (data: { id: number; isApproved: boolean; approvalRemark?: string; approvalUserId?: number }) => request({
			url: "/api/salOrder/Approve",
			method: "POST",
			data: data,
		}),
		// 计算订单汇总
		calculateSummary: (params: { orderId: number }) => request({
			url: "/api/salOrder/CalculateSummary",
			method: "GET",
			params: params,
		}),
	}
}

// 获取销售订单列表（用于下拉选择）
export const getOrderList = (params?: any) => {
	return request({
		url: "/api/salOrder/page",
		method: "POST",
		data: {
			page: 1,
			pageSize: 100,
			...params,
		},
	});
};

// 销售订单实体
export interface SalOrder {
	// 主键Id
	id: number;
	// 订单号
	orderNo?: string;
	// 客户ID
	customerId?: number;
	// 客户名称
	customerName?: string;
	// 联系人姓名
	contactName?: string;
	// 联系人电话
	contactPhone?: string;
	// 订单日期
	orderDate?: string;
	// 预计交货日期
	deliveryDate?: string;
	// 收货地址
	address?: string;
	// 配送方式
	shippingMethod?: string;
	// 运费
	shippingFee?: number;
	// 订单总金额
	totalAmount?: number;
	// 税额
	totalTaxAmount?: number;
	// 折扣金额
	totalDiscount?: number;
	// 应付金额
	payAmount?: number;
	// 付款方式
	paymentType?: string;
	// 状态
	status?: string;
	// 审核结果
	creditCheckResult?: string;
	// 信用使用金额
	creditUsedAmount?: number;
	// 审批人ID
	approvalUserId?: number;
	// 审批时间
	approvalTime?: string;
	// 审批意见
	approvalRemark?: string;
	// 备注
	remark?: string;
	// 创建时间
	createTime?: string;
	// 更新时间
	updateTime?: string;
	// 创建者Id
	createUserId?: number;
	// 创建者姓名
	createUserName?: string;
	// 修改者Id
	updateUserId?: number;
	// 修改者姓名
	updateUserName?: string;
	// 报价单ID
	quoteId?: number;
}
