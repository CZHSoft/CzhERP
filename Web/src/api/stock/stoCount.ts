import {useBaseApi} from '/@/api/base';
import request from '/@/utils/request';

// 盘点单主表接口服务
export const useStoCountApi = () => {
	const baseApi = useBaseApi("stoCount");
	return {
		// 分页查询盘点单主表
		page: baseApi.page,
		// 查看盘点单主表详细
		detail: baseApi.detail,
		// 新增盘点单主表
		add: baseApi.add,
		// 更新盘点单主表
		update: baseApi.update,
		// 删除盘点单主表
		delete: baseApi.delete,
		// 批量删除盘点单主表
		batchDelete: baseApi.batchDelete,
		// 导出盘点单主表数据
		exportData: baseApi.exportData,
		// 导入盘点单主表数据
		importData: baseApi.importData,
		// 下载盘点单主表数据导入模板
		downloadTemplate: baseApi.downloadTemplate,
		// 获取下一个盘点单号
		getNextCode: () => request.get<string>("/api/stoCount/NextCode"),
		// 审核盘点单
		approve: (data: { id: number }) => request.post('/api/stoCount/approve', data),
		// 拒绝盘点单
		reject: (data: { id: number; reason: string }) => request.post('/api/stoCount/reject', data),
	}
}

// 获取盘点单列表（用于下拉选择）
export const getCountList = (params?: any) => {
	return request.post("/api/stoCount/page", { page: 1, pageSize: 100, ...params });
}

// 盘点单主表实体
export interface StoCount {
	// 主键Id
	id: number;
	// 盘点单号
	countNo?: string;
	// 仓库ID
	warehouseId?: number;
	// 仓库编码
	warehouseCode?: string;
	// 仓库名称
	warehouseName: string;
	// 盘点日期
	countDate?: string;
	// 状态(Draft草稿/Counting盘点中/Completed已完成)
	status?: string;
	// 差异数量
	diffQuantity?: number;
	// 差异金额
	diffAmount?: number;
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