import {useBaseApi} from '/@/api/base';
import request from '/@/utils/request';

// 库位档案接口服务
export const useStoLocationApi = () => {
	const baseApi = useBaseApi("stoLocation");
	return {
		// 分页查询库位档案
		page: baseApi.page,
		// 查看库位档案详细
		detail: baseApi.detail,
		// 新增库位档案
		add: baseApi.add,
		// 更新库位档案
		update: baseApi.update,
		// 删除库位档案
		delete: baseApi.delete,
		// 批量删除库位档案
		batchDelete: baseApi.batchDelete,
		// 导出库位档案数据
		exportData: baseApi.exportData,
		// 导入库位档案数据
		importData: baseApi.importData,
		// 下载库位档案数据导入模板
		downloadTemplate: baseApi.downloadTemplate,
		// 获取下一个库位编码
		getNextCode: () => request.get<string>("/api/stoLocation/NextCode"),
	}
}

// 获取仓库列表接口
export const getWarehouseList = () => {
	return request.get<any[]>("/api/stoWarehouse/List");
}

// 库位档案实体
export interface StoLocation {
	// 主键Id
	id: number;
	// 仓库ID
	warehouseId?: number;
	// 仓库编码
	warehouseCode?: string;
	// 库位编码
	locationCode?: string;
	// 库位名称
	locationName: string;
	// 库位容量
	capacity?: number;
	// 是否启用
	isEnabled?: number;
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