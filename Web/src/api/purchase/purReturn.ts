import {useBaseApi} from '/@/api/base';

// 采购退货单主表接口服务
export const usePurReturnApi = () => {
	const baseApi = useBaseApi("purReturn");
	return {
		// 分页查询采购退货单主表
		page: baseApi.page,
		// 查看采购退货单主表详细
		detail: baseApi.detail,
		// 新增采购退货单主表
		add: baseApi.add,
		// 更新采购退货单主表
		update: baseApi.update,
		// 删除采购退货单主表
		delete: baseApi.delete,
		// 批量删除采购退货单主表
		batchDelete: baseApi.batchDelete,
		// 导出采购退货单主表数据
		exportData: baseApi.exportData,
		// 导入采购退货单主表数据
		importData: baseApi.importData,
		// 下载采购退货单主表数据导入模板
		downloadTemplate: baseApi.downloadTemplate,
	}
}

// 采购退货单主表实体
export interface PurReturn {
	// 主键Id
	id: number;
	// 退货单号
	returnNo?: string;
	// 关联入库单ID
	inboundId?: number;
	// 供应商ID
	supplierId?: number;
	// 供应商名称
	supplierName?: string;
	// 退货日期
	returnDate?: string;
	// 总数量
	totalQty?: number;
	// 总金额
	totalAmount?: number;
	// 退货原因
	reason: string;
	// 状态(0待审批/1已审批/2已出库/3已完成)
	status?: number;
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