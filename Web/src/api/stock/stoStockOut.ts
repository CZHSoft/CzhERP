import {useBaseApi} from '/@/api/base';
import request from '/@/utils/request';

// 出库单主表接口服务
export const useStoStockOutApi = () => {
	const baseApi = useBaseApi("stoStockOut");
	return {
		// 分页查询出库单主表
		page: baseApi.page,
		// 查看出库单主表详细
		detail: baseApi.detail,
		// 新增出库单主表
		add: baseApi.add,
		// 更新出库单主表
		update: baseApi.update,
		// 删除出库单主表
		delete: baseApi.delete,
		// 批量删除出库单主表
		batchDelete: baseApi.batchDelete,
		// 导出出库单主表数据
		exportData: baseApi.exportData,
		// 导入出库单主表数据
		importData: baseApi.importData,
		// 下载出库单主表数据导入模板
		downloadTemplate: baseApi.downloadTemplate,
		// 审核出库单
		approve: (data?: any) => request.post('/api/stoStockOut/approve', data),
		// 拒绝出库单
		reject: (data?: any) => request.post('/api/stoStockOut/reject', data),
	}
}

// 出库单主表实体
export interface StoStockOut {
	// 主键Id
	id: number;
	// 出库单号
	stockOutNo?: string;
	// 出库类型(Sale销售/Issue领料/Transfer调拨/Other其他)
	stockOutType?: string;
	// 出库仓库ID
	warehouseId?: number;
	// 仓库编码
	warehouseCode?: string;
	// 仓库名称
	warehouseName: string;
	// 来源单据号
	sourceBillNo: string;
	// 出库日期
	stockOutDate?: string;
	// 出库总数量
	totalQuantity?: number;
	// 出库总金额
	totalAmount?: number;
	// 状态(Draft草稿/Approved已审批/Confirmed已确认/Cancelled已取消)
	status?: string;
	// 审批人ID
	approvalUserId: number;
	// 审批时间
	approvalTime: string;
	// 审批意见
	approvalRemark: string;
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