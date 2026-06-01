
import {useBaseApi} from '/@/api/base';
import request from '/@/utils/request';

// 采购订单主表接口服务
export const usePurOrderApi = () => {
	const baseApi = useBaseApi("purOrder");
	return {
		// 分页查询采购订单主表
		page: baseApi.page,
		// 查看采购订单主表详细
		detail: baseApi.detail,
		// 新增采购订单主表
		add: baseApi.add,
		// 更新采购订单主表
		update: baseApi.update,
		// 删除采购订单主表
		delete: baseApi.delete,
		// 批量删除采购订单主表
		batchDelete: baseApi.batchDelete,
		// 导出采购订单主表数据
		exportData: baseApi.exportData,
		// 导入采购订单主表数据
		importData: baseApi.importData,
		// 下载采购订单主表数据导入模板
		downloadTemplate: baseApi.downloadTemplate,
		// 获取下一个订单号
		getNextCode: () => {
			return request({
				url: '/api/purOrder/nextCode',
				method: 'get',
			});
		},
		// 审核采购订单
		approve: (data: { id: number; approvalRemark?: string; approvalUserId?: number }) => {
			return request({
				url: '/api/purOrder/approve',
				method: 'post',
				data,
			});
		},
		// 拒绝采购订单
		reject: (data: { id: number; rejectReason: string; approvalUserId?: number }) => {
			return request({
				url: '/api/purOrder/reject',
				method: 'post',
				data,
			});
		},
		// 计算采购订单明细汇总
		calculateSummary: (params: { orderId: number }) => {
			return request({
				url: '/api/purOrder/calculateSummary',
				method: 'get',
				params,
			});
		},
		// 获取采购订单列表（用于下拉选择）
		list: (params?: any) => {
			return request({
				url: '/api/purOrder/list',
				method: 'get',
				params,
			});
		},
	}
}

// 采购订单主表实体
export interface PurOrder {
	// 主键Id
	id: number;
	// 订单号
	orderNo?: string;
	// 供应商ID
	supplierId?: number;
	// 供应商编码
	supplierCode?: string;
	// 供应商名称
	supplierName?: string;
	// 来源申请单ID
	requisitionId: number;
	// 合同编号
	contractNo: string;
	// 下单日期
	orderDate?: string;
	// 交货日期
	deliveryDate: string;
	// 付款条款
	paymentTerms: string;
	// 运输方式
	shippingMethod: string;
	// 总数量
	totalQty?: number;
	// 总金额(不含税)
	totalAmount?: number;
	// 税额
	taxAmount?: number;
	// 价税合计
	grandTotal?: number;
	// 状态(0草稿/1审批中/2已确认/3已发货/4已入库/5已完成/6已取消)
	status?: number;
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

