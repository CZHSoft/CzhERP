import { service } from '/@/utils/request';

// 凭证主表接口服务
export const useFinVoucherApi = () => {
	const baseApi = {
		baseUrl: '/api/finVoucher/',
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
		selector: () => baseApi.request({ url: "/api/finVoucher/selector" }),
		getStatuses: () => baseApi.request({ url: "/api/finVoucher/getStatuses" }),
		getNewVoucherNo: () => baseApi.request({ url: "/api/finVoucher/getNewVoucherNo" }),
	};
}

// 凭证主表实体
export interface FinVoucher {
	// 主键Id
	id: number;
	// 凭证号
	voucherNo?: string;
	// 凭证字
	voucherWord: string;
	// 凭证日期
	voucherDate?: string;
	// 会计年度
	year?: number;
	// 会计期间
	period?: number;
	// 附件数量
	attachmentCount?: number;
	// 来源单据类型
	sourceType: string;
	// 来源单据ID
	sourceId: number;
	// 来源单据号
	sourceNo: string;
	// 借方金额合计
	totalDebit?: number;
	// 贷方金额合计
	totalCredit?: number;
	// 状态
	status?: string;
	// 状态（显示用）
	statusText?: string;
	// 制单人ID
	makerId: number;
	// 制单人姓名
	makerName: string;
	// 制单时间
	makeTime: string;
	// 审核人ID
	approverId: number;
	// 审核人姓名
	approverName: string;
	// 审核时间
	approveTime: string;
	// 过账人ID
	posterId: number;
	// 过账人姓名
	posterName: string;
	// 过账时间
	postTime: string;
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

// 凭证下拉选择实体
export interface FinVoucherSelector {
	id: number;
	voucherNo: string;
	voucherDate: string;
	remark?: string;
}
