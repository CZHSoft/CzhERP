import {useBaseApi} from '/@/api/base';

// 预算明细表接口服务
export const useFinBudgetDetailApi = () => {
	const baseApi = useBaseApi("finBudgetDetail");
	return {
		// 分页查询预算明细表
		page: baseApi.page,
		// 查看预算明细表详细
		detail: baseApi.detail,
		// 新增预算明细表
		add: baseApi.add,
		// 更新预算明细表
		update: baseApi.update,
		// 删除预算明细表
		delete: baseApi.delete,
		// 批量删除预算明细表
		batchDelete: baseApi.batchDelete,
		// 导出预算明细表数据
		exportData: baseApi.exportData,
		// 导入预算明细表数据
		importData: baseApi.importData,
		// 下载预算明细表数据导入模板
		downloadTemplate: baseApi.downloadTemplate,
		// 获取预算选择器
		getBudgetSelector: () => baseApi.request({ url: "/api/finBudgetDetail/getBudgetSelector" }),
		// 获取科目选择器
		getAccountSelector: () => baseApi.request({ url: "/api/finBudgetDetail/getAccountSelector" }),
	}
}

// 预算明细表实体
export interface FinBudgetDetail {
	// 主键Id
	id: number;
	// 预算ID
	budgetId?: number;
	// 预算期间
	period?: number;
	// 科目ID
	accountId: number;
	// 科目编码
	accountCode: string;
	// 科目名称
	accountName: string;
	// 部门ID
	deptId: number;
	// 部门名称
	deptName: string;
	// 项目ID
	projectId: number;
	// 项目名称
	projectName: string;
	// 预算金额
	budgetAmount?: number;
	// 已执行金额
	executedAmount?: number;
	// 剩余金额
	remainAmount?: number;
	// 预警阈值
	warnThreshold: number;
	// 备注
	remark: string;
}