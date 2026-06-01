import {useBaseApi} from '/@/api/base';
import request from '/@/utils/request';

// 销售出库接口服务
export const useSalOutboundApi = () => {
	const baseApi = useBaseApi("salOutbound");
	return {
		// 分页查询销售出库
		page: baseApi.page,
		// 查看销售出库详细
		detail: baseApi.detail,
		// 新增销售出库
		add: baseApi.add,
		// 更新销售出库
		update: baseApi.update,
		// 删除销售出库
		delete: baseApi.delete,
		// 批量删除销售出库
		batchDelete: baseApi.batchDelete,
		// 导出销售出库数据
		exportData: baseApi.exportData,
		// 导入销售出库数据
		importData: baseApi.importData,
		// 下载销售出库数据导入模板
		downloadTemplate: baseApi.downloadTemplate,
		// 审核销售出库单（简化流程）
		approve: (data?: any) => request.post('/api/salOutbound/approve', data),
		// 拒绝销售出库单
		reject: (data?: any) => request.post('/api/salOutbound/reject', data),
		// 获取状态选项
		getStatusOptions: (data?: any) => request.get('/api/salOutbound/getStatusOptions', { params: data }),
	}
}

// 销售出库实体
export interface SalOutbound {
	// 主键Id
	id: number;
	// 出库单号
	outboundNo?: string;
	// 订单ID
	orderId?: number;
	// 订单号
	orderNo?: string;
	// 客户ID
	customerId?: number;
	// 客户名称
	customerName?: string;
	// 仓库ID
	warehouseId?: number;
	// 仓库名称
	warehouseName?: string;
	// 出库日期
	outboundDate?: string;
	// 配送方式
	shippingMethod: string;
	// 运单号
	trackingNo: string;
	// 出库总数量
	totalQuantity?: number;
	// 出库总金额
	totalAmount?: number;
	// 状态
	status?: string;
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
