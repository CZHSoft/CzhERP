import {useBaseApi} from '/@/api/base';

// 销售退货明细接口服务
export const useSalReturnItemApi = () => {
	const baseApi = useBaseApi("salReturnItem");
	return {
		// 分页查询销售退货明细
		page: baseApi.page,
		// 查看销售退货明细详细
		detail: baseApi.detail,
		// 新增销售退货明细
		add: baseApi.add,
		// 更新销售退货明细
		update: baseApi.update,
		// 删除销售退货明细
		delete: baseApi.delete,
		// 批量删除销售退货明细
		batchDelete: baseApi.batchDelete,
		// 导出销售退货明细数据
		exportData: baseApi.exportData,
		// 导入销售退货明细数据
		importData: baseApi.importData,
		// 下载销售退货明细数据导入模板
		downloadTemplate: baseApi.downloadTemplate,
	}
}

// 销售退货明细实体
export interface SalReturnItem {
	// 主键Id
	id: number;
	// 退货单ID
	returnId?: number;
	// 出库明细ID
	outboundItemId: number;
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
	// 退货数量
	returnQuantity?: number;
	// 检验结果
	inspectResult: string;
	// 检验备注
	inspectRemark: string;
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