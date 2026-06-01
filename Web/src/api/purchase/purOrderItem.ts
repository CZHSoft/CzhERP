import {useBaseApi} from '/@/api/base';

// 采购订单明细表接口服务
export const usePurOrderItemApi = () => {
	const baseApi = useBaseApi("purOrderItem");
	return {
		// 分页查询采购订单明细表
		page: baseApi.page,
		// 查看采购订单明细表详细
		detail: baseApi.detail,
		// 新增采购订单明细表
		add: baseApi.add,
		// 更新采购订单明细表
		update: baseApi.update,
		// 删除采购订单明细表
		delete: baseApi.delete,
		// 批量删除采购订单明细表
		batchDelete: baseApi.batchDelete,
		// 导出采购订单明细表数据
		exportData: baseApi.exportData,
		// 导入采购订单明细表数据
		importData: baseApi.importData,
		// 下载采购订单明细表数据导入模板
		downloadTemplate: baseApi.downloadTemplate,
	}
}

// 采购订单明细表实体
export interface PurOrderItem {
	// 主键Id
	id: number;
	// 订单ID
	orderId?: number;
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
	// 订单数量
	orderQty?: number;
	// 单价(不含税)
	price?: number;
	// 金额(不含税)
	amount?: number;
	// 税率
	taxRate?: number;
	// 税额
	taxAmount?: number;
	// 交货日期
	deliveryDate: string;
	// 已收货数量
	receivedQty?: number;
	// 备注
	remark: string;
	// 排序
	sortOrder?: number;
}