import {useBaseApi} from '/@/api/base';
import request from '/@/utils/request';

export const useStoAdjustApi = () => {
	const baseApi = useBaseApi("stoAdjust");
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
		getNextCode: () => request.get<string>("/api/stoAdjust/NextCode"),
		approve: (data: ApproveAdjustInput) => request.post('/api/stoAdjust/approve', data),
		reject: (data: RejectAdjustInput) => request.post('/api/stoAdjust/reject', data),
	}
}

export const getAdjustList = (params?: any) => {
	return request({
		url: "/api/stoAdjust/page",
		method: "POST",
		data: {
			page: 1,
			pageSize: 100,
			...params,
		},
	});
};

export interface ApproveAdjustInput {
	id: number;
	approvalUserId?: number;
	approvalRemark?: string;
}

export interface RejectAdjustInput {
	id: number;
	approvalUserId?: number;
	rejectReason: string;
}

export interface StoAdjust {
	id: number;
	adjustNo?: string;
	adjustType?: string;
	warehouseId?: number;
	warehouseCode?: string;
	warehouseName: string;
	sourceBillNo: string;
	adjustDate?: string;
	adjustReason: string;
	totalQuantity?: number;
	totalAmount?: number;
	status?: string;
	approvalUserId: number;
	approvalTime: string;
	approvalRemark: string;
	remark: string;
	createTime: string;
	updateTime: string;
	createUserId: number;
	createUserName: string;
	updateUserId: number;
	updateUserName: string;
}