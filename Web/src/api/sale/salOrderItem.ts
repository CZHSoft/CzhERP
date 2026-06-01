import {useBaseApi} from '/@/api/base';

// 销售订单明细接口服务
export const useSalOrderItemApi = () => {
	const baseApi = useBaseApi("salOrderItem");
	return {
		// 分页查询销售订单明细
		page: baseApi.page,
		// 查看销售订单明细详细
		detail: baseApi.detail,
		// 新增销售订单明细
		add: baseApi.add,
		// 更新销售订单明细
		update: baseApi.update,
		// 删除销售订单明细
		delete: baseApi.delete,
		// 批量删除销售订单明细
		batchDelete: baseApi.batchDelete,
		// 导出销售订单明细数据
		exportData: baseApi.exportData,
		// 导入销售订单明细数据
		importData: baseApi.importData,
		// 下载销售订单明细数据导入模板
		downloadTemplate: baseApi.downloadTemplate,
	}
}

// 销售订单明细实体
export interface SalOrderItem {
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
	// 单位
	unit?: string;
	// 数量
	quantity?: number;
	// 已发货数量
	deliveryQuantity?: number;
	// 单价
	unitPrice?: number;
	// 税率
	taxRate?: number;
	// 税额
	taxAmount?: number;
	// 金额
	amount?: number;
	// 折扣
	discount?: number;
	// 仓库ID
	warehouseId: number;
	// 批次号
	batchNo: string;
	// 排序
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