import {useBaseApi} from '/@/api/base';

// 订单变更记录接口服务
export const useSalOrderLogApi = () => {
	const baseApi = useBaseApi("salOrderLog");
	return {
		// 分页查询订单变更记录
		page: baseApi.page,
		// 查看订单变更记录详细
		detail: baseApi.detail,
		// 新增订单变更记录
		add: baseApi.add,
		// 更新订单变更记录
		update: baseApi.update,
		// 删除订单变更记录
		delete: baseApi.delete,
		// 批量删除订单变更记录
		batchDelete: baseApi.batchDelete,
		// 导出订单变更记录数据
		exportData: baseApi.exportData,
		// 导入订单变更记录数据
		importData: baseApi.importData,
		// 下载订单变更记录数据导入模板
		downloadTemplate: baseApi.downloadTemplate,
	}
}

// 订单变更记录实体
export interface SalOrderLog {
	// 主键Id
	id: number;
	// 订单ID
	orderId?: number;
	// 订单号
	orderNo?: string;
	// 变更类型
	changeType?: string;
	// 变更字段
	changeField: string;
	// 原值
	oldValue: string;
	// 新值
	newValue: string;
	// 变更原因
	changeReason: string;
	// 变更时间
	changeTime?: string;
	// 变更人ID
	changeUserId: number;
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