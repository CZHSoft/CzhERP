import {useBaseApi} from '/@/api/base';

// 客户档案接口服务
export const useSalCustomerApi = () => {
	const baseApi = useBaseApi("salCustomer");
	return {
		// 分页查询客户档案
		page: baseApi.page,
		// 查看客户档案详细
		detail: baseApi.detail,
		// 新增客户档案
		add: baseApi.add,
		// 更新客户档案
		update: baseApi.update,
		// 删除客户档案
		delete: baseApi.delete,
		// 批量删除客户档案
		batchDelete: baseApi.batchDelete,
		// 导出客户档案数据
		exportData: baseApi.exportData,
		// 导入客户档案数据
		importData: baseApi.importData,
		// 下载客户档案数据导入模板
		downloadTemplate: baseApi.downloadTemplate,
	}
}

// 客户档案实体
export interface SalCustomer {
	// 主键Id
	id: number;
	// 客户编码
	customerCode?: string;
	// 客户名称
	customerName?: string;
	// 客户简称
	customerShortName: string;
	// 客户类型
	customerType: string;
	// 行业
	industry: string;
	// 信用等级
	creditLevel: string;
	// 信用额度
	creditLimit: number;
	// 信用期限(天)
	creditPeriod: number;
	// 联系人姓名
	contactName: string;
	// 联系电话
	contactPhone: string;
	// 联系邮箱
	contactEmail: string;
	// 地址
	address: string;
	// 省份
	province: string;
	// 城市
	city: string;
	// 邮编
	zipCode: string;
	// 开户银行
	bankName: string;
	// 银行账号
	bankAccount: string;
	// 税号
	taxNo: string;
	// 是否启用
	isEnabled?: boolean;
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