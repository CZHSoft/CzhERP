import {useBaseApi} from '/@/api/base';

// 客户信用接口服务
export const useSalCreditApi = () => {
	const baseApi = useBaseApi("salCredit");
	return {
		// 分页查询客户信用
		page: baseApi.page,
		// 查看客户信用详细
		detail: baseApi.detail,
		// 新增客户信用
		add: baseApi.add,
		// 更新客户信用
		update: baseApi.update,
		// 删除客户信用
		delete: baseApi.delete,
		// 批量删除客户信用
		batchDelete: baseApi.batchDelete,
		// 导出客户信用数据
		exportData: baseApi.exportData,
		// 导入客户信用数据
		importData: baseApi.importData,
		// 下载客户信用数据导入模板
		downloadTemplate: baseApi.downloadTemplate,
	}
}

// 客户信用实体
export interface SalCredit {
	// 主键Id
	id: number;
	// 客户ID
	customerId?: number;
	// 信用等级
	creditLevel?: string;
	// 信用额度
	creditLimit?: number;
	// 信用期限(天)
	creditPeriod?: number;
	// 已用额度
	usedAmount?: number;
	// 逾期次数
	overdueCount?: number;
	// 最后逾期日期
	lastOverdueDate: string;
	// 评估日期
	assessDate: string;
	// 评估人ID
	assessUserId: number;
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