import {useBaseApi} from '/@/api/base';
import request from '/@/utils/request';

// 入库单主表接口服务
export const useStoStockInApi = () => {
	const baseApi = useBaseApi("stoStockIn");
	return {
		// 分页查询入库单主表
		page: baseApi.page,
		// 查看入库单主表详细
		detail: baseApi.detail,
		// 新增入库单主表
		add: baseApi.add,
		// 更新入库单主表
		update: baseApi.update,
		// 删除入库单主表
		delete: baseApi.delete,
		// 批量删除入库单主表
		batchDelete: baseApi.batchDelete,
		// 导出入库单主表数据
		exportData: baseApi.exportData,
		// 导入入库单主表数据
		importData: baseApi.importData,
		// 下载入库单主表数据导入模板
		downloadTemplate: baseApi.downloadTemplate,
		// 获取下一个入库单号
		getNextCode: () => request.get<string>("/api/stoStockIn/NextCode"),
		// 入库确认
        confirm: (data: any) => request.post<string>("/api/stoStockIn/Confirm", data),
        // 取消入库确认
        cancelConfirm: (data: any) => request.post<string>("/api/stoStockIn/CancelConfirm", data),
        // 创建模拟入库单
        createMockStockIn: (data: any) => request.post<any>("/api/stoStockIn/CreateMockStockIn", data),
    }
}

// 获取仓库列表接口（用于下拉选择）
export const getWarehouseList = () => {
	return request.get<any[]>("/api/stoWarehouse/List");
}

// 入库单主表实体
export interface StoStockIn {
	// 主键Id
	id: number;
	// 入库单号
	stockInNo?: string;
	// 入库类型(Purchase采购/SaleReturn销退/Transfer调拨/Other其他)
	stockInType?: string;
	// 入库仓库ID
	warehouseId?: number;
	// 仓库编码
	warehouseCode?: string;
	// 仓库名称
	warehouseName: string;
	// 来源单据号
	sourceBillNo: string;
	// 入库日期
	stockInDate?: string;
	// 入库总数量
	totalQuantity?: number;
	// 入库总金额
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