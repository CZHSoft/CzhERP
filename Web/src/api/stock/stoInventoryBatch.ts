import {useBaseApi} from '/@/api/base';

// 批次库存接口服务
export const useStoInventoryBatchApi = () => {
	const baseApi = useBaseApi("stoInventoryBatch");
	return {
		// 分页查询批次库存
		page: baseApi.page,
		// 查看批次库存详细
		detail: baseApi.detail,
		// 新增批次库存
		add: baseApi.add,
		// 更新批次库存
		update: baseApi.update,
		// 删除批次库存
		delete: baseApi.delete,
		// 批量删除批次库存
		batchDelete: baseApi.batchDelete,
		// 导出批次库存数据
		exportData: baseApi.exportData,
		// 导入批次库存数据
		importData: baseApi.importData,
		// 下载批次库存数据导入模板
		downloadTemplate: baseApi.downloadTemplate,
	}
}

// 批次库存实体
export interface StoInventoryBatch {
	// 主键Id
	id: number;
	// 仓库ID
	warehouseId?: number;
	// 仓库编码
	warehouseCode?: string;
	// 库位编码
	locationCode: string;
	// 物料ID
	materialId?: number;
	// 物料编码
	materialCode?: string;
	// 批号
	batchNo?: string;
	// 有效期
	expiryDate: string;
	// 批次库存数量
	stockQuantity?: number;
	// 冻结数量
	frozenQuantity?: number;
	// 批次成本价
	costPrice?: number;
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