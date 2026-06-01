﻿import {useBaseApi} from '/@/api/base';
import request from '/@/utils/request';

// 报价单接口服务
export const useSalQuoteApi = () => {
	const baseApi = useBaseApi("salQuote");
	return {
		// 分页查询报价单
		page: baseApi.page,
		// 查看报价单详细
		detail: baseApi.detail,
		// 新增报价单
		add: baseApi.add,
		// 更新报价单
		update: baseApi.update,
		// 删除报价单
		delete: baseApi.delete,
		// 批量删除报价单
		batchDelete: baseApi.batchDelete,
		// 导出报价单数据
		exportData: baseApi.exportData,
		// 导入报价单数据
		importData: baseApi.importData,
		// 下载报价单数据导入模板
		downloadTemplate: baseApi.downloadTemplate,
		// 获取下一个报价单号
		getNextQuoteNo: () => request({
			url: "/api/salQuote/NextQuoteNo",
			method: "GET",
		}),
		// 获取客户列表
		getCustomerList: (params: { keyword?: string }) => request({
			url: "/api/salQuote/CustomerList",
			method: "GET",
			params: params,
		}),
		// 获取当前用户信息
		getCurrentUser: () => request({
			url: "/api/salQuote/CurrentUser",
			method: "GET",
		}),
		// 获取状态选项
		getStatusOptions: () => request({
			url: "/api/salQuote/StatusOptions",
			method: "GET",
		}),
		// 审批报价单
		approve: (data: { id: number; approvalRemark?: string }) => request({
			url: "/api/salQuote/Approve",
			method: "POST",
			data: data,
		}),
		// 拒绝审批报价单
		reject: (data: { id: number; approvalRemark?: string }) => request({
			url: "/api/salQuote/Reject",
			method: "POST",
			data: data,
		}),
		// 计算报价单明细汇总
		calculateSummary: (params: { quoteId: number }) => request({
			url: "/api/salQuote/CalculateSummary",
			method: "GET",
			params: params,
		}),
		// 检查报价单转换条件
		checkConvertCondition: (params: { quoteId: number }) => request({
			url: "/api/salQuote/CheckConvertCondition",
			method: "GET",
			params: params,
		}),
		// // 报价单转销售订单
		// convertToOrder: (data: { quoteId: number; remark?: string }) => request({
		// 	url: "/api/salQuote/ConvertToOrder",
		// 	method: "POST",
		// 	data: data,
		// }),
	}
}

// 报价单实体
export interface SalQuote {
	// 主键Id
	id: number;
	// 报价单号
	quoteNo?: string;
	// 客户ID
	customerId?: number;
	// 客户名称
	customerName?: string;
	// 报价日期
	quoteDate?: string;
	// 有效开始日期
	validStartDate?: string;
	// 有效结束日期
	validEndDate?: string;
	// 总金额
	totalAmount?: number;
	// 总税额
	totalTaxAmount?: number;
	// 状态
	status?: string;
	// 审批人ID
	approvalUserId: number;
	// 审批时间
	approvalTime: string;
	// 审批备注
	approvalRemark: string;
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