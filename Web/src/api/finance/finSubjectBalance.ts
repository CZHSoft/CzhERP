import { service } from '/@/utils/request';

// 科目余额表接口服务
export const useFinSubjectBalanceApi = () => {
	const baseApi = {
		baseUrl: '/api/finSubjectBalance/',
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
		selectorAccount: () => baseApi.request({ url: "/api/finAccount/selector" }),
	};
}

// 科目余额表实体
export interface FinSubjectBalance {
	// 主键Id
	id: number;
	// 科目ID
	accountId?: number;
	// 科目编码
	accountCode?: string;
	// 会计年度
	year?: number;
	// 会计期间
	period?: number;
	// 期初借方余额
	initialDebit?: number;
	// 期初贷方余额
	initialCredit?: number;
	// 借方本年累计
	debitYTD?: number;
	// 贷方本年累计
	creditYTD?: number;
	// 借方本期发生
	debitPeriod?: number;
	// 贷方本期发生
	creditPeriod?: number;
	// 期末借方余额
	endDebit?: number;
	// 期末贷方余额
	endCredit?: number;
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