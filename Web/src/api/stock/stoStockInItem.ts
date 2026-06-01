import {useBaseApi} from '/@/api/base';
import request from '/@/utils/request';

// 入库单明细表接口服务
export const useStoStockInItemApi = () => {
	const baseApi = useBaseApi("stoStockInItem");
	return {
		// 分页查询入库单明细表
		page: baseApi.page,
		// 查看入库单明细表详细
		detail: baseApi.detail,
		// 新增入库单明细表
		add: baseApi.add,
		// 更新入库单明细表
		update: baseApi.update,
		// 删除入库单明细表
		delete: baseApi.delete,
		// 批量删除入库单明细表
		batchDelete: baseApi.batchDelete,
		// 导出入库单明细表数据
		exportData: baseApi.exportData,
		// 导入入库单明细表数据
		importData: baseApi.importData,
		// 下载入库单明细表数据导入模板
		downloadTemplate: baseApi.downloadTemplate,
	}
}

// 获取入库单列表接口（用于下拉选择）
export const getStockInList = () => {
	return request.get<any[]>("/api/stoStockIn/List");
}

// 获取物料列表接口（用于下拉选择）
export const getMaterialList = () => {
	return request.get<any[]>("/api/basMaterial/List");
}

// 获取库位列表接口（用于下拉选择）
export const getLocationList = (warehouseId?: number) => {
	if (warehouseId) {
		return request.get<any[]>(`/api/stoLocation/ListByWarehouse/${warehouseId}`);
	}
	return request.get<any[]>("/api/stoLocation/List");
}

// 入库单明细表实体
export interface StoStockInItem {
	// 主键Id
	id: number;
	// 入库单ID
	stockInId?: number;
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
	// 入库数量
	quantity?: number;
	// 单价
	unitPrice?: number;
	// 金额
	amount?: number;
	// 入库库位
	locationCode: string;
	// 批号
	batchNo: string;
	// 有效期
	expiryDate: string;
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