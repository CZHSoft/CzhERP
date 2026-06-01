import { service } from '/@/utils/request';

// 凭证分录表接口服务
export const useFinVoucherDetailApi = () => {
	const baseApi = {
		baseUrl: '/api/finVoucherDetail/',
		request: <T>(config: any) => service(config),
		page: function (data: any) {
			return baseApi.request({ url: baseApi.baseUrl + "page", method: 'post', data });
		},
		detail: function (id: any) {
			return baseApi.request({ url: baseApi.baseUrl + "detail", method: 'get', params: { id } });
		},
		add: function (data: any) {
			return baseApi.request({ url: baseApi.baseUrl + 'add', method: 'post', data });
		},
		update: function (data: any) {
			return baseApi.request({ url: baseApi.baseUrl + 'update', method: 'post', data });
		},
		delete: function (data: any) {
			return baseApi.request({ url: baseApi.baseUrl + 'delete', method: 'post', data });
		},
		batchDelete: function (data: any) {
			return baseApi.request({ url: baseApi.baseUrl + 'batchDelete', method: 'post', data });
		},
		exportData: function (data: any) {
			return baseApi.request({ responseType: 'arraybuffer', url: baseApi.baseUrl + 'export', method: 'post', data });
		},
		importData: function (file: any) {
			const formData = new FormData();
			formData.append('file', file);
			return baseApi.request({ headers: { 'Content-Type': 'multipart/form-data;charset=UTF-8' }, responseType: 'arraybuffer', url: baseApi.baseUrl + 'import', method: 'post', data: formData });
		},
		downloadTemplate: function () {
			return baseApi.request({ responseType: 'arraybuffer', url: baseApi.baseUrl + 'import', method: 'get' });
		},
	};

	return {
		page: baseApi.page,
		detail: baseApi.detail,
		add: baseApi.add,
		update: baseApi.update,
		delete: baseApi.delete,
		batchDelete: baseApi.batchDelete,
		exportData: baseApi.exportData,
		importData: baseApi.importData,
		downloadTemplate: baseApi.downloadTemplate,
		selectorVoucher: () => baseApi.request({ url: "/api/finVoucher/selector" }),
		selectorAccount: () => baseApi.request({ url: "/api/finAccount/selector" }),
	};
}

// 凭证分录表实体
export interface FinVoucherDetail {
	// 主键Id
	id: number;
	// 凭证ID
	voucherId?: number;
	// 科目ID
	accountId?: number;
	// 科目编码
	accountCode?: string;
	// 科目名称
	accountName?: string;
	// 摘要
	summary: string;
	// 借方金额
	debitAmount?: number;
	// 贷方金额
	creditAmount?: number;
	// 部门ID
	deptId: number;
	// 部门名称
	deptName: string;
	// 个人ID
	personId: number;
	// 个人姓名
	personName: string;
	// 供应商ID
	supplierId: number;
	// 供应商名称
	supplierName: string;
	// 客户ID
	customerId: number;
	// 客户名称
	customerName: string;
	// 项目ID
	projectId: number;
	// 项目名称
	projectName: string;
	// 存货ID
	inventoryId: number;
	// 存货名称
	inventoryName: string;
	// 现金流量编码
	cashFlowCode: string;
	// 现金流量名称
	cashFlowName: string;
	// 排序号
	sortOrder?: number;
	// 备注
	remark: string;
}