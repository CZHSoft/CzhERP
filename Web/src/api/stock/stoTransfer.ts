import {useBaseApi} from '/@/api/base';
import request from '/@/utils/request';

// 调拨单主表接口服务
export const useStoTransferApi = () => {
	const baseApi = useBaseApi("stoTransfer");
	return {
		// 分页查询调拨单主表
		page: baseApi.page,
		// 查看调拨单主表详细
		detail: baseApi.detail,
		// 新增调拨单主表
		add: baseApi.add,
		// 更新调拨单主表
		update: baseApi.update,
		// 删除调拨单主表
		delete: baseApi.delete,
		// 批量删除调拨单主表
		batchDelete: baseApi.batchDelete,
		// 导出调拨单主表数据
		exportData: baseApi.exportData,
		// 导入调拨单主表数据
		importData: baseApi.importData,
		// 下载调拨单主表数据导入模板
		downloadTemplate: baseApi.downloadTemplate,
		// 获取下一个调拨单号
		getNextCode: () => request.get<string>("/api/stoTransfer/NextCode"),
		// 审核调拨单
		approve: (data: ApproveTransferInput) => request.post('/api/stoTransfer/approve', data),
		// 拒绝调拨单
		reject: (data: RejectTransferInput) => request.post('/api/stoTransfer/reject', data),
	}
}

// 获取调拨单列表（用于下拉选择）
export const getTransferList = (params?: any) => {
	return request({
		url: "/api/stoTransfer/page",
		method: "POST",
		data: {
			page: 1,
			pageSize: 100,
			...params,
		},
	});
};

// 审核调拨单输入
export interface ApproveTransferInput {
	id: number;
	approvalUserId?: number;
	approvalRemark?: string;
}

// 拒绝调拨单输入
export interface RejectTransferInput {
	id: number;
	approvalUserId?: number;
	rejectReason: string;
}

// 调拨单主表实体
export interface StoTransfer {
	// 主键Id
	id: number;
	// 调拨单号
	transferNo?: string;
	// 转出仓库ID
	fromWarehouseId?: number;
	// 转出仓库编码
	fromWarehouseCode?: string;
	// 转出仓库名称
	fromWarehouseName: string;
	// 转入仓库ID
	toWarehouseId?: number;
	// 转入仓库编码
	toWarehouseCode?: string;
	// 转入仓库名称
	toWarehouseName: string;
	// 调拨日期
	transferDate?: string;
	// 调拨总数量
	totalQuantity?: number;
	// 状态(Draft草稿/Approved已审批/Completed已完成/Cancelled已取消)
	status?: string;
	// 审批人ID
	approvalUserId: number;
	// 审批时间
	approvalTime: string;
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