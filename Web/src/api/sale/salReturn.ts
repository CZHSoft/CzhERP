import {useBaseApi} from '/@/api/base';

// 销售退货接口服务
export const useSalReturnApi = () => {
	const baseApi = useBaseApi("salReturn");
	return {
		// 分页查询销售退货
		page: baseApi.page,
		// 查看销售退货详细
		detail: baseApi.detail,
		// 新增销售退货
		add: baseApi.add,
		// 更新销售退货
		update: baseApi.update,
		// 删除销售退货
		delete: baseApi.delete,
		// 批量删除销售退货
		batchDelete: baseApi.batchDelete,
		// 导出销售退货数据
		exportData: baseApi.exportData,
		// 导入销售退货数据
		importData: baseApi.importData,
		// 下载销售退货数据导入模板
		downloadTemplate: baseApi.downloadTemplate,
	}
}

// 销售退货实体
export interface SalReturn {
	// 主键Id
	id: number;
	// 退货单号
	returnNo?: string;
	// 订单ID
	orderId: number;
	// 订单号
	orderNo: string;
	// 客户ID
	customerId?: number;
	// 客户名称
	customerName?: string;
	// 退货日期
	returnDate?: string;
	// 退货类型
	returnType: string;
	// 退货原因
	returnReason: string;
	// 退货总数量
	totalQuantity?: number;
	// 退货总金额
	totalAmount?: number;
	// 状态
	status?: string;
	// 审批人ID
	approvalUserId: number;
	// 审批时间
	approvalTime: string;
	// 审批备注
	approvalRemark: string;
	// 入库仓库ID
	warehouseId: number;
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