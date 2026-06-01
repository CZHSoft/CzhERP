import {useBaseApi} from '/@/api/base';

// 库存余额接口服务
export const useStoInventoryApi = () => {
	const baseApi = useBaseApi("stoInventory");
	return {
		// 分页查询库存余额
		page: baseApi.page,
		// 查看库存余额详细
		detail: baseApi.detail,
		// 新增库存余额
		add: baseApi.add,
		// 更新库存余额
		update: baseApi.update,
		// 删除库存余额
		delete: baseApi.delete,
		// 批量删除库存余额
		batchDelete: baseApi.batchDelete,
		// 导出库存余额数据
		exportData: baseApi.exportData,
		// 导入库存余额数据
		importData: baseApi.importData,
		// 下载库存余额数据导入模板
		downloadTemplate: baseApi.downloadTemplate,
	}
}

// 库存余额实体
export interface StoInventory {
	// 主键Id
	id: number;
	// 仓库ID
	warehouseId?: number;
	// 仓库编码
	warehouseCode?: string;
	// 仓库名称
	warehouseName: string;
	// 物料ID
	materialId?: number;
	// 物料编码
	materialCode?: string;
	// 物料名称
	materialName: string;
	// 规格型号
	spec: string;
	// 单位
	unit?: string;
	// 库存数量
	stockQuantity?: number;
	// 冻结数量
	frozenQuantity?: number;
	// 可用数量
	availableQuantity?: number;
	// 成本单价
	costPrice?: number;
	// 库存总成本
	totalCost?: number;
	// 最低库存
	minStock?: number;
	// 最高库存
	maxStock?: number;
	// 更新时间
	updateTime: string;
	// 创建时间
	createTime: string;
	// 创建者Id
	createUserId: number;
	// 创建者姓名
	createUserName: string;
	// 修改者Id
	updateUserId: number;
	// 修改者姓名
	updateUserName: string;
}