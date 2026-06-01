import {useBaseApi} from '/@/api/base';

// 预算主表接口服务
export const useFinBudgetApi = () => {
	const baseApi = useBaseApi("finBudget");
	return {
		// 分页查询预算主表
		page: baseApi.page,
		// 查看预算主表详细
		detail: baseApi.detail,
		// 新增预算主表
		add: baseApi.add,
		// 更新预算主表
		update: baseApi.update,
		// 删除预算主表
		delete: baseApi.delete,
		// 批量删除预算主表
		batchDelete: baseApi.batchDelete,
		// 导出预算主表数据
		exportData: baseApi.exportData,
		// 导入预算主表数据
		importData: baseApi.importData,
		// 下载预算主表数据导入模板
		downloadTemplate: baseApi.downloadTemplate,
		// 获取预算类型字典
		getBudgetTypes: () => baseApi.request({ url: "/api/finBudget/getBudgetTypes" }),
		// 获取新预算编号
		getNewBudgetNo: () => baseApi.request({ url: "/api/finBudget/getNewBudgetNo" }),
		// 预算选择器
		selector: () => baseApi.request({ url: "/api/finBudget/selector" }),
	}
}

// 预算主表实体
export interface FinBudget {
	// 主键Id
	id: number;
	// 预算编号
	budgetNo?: string;
	// 预算年度
	budgetYear?: number;
	// 预算名称
	budgetName?: string;
	// 预算类型
	budgetType: string;
	// 预算总额
	totalAmount?: number;
	// 已执行金额
	executedAmount?: number;
	// 剩余金额
	remainAmount?: number;
	// 状态
	status?: string;
	// 版本号
	version?: number;
	// 上级预算ID
	parentBudgetId: number;
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