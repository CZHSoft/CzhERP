import { service } from '/@/utils/request';

// 会计科目表接口服务
export const useFinAccountApi = () => {
	const baseApi = {
		baseUrl: '/api/finAccount/',
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
		selector: () => baseApi.request({ url: "/api/finAccount/selector" }),
		getAccountTypes: () => baseApi.request({ url: "/api/finAccount/getAccountTypes" }),
		getDirections: () => baseApi.request({ url: "/api/finAccount/getDirections" }),
		getTree: () => baseApi.request({ url: "/api/finAccount/getTree" }),
	};
}

// 会计科目表实体
export interface FinAccount {
	// 主键Id
	id: number;
	// 科目编码
	accountCode?: string;
	// 科目名称
	accountName?: string;
	// 科目全称
	fullName: string;
	// 上级科目ID
	parentId: number;
	// 上级科目名称
	parentAccountName?: string;
	// 科目级次
	level?: number;
	// 科目类型
	accountType: string;
	// 科目类型（显示用）
	accountTypeText?: string;
	// 余额方向
	direction: string;
	// 余额方向（显示用）
	directionText?: string;
	// 是否明细科目
	isDetail?: boolean;
	// 是否明细科目（显示用）
	isDetailText?: string;
	// 是否辅助核算
	isAuxiliary?: boolean;
	// 是否辅助核算（显示用）
	isAuxiliaryText?: string;
	// 是否现金流量科目
	isCashFlow?: boolean;
	// 是否现金流量科目（显示用）
	isCashFlowText?: string;
	// 部门辅助核算
	auxDept?: boolean;
	// 部门辅助核算（显示用）
	auxDeptText?: string;
	// 个人辅助核算
	auxPerson?: boolean;
	// 个人辅助核算（显示用）
	auxPersonText?: string;
	// 项目辅助核算
	auxProject?: boolean;
	// 项目辅助核算（显示用）
	auxProjectText?: string;
	// 供应商辅助核算
	auxSupplier?: boolean;
	// 供应商辅助核算（显示用）
	auxSupplierText?: string;
	// 客户辅助核算
	auxCustomer?: boolean;
	// 客户辅助核算（显示用）
	auxCustomerText?: string;
	// 存货辅助核算
	auxInventory?: boolean;
	// 存货辅助核算（显示用）
	auxInventoryText?: string;
	// 是否启用
	isEnabled?: boolean;
	// 是否启用（显示用）
	isEnabledText?: string;
	// 排序号
	sortOrder?: number;
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

// 科目下拉选择实体
export interface FinAccountSelector {
	id: number;
	accountCode: string;
	accountName: string;
	level: number;
	isDetail: boolean;
}
