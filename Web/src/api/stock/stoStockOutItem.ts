import {useBaseApi} from '/@/api/base';

// 出库单明细表接口服务
export const useStoStockOutItemApi = () => {
	const baseApi = useBaseApi("stoStockOutItem");
	return {
		// 分页查询出库单明细表
		page: baseApi.page,
		// 查看出库单明细表详细
		detail: baseApi.detail,
		// 新增出库单明细表
		add: baseApi.add,
		// 更新出库单明细表
		update: baseApi.update,
		// 删除出库单明细表
		delete: baseApi.delete,
		// 批量删除出库单明细表
		batchDelete: baseApi.batchDelete,
		// 导出出库单明细表数据
		exportData: baseApi.exportData,
		// 导入出库单明细表数据
		importData: baseApi.importData,
		// 下载出库单明细表数据导入模板
		downloadTemplate: baseApi.downloadTemplate,
	}
}

// 出库单明细表实体
export interface StoStockOutItem {
	// 主键Id
	id: number;
	// 出库单ID
	stockOutId?: number;
	// 物料ID
	materialId?: number;
	// 物料编码
	materialCode?: string;
	// 物料名称
	materialName?: string;
	// 规格型号
	spec: string;
	// 单位
	unit?: string;
	// 出库数量
	quantity?: number;
	// 单价
	unitPrice?: number;
	// 金额
	amount?: number;
	// 出库库位
	locationCode: string;
	// 批号
	batchNo: string;
	// 排序号
	sortOrder?: number;
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