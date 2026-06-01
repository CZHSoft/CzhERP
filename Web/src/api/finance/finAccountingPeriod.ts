import {useBaseApi} from '/@/api/base';

// 会计期间表接口服务
export const useFinAccountingPeriodApi = () => {
	const baseApi = useBaseApi("finAccountingPeriod");
	return {
		// 分页查询会计期间表
		page: baseApi.page,
		// 查看会计期间表详细
		detail: baseApi.detail,
		// 新增会计期间表
		add: baseApi.add,
		// 更新会计期间表
		update: baseApi.update,
		// 删除会计期间表
		delete: baseApi.delete,
		// 批量删除会计期间表
		batchDelete: baseApi.batchDelete,
		// 导出会计期间表数据
		exportData: baseApi.exportData,
		// 导入会计期间表数据
		importData: baseApi.importData,
		// 下载会计期间表数据导入模板
		downloadTemplate: baseApi.downloadTemplate,
	}
}

// 会计期间表实体
export interface FinAccountingPeriod {
	// 主键Id
	id: number;
	// 会计年度
	year?: number;
	// 期间序号
	period?: number;
	// 开始日期
	startDate?: string;
	// 结束日期
	endDate?: string;
	// 期间状态
	status?: string;
	// 是否当前期间
	isCurrent?: boolean;
	// 是否已结账
	isClosed?: boolean;
	// 结账人ID
	closerUserId: number;
	// 结账时间
	closeTime: string;
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