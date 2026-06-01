import {useBaseApi} from '/@/api/base';

// 库存变动日志接口服务
export const useStoStockLogApi = () => {
	const baseApi = useBaseApi("stoStockLog");
	return {
		// 分页查询库存变动日志
		page: baseApi.page,
		// 查看库存变动日志详细
		detail: baseApi.detail,
		// 新增库存变动日志
		add: baseApi.add,
		// 更新库存变动日志
		update: baseApi.update,
		// 删除库存变动日志
		delete: baseApi.delete,
		// 批量删除库存变动日志
		batchDelete: baseApi.batchDelete,
		// 导出库存变动日志数据
		exportData: baseApi.exportData,
		// 导入库存变动日志数据
		importData: baseApi.importData,
		// 下载库存变动日志数据导入模板
		downloadTemplate: baseApi.downloadTemplate,
	}
}

// 库存变动日志实体
export interface StoStockLog {
	// 主键Id
	id: number;
	// 业务类型(StockIn入库/StockOut出库/Transfer调拨/Adjust调整/Count盘点)
	businessType?: string;
	// 业务单据号
	businessNo?: string;
	// 仓库ID
	warehouseId: number;
	// 仓库编码
	warehouseCode: string;
	// 物料ID
	materialId: number;
	// 物料编码
	materialCode: string;
	// 物料名称
	materialName: string;
	// 变动类型(Increase增加/Decrease减少)
	changeType?: string;
	// 变动数量
	changeQuantity?: number;
	// 变动前数量
	beforeQuantity?: number;
	// 变动后数量
	afterQuantity?: number;
	// 成本单价
	costPrice?: number;
	// 变动金额
	changeAmount?: number;
	// 库位编码
	locationCode: string;
	// 批号
	batchNo: string;
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