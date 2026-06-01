import {useBaseApi} from '/@/api/base';

// 调拨单明细表接口服务
export const useStoTransferItemApi = () => {
	const baseApi = useBaseApi("stoTransferItem");
	return {
		// 分页查询调拨单明细表
		page: baseApi.page,
		// 查看调拨单明细表详细
		detail: baseApi.detail,
		// 新增调拨单明细表
		add: baseApi.add,
		// 更新调拨单明细表
		update: baseApi.update,
		// 删除调拨单明细表
		delete: baseApi.delete,
		// 批量删除调拨单明细表
		batchDelete: baseApi.batchDelete,
		// 导出调拨单明细表数据
		exportData: baseApi.exportData,
		// 导入调拨单明细表数据
		importData: baseApi.importData,
		// 下载调拨单明细表数据导入模板
		downloadTemplate: baseApi.downloadTemplate,
	}
}

// 调拨单明细表实体
export interface StoTransferItem {
	// 主键Id
	id: number;
	// 调拨单ID
	transferId?: number;
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
	// 调拨数量
	quantity?: number;
	// 转出库位
	fromLocationCode: string;
	// 转入库位
	toLocationCode: string;
	// 批号
	batchNo: string;
	// 排序号
	sortOrder?: number;
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