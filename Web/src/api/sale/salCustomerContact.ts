import {useBaseApi} from '/@/api/base';

// 客户联系人接口服务
export const useSalCustomerContactApi = () => {
	const baseApi = useBaseApi("salCustomerContact");
	return {
		// 分页查询客户联系人
		page: baseApi.page,
		// 查看客户联系人详细
		detail: baseApi.detail,
		// 新增客户联系人
		add: baseApi.add,
		// 更新客户联系人
		update: baseApi.update,
		// 删除客户联系人
		delete: baseApi.delete,
		// 批量删除客户联系人
		batchDelete: baseApi.batchDelete,
		// 导出客户联系人数据
		exportData: baseApi.exportData,
		// 导入客户联系人数据
		importData: baseApi.importData,
		// 下载客户联系人数据导入模板
		downloadTemplate: baseApi.downloadTemplate,
	}
}

// 客户联系人实体
export interface SalCustomerContact {
	// 主键Id
	id: number;
	// 客户ID
	customerId?: number;
	// 联系人姓名
	contactName?: string;
	// 职位
	position: string;
	// 电话
	phone: string;
	// 手机
	mobile: string;
	// 邮箱
	email: string;
	// 是否主要联系人
	isPrimary?: boolean;
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