import {useBaseApi} from '/@/api/base';

// 采购入库明细表接口服务
export const usePurInboundItemApi = () => {
	const baseApi = useBaseApi("purInboundItem");
	return {
		// 分页查询采购入库明细表
		page: baseApi.page,
		// 查看采购入库明细表详细
		detail: baseApi.detail,
		// 新增采购入库明细表
		add: baseApi.add,
		// 更新采购入库明细表
		update: baseApi.update,
		// 删除采购入库明细表
		delete: baseApi.delete,
		// 批量删除采购入库明细表
		batchDelete: baseApi.batchDelete,
		// 导出采购入库明细表数据
		exportData: baseApi.exportData,
		// 导入采购入库明细表数据
		importData: baseApi.importData,
		// 下载采购入库明细表数据导入模板
		downloadTemplate: baseApi.downloadTemplate,
	}
}

// 采购入库明细表实体
export interface PurInboundItem {
	// 主键Id
	id: number;
	// 入库单ID
	inboundId?: number;
	// 订单明细ID
	orderItemId: number;
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
	// 实收数量
	receivedQty?: number;
	// 合格数量
	qualifiedQty?: number;
	// 不合格数量
	defectiveQty?: number;
	// 单价
	price?: number;
	// 金额
	amount?: number;
	// 库位ID
	locationId: number;
	// 批次号
	batchNo: string;
	// 有效期
	expiryDate: string;
	// 备注
	remark: string;
	// 排序
	sortOrder?: number;
}