import {useBaseApi} from '/@/api/base';

// 销售出库明细接口服务
export const useSalOutboundItemApi = () => {
	const baseApi = useBaseApi("salOutboundItem");
	return {
		// 分页查询销售出库明细
		page: baseApi.page,
		// 查看销售出库明细详细
		detail: baseApi.detail,
		// 新增销售出库明细
		add: baseApi.add,
		// 更新销售出库明细
		update: baseApi.update,
		// 删除销售出库明细
		delete: baseApi.delete,
		// 批量删除销售出库明细
		batchDelete: baseApi.batchDelete,
		// 导出销售出库明细数据
		exportData: baseApi.exportData,
		// 导入销售出库明细数据
		importData: baseApi.importData,
		// 下载销售出库明细数据导入模板
		downloadTemplate: baseApi.downloadTemplate,
	}
}

// 销售出库明细实体
export interface SalOutboundItem {
	// 主键Id
	id: number;
	// 出库单ID
	outboundId?: number;
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
	// 单位
	unit?: string;
	// 订单数量
	orderQuantity?: number;
	// 出库数量
	outboundQuantity?: number;
	// 库位编码
	locationCode: string;
	// 批次号
	batchNo: string;
	// 有效期
	expiryDate: string;
	// 单位成本
	unitCost?: number;
	// 金额
	amount?: number;
	// 排序
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