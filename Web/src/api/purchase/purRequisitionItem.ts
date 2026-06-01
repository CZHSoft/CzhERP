import {useBaseApi} from '/@/api/base';

// 采购申请明细表接口服务
export const usePurRequisitionItemApi = () => {
	const baseApi = useBaseApi("purRequisitionItem");
	return {
		// 分页查询采购申请明细表
		page: baseApi.page,
		// 查看采购申请明细表详细
		detail: baseApi.detail,
		// 新增采购申请明细表
		add: baseApi.add,
		// 更新采购申请明细表
		update: baseApi.update,
		// 删除采购申请明细表
		delete: baseApi.delete,
		// 批量删除采购申请明细表
		batchDelete: baseApi.batchDelete,
		// 导出采购申请明细表数据
		exportData: baseApi.exportData,
		// 导入采购申请明细表数据
		importData: baseApi.importData,
		// 下载采购申请明细表数据导入模板
		downloadTemplate: baseApi.downloadTemplate,
	}
}

// 采购申请明细表实体
export interface PurRequisitionItem {
	// 主键Id
	id: number;
	// 申请单ID
	requisitionId?: number;
	// 物料ID
	materialId?: number;
	// 物料编码
	materialCode?: string;
	// 物料名称
	materialName?: string;
	// 规格型号
	spec: string;
	// 单位ID
	unitId: number;
	// 单位名称
	unitName?: string;
	// 申请数量
	requestQty?: number;
	// 期望单价
	expectedPrice: number;
	// 金额
	amount?: number;
	// 备注
	remark: string;
	// 排序
	sortOrder?: number;
}