import {useBaseApi} from '/@/api/base';

// 采购退货明细表接口服务
export const usePurReturnItemApi = () => {
	const baseApi = useBaseApi("purReturnItem");
	return {
		// 分页查询采购退货明细表
		page: baseApi.page,
		// 查看采购退货明细表详细
		detail: baseApi.detail,
		// 新增采购退货明细表
		add: baseApi.add,
		// 更新采购退货明细表
		update: baseApi.update,
		// 删除采购退货明细表
		delete: baseApi.delete,
		// 批量删除采购退货明细表
		batchDelete: baseApi.batchDelete,
		// 导出采购退货明细表数据
		exportData: baseApi.exportData,
		// 导入采购退货明细表数据
		importData: baseApi.importData,
		// 下载采购退货明细表数据导入模板
		downloadTemplate: baseApi.downloadTemplate,
	}
}

// 采购退货明细表实体
export interface PurReturnItem {
	// 主键Id
	id: number;
	// 退货单ID
	returnId?: number;
	// 入库明细ID
	inboundItemId: number;
	// 物料ID
	materialId?: number;
	// 物料编码
	materialCode?: string;
	// 物料名称
	materialName?: string;
	// 规格型号
	spec: string;
	// 单位名称
	unitName?: string;
	// 退货数量
	returnQty?: number;
	// 单价
	price?: number;
	// 金额
	amount?: number;
	// 退货原因
	reason: string;
	// 排序
	sortOrder?: number;
}